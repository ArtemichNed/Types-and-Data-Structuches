using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace Lab_02
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Программу надо запускать для четной и нечетной размерности матриц по отдельности, изменяя n и имя файла для записи результата
            int n = 101;
            //переменные для замера времени перемножения матриц
            Stopwatch stopWatch1 = new Stopwatch();
            Stopwatch stopWatch2 = new Stopwatch();
            Stopwatch stopWatch3 = new Stopwatch();

            Random rnd = new Random();
            textBox1.Clear();
            //цикл по размерам матриц (100..1000 или 101...1001)
            for (; n < 1002; n += 100)
            {
                textBox1.Text += "Матрица " + n + "x" + n + "\r\n";
                int[,] A = new int[n, n];
                int[,] B = new int[n, n];
                int[,] C1 = new int[n, n];
                int[,] C2 = new int[n, n];
                int[,] C3 = new int[n, n];

                //Генерируем матрицы случайных чисел         
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        A[i, j] = rnd.Next(0, 9);
                        B[i, j] = rnd.Next(0, 9);
                    }
                }
                //переменные времени в миллисекундах
                long ts1 = 0;
                long ts2 = 0;
                long ts3 = 0;
                for (int i = 0; i < 10; i++)
                {
                    stopWatch1.Start();
                    C1 = Simple(A, B, n);
                    stopWatch1.Stop();
                    ts1 += stopWatch1.ElapsedMilliseconds;

                    stopWatch2.Start();
                    C2 = Vinograd(A, B, n);
                    stopWatch2.Stop();
                    ts2 += stopWatch2.ElapsedMilliseconds;

                    stopWatch3.Start();
                    C3 = BetterVinograd(A, B, n);
                    stopWatch3.Stop();
                    ts3 += stopWatch3.ElapsedMilliseconds;
                }
                //находим среднее время
                ts1 /= 10;
                ts2 /= 10;
                ts3 /= 10;

                textBox1.Text += "\t" + "Стандартный метод  " + ts1.ToString() + " мс \r\n\t" + "Метод Винограда  " + ts2.ToString()
                    + " мс \r\n\t" + "Улучшенный метод Винограда  " + ts3.ToString() + " мс \r\n";
            }
            //считываем результат из окна в строку и записываем в файл
            string str;
            str = textBox1.Text;
            //чистим сначала текстовый файл
            File.WriteAllText(@"test.txt", string.Empty);

            // запись в файл полученных массивов точек
            using (FileStream fstream = new FileStream(@"test.txt", FileMode.OpenOrCreate))
            {
                // преобразуем строку в байты
                byte[] array = System.Text.Encoding.Default.GetBytes(str);
                // запись массива байтов в файл
                fstream.Write(array, 0, array.Length);
            }
        }
        //стадартный метод умножения матриц
        private int[,] Simple(int[,] A, int[,] B, int n)
        {
            int[,] C = new int[n, n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    C[i, j] = 0;
                    for (int k = 0; k < n; k++)
                    {
                        C[i, j] += A[i, k] * B[k, j];
                    }
                }
            }

            return C;
        }
        //умножение матриц по Винограду
        private int[,] Vinograd(int[,] A, int[,] B, int n)
        {
            int[,] C = new int[n, n];
            int[] rowFactor = new int[n];
            int[] colFactor = new int[n];
            int d = n / 2;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < d; j++)
                {
                    rowFactor[i] += A[i, 2 * j] * A[i, 2 * j + 1];
                }
            }
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < d; j++)
                {
                    colFactor[i] += B[2 * j, i] * B[2 * j + 1, i];
                }
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    C[i, j] = -rowFactor[i] - colFactor[j];
                    for (int k = 0; k < d; k++)
                    {
                        C[i, j] += (A[i, 2 * k] + B[2 * k + 1, j]) * (A[i, 2 * k + 1] + B[2 * k, j]);
                    }
                }
            }
            if (n % 2 != 0)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        C[i, j] += A[i, n - 1] * B[n - 1, j];
                    }
                }
            }
            return C;
        }
        //умножение матриц по улучшенному методу Винограду
        private int[,] BetterVinograd(int[,] A, int[,] B, int n)
        {
            int[,] C = new int[n, n];
            int[] rowFactor = new int[n];
            int[] colFactor = new int[n];
            int d = n / 2;
            int buf = 0;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < d; j++)
                {
                    rowFactor[i] += A[i, 2 * j] * A[i, 2 * j + 1];
                }
            }
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < d; j++)
                {
                    colFactor[i] += B[2 * j, i] * B[2 * j + 1, i];
                }
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    buf = -rowFactor[i] - colFactor[j];

                    for (int k = 1; k < n; k += 2)
                    {
                        buf += (A[i, k - 1] + B[k, j]) * (A[i, k] + B[k - 1, j]);

                    }
                    C[i, j] = buf;
                }
            }
            if (n % 2 != 0)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        C[i, j] += A[i, n - 1] * B[n - 1, j];
                    }
                }
            }
            return C;
        }

    }
}
