using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace GameSudoku
{
    public partial class SudokuForm : Form
    {
        Button[,] btnBacktracking = new Button[Cons.n, Cons.n];
        public SudokuForm()
        {
            InitializeComponent();
            drawBoard();

        }
        void drawBoard()
        {
            for (int i = 0; i < Cons.n; i++)
            {
                for (int j = 0; j < Cons.n; j++)
                {
                    btnBacktracking[i, j] = new Button();
                    btnBacktracking[i, j].Size = new Size(Cons.btn_size, Cons.btn_size);
                    btnBacktracking[i, j].Text = "";
                    btnBacktracking[i, j].Font = new Font("Microsoft Sans Serif", 15, FontStyle.Bold);
                    btnBacktracking[i, j].ForeColor = Color.White;
                    btnBacktracking[i, j].Location = new Point(j * Cons.btn_size, i * Cons.btn_size);
                    if (((i < 3 || i > 5) && (j > 2 && j < 6)) || ((j < 3 || j > 5) && (i > 2 && i < 6)))
                    {
                        btnBacktracking[i, j].BackColor = Color.SaddleBrown;
                    }
                    btnBacktracking[i, j].Click += new EventHandler(btnClick);
                    panel.Controls.Add(btnBacktracking[i, j]);
                }
            }
        }
        private static bool isValid(Button[,] btnBacktracking, int row, int col, char c)
        {
            for (int i = 0; i < Cons.n; i++)
            {
                //check row
                if (btnBacktracking[i, col].Text != "" && btnBacktracking[i, col].Text == c.ToString())
                    return false;
                //check column
                if (btnBacktracking[row, i].Text != "" && btnBacktracking[row, i].Text == c.ToString())
                    return false;
                //check 3x3 block
                if (btnBacktracking[3 * (row / 3) + i / 3, 3 * (col / 3) + i % 3].Text != "" && btnBacktracking[3 * (row / 3) + i / 3, 3 * (col / 3) + i % 3].Text == c.ToString())
                    return false;
            }
            return true;
        }
        private bool solve(Button[,] btnBacktracking)
        {
            for (int i = 0; i < btnBacktracking.GetLength(0); i++)
            {
                for (int j = 0; j < btnBacktracking.GetLength(1); j++)
                {
                    if (btnBacktracking[i, j].Text == "")
                    {
                        for (char c = '1'; c <= '9'; c++)
                        {
                            if (isValid(btnBacktracking, i, j, c))
                            {
                                Demo(i, j, c);
                                btnBacktracking[i, j].Text = c.ToString();
                                if (solve(btnBacktracking)) return true;
                                else
                                {
                                    btnBacktracking[i, j].Text = "";
                                }
                            }
                        }
                        return false;
                    }
                }
            }
            return true;
        }
        Form1 f = new Form1()
        {
            Location = new Point(0, 0)
        };
        public void Demo(int i, int j, char c)
        {
            Thread.Sleep(10);
            f.ShowDialog();
            //btnBacktracking[i, j].Text = c.ToString();
        }
        private bool checkInput()
        {
            for (int i = 0; i < Cons.n; i++)
            {
                for (int j = 0; j < Cons.n; j++)
                {
                    if (btnBacktracking[i, j].Text != "")
                    {
                        for (int k = i + 1; k < 9; ++k)  // kiem tra theo hang
                        {
                            if (btnBacktracking[i, j].Text != "")
                            {
                                if (btnBacktracking[i, j].Text == btnBacktracking[k, j].Text)
                                    return false;
                            }
                        }
                        for (int k = 0; k < i; ++k)  // kiem tra theo hang
                        {
                            if (btnBacktracking[i, j].Text != "")
                            {
                                if (btnBacktracking[i, j].Text == btnBacktracking[k, j].Text)
                                    return false;
                            }
                        }
                        for (int k = j + 1; k < 9; ++k)  // kiem tra theo hang
                        {
                            if (btnBacktracking[i, j].Text != "")
                            {
                                if (btnBacktracking[i, j].Text == btnBacktracking[i, k].Text)
                                    return false;
                            }
                        }
                        for (int k = 0; k < j; ++k)  // kiem tra theo hang
                        {
                            if (btnBacktracking[i, j].Text != "")
                            {
                                if (btnBacktracking[i, j].Text == btnBacktracking[i, k].Text)
                                    return false;
                            }
                        }
                        int boxRowOffset = (i / 3) * 3;
                        int boxColOffset = (j / 3) * 3;

                        for (int k = 0; k < 3; ++k) //kiem tra trong 9 ô nhỏ
                            for (int m = 0; m < 3; ++m)
                                if ((boxRowOffset + k) != i && boxColOffset + m != j)
                                {
                                    if (btnBacktracking[boxRowOffset + k, boxColOffset + m].Text != "")
                                    {
                                        if (btnBacktracking[i, j].Text == btnBacktracking[boxRowOffset + k, boxColOffset + m].Text)
                                            return false;
                                    }
                                }
                    }
                }
            }
            return true;
        }
        private void solveBtn_Click(object sender, EventArgs e)
        {
            timer.Start();
            timer1.Start();
            if (checkInput())
            {
                for (int i = 0; i < Cons.n; i++)
                {
                    for (int j = 0; j < Cons.n; j++)
                    {
                        if (btnBacktracking[i, j].Text == "")
                            btnBacktracking[i, j].ForeColor = Color.LightYellow;
                    }
                }
                if (!solve(btnBacktracking)) MessageBox.Show("Can't solve");
            }
            else
            {
                MessageBox.Show("Please check the input!");
            }
            timer.Stop();
            timer1.Stop();

        }
        private void btnClick(object sender, EventArgs e)
        {
            int num;
            Button btn = (Button)sender;
            int y = (btn.Location.X) / Cons.btn_size;
            int x = (btn.Location.Y) / Cons.btn_size;
            if (btnBacktracking[x, y].Text == "") num = 0;
            else num = int.Parse(btnBacktracking[x, y].Text);

            num = (num + 1) % 10;
            if (num == 0) btnBacktracking[x, y].Text = "";
            else btnBacktracking[x, y].Text = num.ToString();
        }
        private void Clear()
        {
            for (int i = 0; i < Cons.n; i++)
            {
                for (int j = 0; j < Cons.n; j++)
                {
                    btnBacktracking[i, j].Text = "";
                    btnBacktracking[i, j].ForeColor = Color.Cyan;
                }
            }
        }
        private void newBtn_Click(object sender, EventArgs e)
        {
            Clear();
            labeltick.Text = "0";
        }

        private void importBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "txt file|*.txt";
            if (op.ShowDialog() == DialogResult.OK)
            {
                Clear();
                string filename = op.FileName;
                string[] filelines = File.ReadAllLines(filename);
                if (filelines.Length == 9)
                {
                    for (int j = 0; j < filelines.Length; j++)
                    {
                        string[] splitLines = filelines[j].Split(' ');
                        if (splitLines.Length == 9)
                        {
                            for (int i = 0; i < splitLines.Length; i++)
                            {
                                if (int.Parse(splitLines[i]) >= 0 && int.Parse(splitLines[i]) <= 9)
                                {
                                    if (int.Parse(splitLines[i]) == 0)
                                    {
                                        btnBacktracking[j, i].Text = "";
                                    }
                                    else
                                    {
                                        btnBacktracking[j, i].Text = splitLines[i];
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Error Reading File");
                                    Clear();
                                    return;
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Error Reading File");
                            Clear();
                            return;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Error Reading File");
                    Clear();
                    return;
                }
            }
        }
        int second = 0;
        int tick = 0;
        private void timer_Tick(object sender, EventArgs e)
        {
            tick++;
            labeltick.Text = tick.ToString();
            if (tick == 60)
            {
                second++;
                tick = 0;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            f.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}