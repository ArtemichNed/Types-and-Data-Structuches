using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab_01
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string s = textBox1.Text;
            string t = textBox2.Text;

            int m = s.Length, n = t.Length;

            int[,] mas = new int[m + 1, n + 1];

            for (int i = 1; i < m + 1; ++i)
            {
                mas[i, 0] = i;
            }

            for (int j = 1; j < n + 1; ++j)
            {
                mas[0, j] = j;
            }

            for (int j = 1; j < n + 1; ++j)
            {
                for (int i = 1; i < m + 1; ++i)
                {
                    if (s[i - 1] == t[j - 1])
                    {
                        // Совпадение симвовлов
                        mas[i, j] = mas[i - 1, j - 1];
                    }
                    else if ((i - 2) > 0 && (j - 2) > 0 && s[i - 2] == t[j - 1] && s[i - 1] == t[j - 2])
                    {
                        // Расстояние Дамерау-Левенштейна
                        // Минимум между удалением, вставкой и заменой и перестановкой
                        mas[i, j] = Math.Min(mas[i - 2, j - 2], Math.Min(mas[i - 1, j] + 1,
                            Math.Min(mas[i, j - 1] + 1, mas[i - 1, j - 1] + 1)));
                    }
                    else
                    {
                        // Расстояние Левенштейна
                        // Минимум между удалением, вставкой и заменой
                        mas[i, j] = Math.Min(mas[i - 1, j] + 1,
                            Math.Min(mas[i, j - 1] + 1, mas[i - 1, j - 1] + 1));
                    }
                }
            }
            textBox3.Text += mas[m, n].ToString();


            for (int j = 0; j < n + 1; ++j)
            {
                for (int i = 0; i < m + 1; ++i)
                {
                    textBox4.Text += mas[i, j].ToString() + "  ";
                }
                textBox4.Text += "\r\n";
            }

        }
    }
}
