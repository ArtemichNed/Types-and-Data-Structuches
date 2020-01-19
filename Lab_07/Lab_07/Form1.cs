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

namespace Lab_07
{
    public partial class Form1 : Form
    {
        static int n = 19;
        int[,] matrix = new int[n, n];

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] lines = File.ReadAllLines(@"matrix2.txt");

            for (int i = 0; i < lines.Length; i++)
            {
                string[] temp = lines[i].Split(' ');
                for (int j = 0; j < temp.Length; j++)
                    matrix[i, j] = Convert.ToInt32(temp[j]);
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    textBox1.Text += matrix[i, j].ToString() + "   ";
                }
                textBox1.Text += "\r\n";
            }
            //Осуществляем поиск в глубину
            textBox3.Clear();
            int u = int.Parse(textBox2.Text);
            var result = DFS(u, matrix);
            foreach (int i in result)
            {
                { textBox3.Text += i.ToString() + " "; }
            }
        }
        private HashSet<int> DFS(int index, int[,] matrix)
        {
            var list = new HashSet<int>();
            list.Add(index);
            for (int j = 0; j < matrix.GetLength(0); j++)
            {
                if (matrix[index, j] == 1)
                {
                    matrix[index, j] = matrix[j, index] = 0;
                    list.UnionWith(DFS(j, matrix));
                }
            }
            return list;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string[] lines = File.ReadAllLines(@"matrix2.txt");
            // int[,]  C = new int[lines.Length, lines[0].Split(' ').Length];
            for (int i = 0; i < lines.Length; i++)
            {
                string[] temp = lines[i].Split(' ');
                for (int j = 0; j < temp.Length; j++)
                    matrix[i, j] = Convert.ToInt32(temp[j]);
            }
            textBox4.Clear();
            Queue<int> q = new Queue<int>();    //Это очередь для вершин
            //int n = 19;
            int u = int.Parse(textBox2.Text);
            bool[] used = new bool[n];  //массив с посещенными вершинами
            List<int> list = new List<int>();
            list.Add(u);
            used[u] = true;
            q.Enqueue(u);
            while (q.Count != 0)
            {
                u = q.Peek();
                q.Dequeue();

                for (int i = 0; i < 19; i++)
                {
                    if (Convert.ToBoolean(matrix[u, i]))
                    {
                        if (!used[i])
                        {
                            used[i] = true;
                            list.Add(i);
                            q.Enqueue(i);
                        }
                    }
                }
            }

            for (int i = 0; i < list.Count; i++) textBox4.Text += list[i].ToString() + " ";
        }
    }
}
