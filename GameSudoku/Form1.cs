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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            drawBoard();
        }
        void drawBoard()
        {
            Button oldbutton = new Button() { Width = 0, Location = new Point(0, 0) };
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Button newbtn = new Button()
                    {
                        Width = Cons.board_width,
                        Height = Cons.board_height,
                        Location = new Point(oldbutton.Location.X + oldbutton.Width, oldbutton.Location.Y),
                        BackColor = Color.PapayaWhip
                    };
                    panel.Controls.Add(newbtn);
                    oldbutton = newbtn;
                }
                oldbutton.Location = new Point(0, oldbutton.Location.Y + Cons.board_height);
                oldbutton.Width = 0;
                oldbutton.Height = 0;
            }
        }
    }
}
