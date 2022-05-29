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
        Button[,] btnSudoku = new Button[Cons.n, Cons.n];

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
                    btnSudoku[i, j] = new Button();
                    btnSudoku[i, j].Size = new Size(Cons.btn_size, Cons.btn_size);
                    btnSudoku[i, j].Text = "";
                    btnSudoku[i, j].Font = new Font("Microsoft Sans Serif", 15, FontStyle.Bold);
                    btnSudoku[i, j].ForeColor = Color.White;
                    btnSudoku[i, j].Location = new Point(j * Cons.btn_size, i * Cons.btn_size);
                    if (((i < 3 || i > 5) && (j > 2 && j < 6)) || ((j < 3 || j > 5) && (i > 2 && i < 6)))
                    {
                        btnSudoku[i, j].BackColor = Color.SaddleBrown;
                    }
                    btnSudoku[i, j].Click += new EventHandler(btnClick);
                    panel.Controls.Add(btnSudoku[i, j]);
                }
            }
        }
        private static bool isValid(Button[,] btnSudoku, int row, int col, char c)
        {
            for (int i = 0; i < Cons.n; i++)
            {
                //check row
                if (btnSudoku[i, col].Text != "" && btnSudoku[i, col].Text == c.ToString())
                    return false;
                //check column
                if (btnSudoku[row, i].Text != "" && btnSudoku[row, i].Text == c.ToString())
                    return false;
                //check 3x3 block
                if (btnSudoku[3 * (row / 3) + i / 3, 3 * (col / 3) + i % 3].Text != "" && btnSudoku[3 * (row / 3) + i / 3, 3 * (col / 3) + i % 3].Text == c.ToString())
                    return false;
            }
            return true;
        }
        private bool solve(Button[,] btnSudoku)
        {
            for (int i = 0; i < btnSudoku.GetLength(0); i++)
            {
                for (int j = 0; j < btnSudoku.GetLength(1); j++)
                {
                    if (btnSudoku[i, j].Text == "")
                    {
                        for (char c = '1'; c <= '9'; c++)
                        {
                            if (isValid(btnSudoku, i, j, c))
                            {
                                Demo(i, j, c);
                                btnSudoku[i, j].Text = c.ToString();
                                if (solve(btnSudoku)) return true;
                                else
                                {
                                    btnSudoku[i, j].Text = "";
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
            //btnSudoku[i, j].Text = c.ToString();
        }
        private bool checkInput()
        {
            for (int i = 0; i < Cons.n; i++)
            {
                for (int j = 0; j < Cons.n; j++)
                {
                    if (btnSudoku[i, j].Text != "")
                    {
                        for (int k = i + 1; k < 9; ++k)  // kiem tra theo hang
                        {
                            if (btnSudoku[i, j].Text != "")
                            {
                                if (btnSudoku[i, j].Text == btnSudoku[k, j].Text)
                                    return false;
                            } 
                        }
                        for (int k = 0; k < i; ++k)  // kiem tra theo hang
                        {
                            if (btnSudoku[i, j].Text != "")
                            {
                                if (btnSudoku[i, j].Text == btnSudoku[k, j].Text)
                                    return false;
                            }
                        }
                        for (int k = j + 1; k < 9; ++k)  // kiem tra theo cot
                        {
                            if (btnSudoku[i, j].Text != "")
                            {
                                if (btnSudoku[i, j].Text == btnSudoku[i, k].Text)
                                    return false;
                            }
                        }
                        for (int k = 0; k < j; ++k)  // kiem tra theo cot
                        {
                            if (btnSudoku[i, j].Text != "")
                            {
                                if (btnSudoku[i, j].Text == btnSudoku[i, k].Text)
                                    return false;
                            }
                        }
                        int boxRowOffset = (i / 3) * 3;
                        int boxColOffset = (j / 3) * 3;

                        for (int k = 0; k < 3; ++k) //kiem tra trong 9 ô nhỏ
                            for (int m = 0; m < 3; ++m)
                                if ((boxRowOffset + k) != i && boxColOffset + m != j)
                                {
                                    if (btnSudoku[boxRowOffset + k, boxColOffset + m].Text != "")
                                    {
                                        if (btnSudoku[i, j].Text == btnSudoku[boxRowOffset + k, boxColOffset + m].Text)
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
                if (rbtnBT.Checked == true)
                {
                    for (int i = 0; i < Cons.n; i++)
                    {
                        for (int j = 0; j < Cons.n; j++)
                        {
                            if (btnSudoku[i, j].Text == "")
                                btnSudoku[i, j].ForeColor = Color.LightYellow;
                        }
                    }
                    if (!solve(btnSudoku)) MessageBox.Show("Can't solve");
                }
                else
                {
                    if (rbtnHeu.Checked == true)
                    {
                        for (int i = 0; i < Cons.n; i++)
                        {
                            for (int j = 0; j < Cons.n; j++)
                            {
                                if (btnSudoku[i, j].Text == "")
                                    btnSudoku[i, j].ForeColor = Color.LightYellow;
                            }
                        }
                        if (!solveHeu(btnSudoku)) MessageBox.Show("Can't solve");
                    }
                    else
                    {
                        timer.Stop();
                        timer1.Stop();
                        MessageBox.Show("Please choose an algorithm!!!");
                        return;
                    }
                }
            }
            else
            {
                MessageBox.Show("Please check the input!");
            }
            timer.Stop();
            timer1.Stop();
            MessageBox.Show("Time run: " + labeltick.Text + "s");
        }
        private void btnClick(object sender, EventArgs e)
        {
            int num;
            Button btn = (Button)sender;
            int y = (btn.Location.X) / Cons.btn_size;
            int x = (btn.Location.Y) / Cons.btn_size;
            if (btnSudoku[x, y].Text == "") num = 0;
            else num = int.Parse(btnSudoku[x, y].Text);

            num = (num + 1) % 10;
            if (num == 0)
            {
                btnSudoku[x, y].Text = "";
            }
            else
            {
                btnSudoku[x, y].Text = num.ToString();
            }
        }
        private void Clear()
        {
            for (int i = 0; i < Cons.n; i++)
            {
                for (int j = 0; j < Cons.n; j++)
                {
                    btnSudoku[i, j].Text = "";
                    btnSudoku[i, j].ForeColor = Color.Cyan;
                }
            }
        }
        private void newBtn_Click(object sender, EventArgs e)
        {
            Clear();
            labeltick.Text = "0";
            tick = 0;
        }

        private void importBtn_Click(object sender, EventArgs e)
        {
            tick = 0;
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
                                        btnSudoku[j, i].Text = "";
                                    }
                                    else
                                    {
                                        btnSudoku[j, i].Text = splitLines[i];
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
        int tick = 0;
        private void timer_Tick(object sender, EventArgs e)
        {
            tick++;
            labeltick.Text = tick.ToString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            f.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        //--------------------------Heuristic BT--------------------------
        Value[,] values = new Value[9, 9];
        Queue<Value> blankCells = new Queue<Value>();

        // find the solution
        public bool solveHeu(Button[,] btnSudoku)
        {
            updateValues();
            if (blankCells.Count() != 0)
            {
                // trả về phần tử đầu tiên và xoá nó ra khỏi PriorityQueue.
                Value vals = blankCells.Dequeue();
                int row = vals.getRow(), col = vals.getCol();

                foreach(char num in vals.getValues())
                {
                    if (isValid(btnSudoku, row, col, num))
                    {
                        Demo(row, col, num);
                        btnSudoku[row,col].Text = num.ToString();
                        if (solveHeu(btnSudoku))
                        {
                            return true;
                        }
                        // there are no solutions for this num
                        btnSudoku[row, col].Text = "";
                    }
                }
                // no solutions
                return false;
            }
            return true;
        }

        // find values for all the cells
        private void updateValues()
        {
            Value[] val2 = new Value[9*9];
            int d = 0;
            blankCells.Clear();
            for (int row = 0; row < Cons.n; row++)
                for (int col = 0; col < Cons.n; col++)
                {
                    if (btnSudoku[row, col].Text == "")
                    {
                        Value val = possibleValues(row, col);
                        values[row, col] = val;
                        val2[d++] = val;
                    }
                }
            for (int i = 0; i < d - 1; i++)
                for(int j = i+1; j < d; j++)
                    if(val2[i].compareTo(val2[j])>0)
                        Swap(ref val2[i], ref val2[j]);

            for (int i = 0; i < d; i++)
                blankCells.Enqueue(val2[i]);
        }
        public static void Swap(ref Value x, ref Value y)
        {
            Value tmp = x;
            x = y;
            y = tmp;
        }

        // return all the possible values for the current cell
        private Value possibleValues(int row, int col)
        {
            Value result = new Value(row, col);
            if (btnSudoku[row, col].Text != "")
                return result;
            for (char i = '1'; i <= '9'; i++)
                if (isValid(btnSudoku, row, col, i))
                    result.add(i);
            return result;
        }
    }
}