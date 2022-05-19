using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            for(int i = 0; i < Cons.n; i++)
            {
                for (int j = 0; j < Cons.n; j++)
                {
                    btnBacktracking[i, j] = new Button();
                    btnBacktracking[i, j].Size = new Size(Cons.btn_size, Cons.btn_size);
                    btnBacktracking[i, j].Text = "";
                    btnBacktracking[i, j].Location = new Point(j * Cons.btn_size, i * Cons.btn_size);
                    if(((i < 3 || i > 5) && (j > 2 && j < 6)) || ((j < 3 || j > 5) && (i > 2 && i < 6)))
                    {
                        btnBacktracking[i, j].BackColor = Color.RoyalBlue;
                    }
                    panel.Controls.Add(btnBacktracking[i, j]);
                }
            }
        }
    }
}
