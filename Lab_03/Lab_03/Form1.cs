using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab_03
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox17_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int n = 6;
            int[,] A = { { 0, 10, 0, 20, 0, 0 }, { 0, 11, 100, 0, 12, 0 }, 
                         { 30, 0, 0, 0, 13, 0 }, { 0, 0, 0, 15, 0, 0 }, 
                         { 0, 0, 0, 0, 0, 0 }, { 40, 14, 0, 0, 0, 0 } };


            int[,] C1 = new int[n, n]; //контрольное произведение
            int[,] C2 = new int[n, n]; //контрольная сумма

            int[,] C11 = new int[n, n]; //контрольное произведение
            int[,] C21 = new int[n, n]; //контрольная сумма

            C1 = Simple(A, A, n);
            C2 = Summa(A, A, n);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    textBox1.Text += A[i, j].ToString();
                    textBox2.Text += C1[i, j].ToString();
                    textBox3.Text += C2[i, j].ToString();
                    if (j != n - 1) textBox1.Text += "     ";
                    if (j != n - 1) textBox2.Text += "     ";
                    if (j != n - 1) textBox3.Text += "     ";
                }
                textBox1.Text += "\r\n";
                textBox2.Text += "\r\n";
                textBox3.Text += "\r\n";
            }
            //составляем массиа ненулевых элементов
            int k = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (A[i, j] != 0) k++;
                }
            }
            int[] AN = new int[k];
            int[] IA = new int[k];
            int[] JA = new int[k];
            int[] NR = new int[k];
            int[] NC = new int[k];
            int[] JR = new int[n];
            int[] JC = new int[n];

            int K = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (A[i, j] != 0)
                    {
                        AN[K] = A[i, j];
                        IA[K] = i;
                        JA[K] = j;
                        textBox4.Text += AN[K] + " ";
                        textBox5.Text += IA[K] + " ";
                        textBox6.Text += JA[K] + " ";
                        K++;
                    };
                }
            }

            NR = NextRow(A, n, k);
            for (int i = 0; i < k; i++) { textBox7.Text += NR[i] + " "; }
            NC = NextCollom(A, AN, n, k);
            for (int i = 0; i < k; i++) { textBox8.Text += NC[i] + " "; }
            JR = JRow(A, AN, n, k);
            for (int i = 0; i < n; i++) { textBox9.Text += JR[i] + " "; }
            JC = JColumn(A, AN, n, k);
            for (int i = 0; i < n; i++) { textBox10.Text += JC[i] + " "; }

            int[] NR2 = new int[k];
            int[] NC2 = new int[k];
            NR2 = NextRow2(A, AN, n, k);
            for (int i = 0; i < k; i++) { textBox11.Text += NR2[i] + " "; }
            NC2 = NextCollom2(A, AN, n, k);
            for (int i = 0; i < k; i++) { textBox12.Text += NC2[i] + " "; }

            List<int> ANCol = new List<int>();
            List<int> ANRowOfCol = new List<int>();
            List<int> ANRow = new List<int>();
            List<int> ANColOfRow = new List<int>();

            for (int i = 0; i < n; i++)
            {
                ANRow = GetRow(JR, AN, NR2, n, i);
                //for (int i = 0; i < ANRow.Count; i++) { textBox15.Text += ANRow[i].ToString() + " "; }

                ANColOfRow = GetColOfRow(ANRow, JC, AN, NC2, JR, n, i);
                // for (int i = 0; i < ANColOfRow.Count; i++) { textBox16.Text += ANColOfRow[i].ToString() + " "; }

                for (int j = 0; j < n; j++)
                {
                    ANCol = GetColumn(JC, AN, NC2, n, j);
                    //for (int i = 0; i < ANCol.Count; i++) { textBox13.Text += ANCol[i].ToString() + " "; }

                    ANRowOfCol = GetRowOfCol(ANCol, JC, AN, NR2, JR, n, j);
                    //for (int i = 0; i < ANRowOfCol.Count; i++) { textBox14.Text += ANRowOfCol[i].ToString() + " "; }

                    C11[i, j] = Proiz1(ANCol, ANRowOfCol, ANRow, ANColOfRow);
                    C21[i, j] = Sum1(ANCol, ANRowOfCol, ANRow, ANColOfRow, i, j);
                    textBox17.Text += C11[i, j].ToString();
                    textBox18.Text += C21[i, j].ToString();
                    if (j != n - 1) textBox17.Text += "     ";
                    if (j != n - 1) textBox18.Text += "     ";
                }
                textBox17.Text += "\r\n";
                textBox18.Text += "\r\n";
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
        private int[,] Summa(int[,] A, int[,] B, int n)
        {
            int[,] C = new int[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    C[i, j] = A[i, j] + B[i, j];
                }
            }
            return C;
        }
        private int[,] Summa2(int[,] A, int[,] B, int n, int m)
        {
            int[,] C = new int[m, n];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    C[i, j] = A[i, j] + B[i, j];
                }
            }
            return C;
        }
        private int[] NextRow(int[,] A, int n, int k)
        {
            int[] NR = new int[k];
            int K = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (A[i, j] != 0 && j != n - 1)
                    {
                        for (int p = j + 1; p < n; p++)
                        {
                            if (p == n - 1 && A[i, p] != 0)
                            {
                                NR[K] = K + 1;
                                K++;
                                break;
                            }
                            if (A[i, p] != 0)
                            {
                                NR[K] = K + 1;
                                K++;
                                break;
                            }
                            if (A[i, p] == 0 && p == n - 1)
                            {
                                NR[K] = -1;
                                K++;
                            }
                        }
                    };
                    if (A[i, j] != 0 && j == n - 1)
                    {
                        NR[K] = -1;
                        K++;
                    }
                }
            }
            return NR;
        }
        private int[] NextCollom(int[,] A, int[] AN, int n, int k)
        {
            int[] NC = new int[k];
            int K = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (A[i, j] != 0 && i != n - 1)
                    {
                        for (int p = i + 1; p < n; p++)
                        {
                            if (p == n - 1 && A[p, j] != 0)
                            {
                                NC[K] = Array.IndexOf(AN, A[p, j]);
                                K++;
                                break;
                            }
                            if (A[p, j] != 0)
                            {
                                NC[K] = Array.IndexOf(AN, A[p, j]);
                                K++;
                                break;
                            }
                            if (A[p, j] == 0 && p == n - 1)
                            {
                                NC[K] = -1;
                                K++;
                            }
                        }
                    };
                    if (A[i, j] != 0 && i == n - 1)
                    {
                        NC[K] = -1;
                        K++;
                    }
                }
            }
            return NC;
        }
        private int[] JRow(int[,] A, int[] AN, int n, int k)
        {
            int[] JR = new int[n];
            int K = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; )
                {
                    if (A[i, j] == 0 && j == n - 1)
                    {
                        JR[K] = -1;
                        K++;
                        break;
                    }
                    if (A[i, j] != 0)
                    {
                        JR[K] = Array.IndexOf(AN, A[i, j]);
                        K++;
                        break;
                    }
                    j++;
                }
            }
            return JR;

        }
        private int[] JColumn(int[,] A, int[] AN, int n, int k)
        {
            int[] JC = new int[n];
            int K = 0;
            for (int j = 0; j < n; j++)
            {
                for (int i = 0; i < n; )
                {
                    if (A[i, j] == 0 && i == n - 1)
                    {
                        JC[K] = -1;
                        K++;
                        break;
                    }
                    if (A[i, j] != 0)
                    {
                        JC[K] = Array.IndexOf(AN, A[i, j]);
                        K++;
                        break;
                    }
                    i++;
                }
            }
            return JC;
        }
        private int[] NextRow2(int[,] A, int[] AN, int n, int k)
        {
            int[] NR = new int[k];
            int K = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (A[i, j] != 0 && j != n - 1)
                    {
                        for (int p = j + 1; p < n; p++)
                        {
                            if (p == n - 1 && A[i, p] != 0)
                            {
                                NR[K] = Array.IndexOf(AN, A[i, p]);
                                K++;
                                break;
                            }
                            if (A[i, p] != 0)
                            {
                                NR[K] = Array.IndexOf(AN, A[i, p]);
                                K++;
                                break;
                            }
                            if (A[i, p] == 0 && p == n - 1)
                            {
                                for (int h = 0; h < n - 1; h++)
                                {
                                    if (A[i, h] != 0)
                                    {
                                        NR[K] = Array.IndexOf(AN, A[i, h]);
                                        K++;
                                        break;
                                    }
                                }
                            }
                        }
                    };
                    if (A[i, j] != 0 && j == n - 1)
                    {
                        for (int h = 0; h < n - 1; h++)
                        {
                            if (A[i, h] != 0)
                            {
                                NR[K] = Array.IndexOf(AN, A[i, h]);
                                K++;
                                break;
                            }
                        }
                    }
                }
            }
            return NR;
        }
        private int[] NextCollom2(int[,] A, int[] AN, int n, int k)
        {
            int[] NC = new int[k];
            int K = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (A[i, j] != 0 && i != n - 1)
                    {
                        for (int p = i + 1; p < n; p++)
                        {
                            if (p == n - 1 && A[p, j] != 0)
                            {
                                NC[K] = Array.IndexOf(AN, A[p, j]);
                                K++;
                                break;
                            }
                            if (A[p, j] != 0)
                            {
                                NC[K] = Array.IndexOf(AN, A[p, j]);
                                K++;
                                break;
                            }
                            if (A[p, j] == 0 && p == n - 1)
                            {
                                for (int h = 0; h < n - 1; h++)
                                {
                                    if (A[h, j] != 0)
                                    {
                                        NC[K] = Array.IndexOf(AN, A[h, j]);
                                        K++;
                                        break;
                                    }
                                }
                            }
                        }
                    };
                    if (A[i, j] != 0 && i == n - 1)
                    {
                        for (int h = 0; h < n - 1; h++)
                        {
                            if (A[h, j] != 0)
                            {
                                NC[K] = Array.IndexOf(AN, A[h, j]);
                                K++;
                                break;
                            }
                        }
                    }
                }
            }
            return NC;
        }
        private List<int> GetColumn(int[] JC, int[] AN, int[] NC, int n, int num)
        {
            List<int> ColCur = new List<int>();
            int index = JC[num];
            if (index == -1) return ColCur;
            ColCur.Insert(0, AN[JC[num]]);
            for (int i = 1; i < n; i++)
            {
                index = NC[index];
                if (index == JC[num]) break;
                ColCur.Insert(i, AN[index]);
                if (JC[num] == NC[index]) break;
            }
            return ColCur;
        }
        private List<int> GetRowOfCol(List<int> ANCol, int[] JC, int[] AN, int[] NR, int[] JR, int n, int num)
        {
            List<int> RowCur = new List<int>();
            int index = JC[num];
            if (index == -1) return RowCur;
            //int index;
            bool OK = false;
            for (int h = 0; h < ANCol.Count; h++)
            {
                index = Array.IndexOf(AN, ANCol[h]);
                for (int j = 0; j < 10; j++)
                {
                    OK = false;
                    for (int i = 0; i < n; i++)
                    {
                        if (index == JR[i])
                        {
                            RowCur.Add(Array.IndexOf(JR, index));
                            OK = true;
                            break;
                        }
                    }
                    if (OK) break;
                    index = NR[index];
                }
            }
            return RowCur;
        }
        private List<int> GetRow(int[] JR, int[] AN, int[] NR, int n, int num)
        {
            List<int> RowCur = new List<int>();
            int index = JR[num];
            if (index == -1) return RowCur;
            RowCur.Insert(0, AN[JR[num]]);
            for (int i = 1; i < n; i++)
            {
                index = NR[index];
                if (index == JR[num]) break;
                RowCur.Insert(i, AN[index]);
                if (JR[num] == NR[index]) break;
            }
            return RowCur;
        }
        private List<int> GetColOfRow(List<int> ANRow, int[] JC, int[] AN, int[] NC, int[] JR, int n, int num)
        {
            List<int> ColCur = new List<int>();
            int index = JR[num];
            if (index == -1) return ColCur;
            //int index;
            bool OK = false;
            for (int h = 0; h < ANRow.Count; h++)
            {
                index = Array.IndexOf(AN, ANRow[h]);
                for (int j = 0; j < 10; j++)
                {
                    OK = false;
                    for (int i = 0; i < n; i++)
                    {
                        if (index == JC[i])
                        {
                            ColCur.Add(Array.IndexOf(JC, index));
                            OK = true;
                            break;
                        }
                    }
                    if (OK) break;
                    index = NC[index];
                }
            }
            return ColCur;
        }
        private int Proiz1(List<int> ANCol, List<int> ANRowOfCol, List<int> ANRow, List<int> ANColOfRow)
        {
            int Proiz = 0;
            int i = 0, j = 0;
            while (j != ANColOfRow.Count && i != ANRowOfCol.Count)
            {
                if (ANRowOfCol[i] > ANColOfRow[j])
                {
                    j++;
                    continue;
                }
                if (ANRowOfCol[i] < ANColOfRow[j])
                {
                    i++;
                    continue;
                }
                if (ANRowOfCol[i] == ANColOfRow[j])
                {
                    Proiz += ANCol[i] * ANRow[j];
                    i++;
                    j++;
                }
            }
            return Proiz;
        }
        private int Sum1(List<int> ANCol, List<int> ANRowOfCol, List<int> ANRow, List<int> ANColOfRow, int row, int col)
        {
            int Sum = 0;
            int i = 0, j = 0;
            while (j != ANColOfRow.Count)
            {
                if (ANColOfRow[j] == col)
                {
                    Sum += ANRow[j];
                    break;
                }
                j++;
            }
            while (i != ANRowOfCol.Count)
            {
                if (ANRowOfCol[i] == row)
                {
                    Sum += ANCol[i];
                    break;
                }
                i++;
            }
            return Sum;
        }
        private List<int> Sum2(List<int> ANRow1, List<int> ANColOfRow1, List<int> ANRow2, List<int> ANColOfRow2)
        {
            List<int> Sum = new List<int>();
            int i = 0, j = 0;
            int h = 0;
            while (j != ANColOfRow2.Count && i != ANColOfRow1.Count)
            {
                if (ANColOfRow1[i] > ANColOfRow2[j])
                {
                    Sum.Add(ANRow2[j]);
                    h++;
                    j++;
                    continue;
                }
                if (ANColOfRow1[i] < ANColOfRow2[j])
                {
                    Sum.Add(ANRow1[i]);
                    h++;
                    i++;
                    continue;
                }
                if (ANColOfRow1[i] == ANColOfRow2[j])
                {
                    Sum.Add(ANRow1[i] + ANRow2[j]);
                    h++;
                    i++;
                    j++;
                    //break;
                }
            }
            return Sum;
        }
        private int Sum3(List<int> ANRow1, List<int> ANColOfRow1, List<int> ANRow2, List<int> ANColOfRow2)
        {
            int Sum = 0;
            int i = 0, j = 0;
            while (j != ANColOfRow2.Count && i != ANColOfRow1.Count)
            {
                if (ANColOfRow1[i] > ANColOfRow2[j])
                {
                    j++;
                    continue;
                }
                if (ANColOfRow1[i] < ANColOfRow2[j])
                {
                    i++;
                    continue;
                }
                if (ANColOfRow1[i] == ANColOfRow2[j])
                {
                    Sum = ANRow1[i] + ANRow2[j];
                    i++;
                    j++;
                    //break;
                }
            }
            return Sum;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int[,] B = { { 0, 0, 1, 3, 0, 0, 0, 5, 0, 0 }, 
                         { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 
                         { 0, 0, 0, 0, 0, 7, 0, 1, 0, 0 } };
            //Метод Чанга и Густовса
            int n = 10;
            int m = 3;
            int[,] B2 = new int[m, n]; //контрольная сумма
            int[,] B21 = new int[m, n];
            B2 = Summa2(B, B, n, m);

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    textBox20.Text += B[i, j].ToString();
                    textBox21.Text += B2[i, j].ToString();
                    //textBox3.Text += C2[i, j].ToString();
                    if (j != n - 1) textBox20.Text += "     ";
                    if (j != n - 1) textBox21.Text += "     ";
                    //if (j != n - 1) textBox3.Text += "     ";
                }
                textBox20.Text += "\r\n";
                textBox21.Text += "\r\n";
                //textBox3.Text += "\r\n";
            }
            //составляем массив ненулевых элементов
            int k = 0;
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (B[i, j] != 0) k++;
                }
            }
            int[] AN = new int[k];
            int[] JA = new int[k];
            int[] JR = new int[m + 1];
            int h = 1;
            JR[0] = 0;
            textBox24.Text += JR[0] + " ";

            int K = 0;
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (B[i, j] != 0)
                    {
                        AN[K] = B[i, j];
                        JA[K] = j;
                        textBox22.Text += AN[K] + " ";
                        textBox23.Text += JA[K] + " ";
                        K++;
                    };
                }
                JR[h] = K;
                textBox24.Text += JR[h] + " ";
                h++;
            }

            List<int> AN1 = new List<int>();
            List<int> JA1 = new List<int>();
            List<int> AN2 = new List<int>();
            List<int> JA2 = new List<int>();
            int index = 0;

            for (int i = 0; i < m; i++)
            {
                //int[] row = new int[n];

                int raznica = JR[i + 1] - JR[i];
                if (raznica == 0)
                {
                    for (int j = 0; j < n; j++)
                    {
                        B21[i, j] = 0;
                        textBox27.Text += B21[i, j].ToString() + "     ";
                    }
                    textBox27.Text += "\r\n";
                    continue;
                }

                AN1.Clear();
                JA1.Clear();
                AN2.Clear();
                JA2.Clear();

                AN1 = GetAN(AN, JA, JR, raznica, index);
                //for (int i = 0; i < ANCur.Count; i++) { textBox25.Text += ANCur[i].ToString() + " "; }
                AN2 = GetAN(AN, JA, JR, raznica, index);
                JA1 = GetJA(AN, JA, JR, raznica, index);
                // for (int i = 0; i < JACur.Count; i++) { textBox26.Text += JACur[i].ToString() + " "; }
                JA2 = GetJA(AN, JA, JR, raznica, index);

                AN1.Add(0);
                AN2.Add(0);
                JA1.Add(0);
                JA2.Add(0);

                int p = 0;
                int l = 0;
                for (int j = 0; j < n; j++)
                {
                    if (j != JA1[p] && j != JA2[l]) B21[i, j] = 0;
                    if (j == JA1[p] && j != JA2[l]) { B21[i, j] = AN1[p]; p++; };
                    if (j != JA1[p] && j == JA2[l]) { B21[i, j] = AN2[l]; l++; };
                    if (j == JA1[p] && j == JA2[l]) { B21[i, j] = AN1[p] + AN2[l]; l++; p++; };
                    textBox27.Text += B21[i, j].ToString() + "     ";
                }
                index += raznica;
                textBox27.Text += "\r\n";
            }
        }
        private List<int> GetAN(int[] AN, int[] JA, int[] JR, int n, int index)
        {
            List<int> ANCur = new List<int>();
            for (int i = index; i < index + n; i++)
            {
                ANCur.Add(AN[i]);

            }
            return ANCur;
        }
        private List<int> GetJA(int[] AN, int[] JA, int[] JR, int n, int index)
        {
            List<int> JACur = new List<int>();
            for (int i = index; i < index + n; i++)
            {
                JACur.Add(JA[i]);

            }
            return JACur;
        }
    }
}
