using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace practice6_1
{
    public partial class Form1 : Form
    {
        bool start = false;
        bool but_st = false;
        int boardnum = 25;
        public Form1()
        {
            InitializeComponent();
            reset();
        }

        PictureBox ball = new PictureBox();

        int mouseX;

        private void BoardMove(object sender, MouseEventArgs e)
        {
            mouseX = e.X;    
        }

        private void BoardMoving(object sender, MouseEventArgs e)
        {
            if (but_st)
            {
                if (e.Button == MouseButtons.Left)
                {
                    int x = board.Left + (e.X - mouseX);
                    if (x < 0) { x = 0; }
                    if (x > this.ClientSize.Width - board.Width)
                    {
                        x = this.ClientSize.Width - board.Width;
                    }
                    board.Left = x;
                }
            }
        }

        Label[] bricks = new Label[25];
        private void GenerateBricks() {
            int index = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    bricks[index] = new Label();
                    bricks[index].Width = 100;
                    bricks[index].Height = 35;
                    bricks[index].Location = new Point(i*100, j*35);
                    bricks[index].BackColor = Color.DarkOrange;
                    bricks[index].BorderStyle = BorderStyle.Fixed3D;
                    this.Controls.Add(bricks[index]);
                    index++;
                }
            }
        }

        private void GenerateBall() {
            ball = new PictureBox();
            ball.Width = 32;
            ball.Height = 32;
            ball.Image = new Bitmap(Properties.Resources.pp,ball.Size);
            ball.Location = new Point(180, 432);
            this.Controls.Add(ball);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (board.Left != 180)
            {
                start = true;
            }
            if (start)
            {
                move();
                AutoBoardMove();
                CollidesAutoBoard();
                collidesBricks();
                CollidesBoard();
            }
        }

        private void AutoBoardMove() {
            if (ball.Top>=autoboard.Top)
            {
                if (Vx > 0 && autoboard.Right < 516)
                    autoboard.Left += 5;
                else if (Vx < 0 && autoboard.Left >= 0)
                    autoboard.Left -= 5;
            }
            else {
                if (Vx > 0 && autoboard.Left >= 0)
                    autoboard.Left -= 5;
                else if (Vx < 0 && autoboard.Right < 516)
                    autoboard.Left += 5;
            }
        }

        private void CollidesAutoBoard() {
            if (ball.Top <= autoboard.Bottom && ball.Top > autoboard.Top && ball.Left +16> autoboard.Left && ball.Left+16 < autoboard.Right)
            {
                Vy = -Vy;
            }
            else if (ball.Bottom>= autoboard.Top && ball.Bottom < autoboard.Bottom && ball.Left+16 > autoboard.Left && ball.Left +16 < autoboard.Right)
            {
                Vy = -Vy;
            }
            else if (ball.Right >= autoboard.Left && ball.Left+16<autoboard.Left&&ball.Right < autoboard.Right && ball.Top + 16 > autoboard.Top && ball.Top + 16 < autoboard.Bottom) {
                Vx = -Vx;
            }
            else if (ball.Left <= autoboard.Right && ball.Right-16>autoboard.Right&&ball.Left > autoboard.Left && ball.Top + 16 > autoboard.Top && ball.Top + 16 < autoboard.Bottom)
            {
                Vx = -Vx;
            }
        }

        private void CollidesBoard() {
            //if (ball.Bottom >= board.Top && ball.Left + 16 >= board.Left && ball.Right - 16 <= board.Right) {
            if (ball.Bottom >= board.Top && ball.Left + 16 >= board.Left && ball.Right - 16 <= board.Right)
            {
                if (ball.Left + 16 < board.Left + board.Width/2)
                {
                    Vy = -Vy;
                    Vx = -Math.Abs(Vx);
                }
                else if (ball.Left + 16 >= board.Width / 2) {
                    Vy = -Vy;
                    Vx = Math.Abs(Vx);
                }
            }
        }

        private void collidesBricks() {
            for (int i = 0; i < 25; i++) {
                if (ball.Top <= bricks[i].Bottom && ball.Top > bricks[i].Top && ball.Right -16< bricks[i].Right && ball.Left+16 > bricks[i].Left && !bricks[i].IsDisposed)
                {
                    bricks[i].Dispose();
                    boardnum--;
                    Vy = -Vy;
                    break;
                }
                else if (ball.Bottom < bricks[i].Bottom && ball.Bottom >= bricks[i].Top && ball.Right -16< bricks[i].Right && ball.Left  +16> bricks[i].Left && !bricks[i].IsDisposed)
                {
                    bricks[i].Dispose();
                    boardnum--;
                    Vy = -Vy;
                    break;
                }
                else if (ball.Right >= bricks[i].Left && ball.Right < bricks[i].Right && ball.Top + 16 > bricks[i].Top && ball.Bottom - 16 < bricks[i].Bottom && !bricks[i].IsDisposed)
                {
                    bricks[i].Dispose();
                    boardnum--;
                    Vx = -Vx;
                    break;
                }
                else if (ball.Left <= bricks[i].Right && ball.Left > bricks[i].Left && ball.Top + 16 > bricks[i].Top && ball.Bottom - 16 < bricks[i].Bottom && !bricks[i].IsDisposed) {
                    bricks[i].Dispose();
                    boardnum--;
                    Vx = -Vx;
                    break;
                }
                if (boardnum == 0)
                {
                    timer1.Stop();
                    start = false;
                    boardnum = 25;
                    MessageBox.Show("You win the Game!");
                    MessageBox.Show("Press to Restart!");
                    ball.Dispose();
                    for (int j = 0; j < 25; j++)
                    {
                        if (!bricks[j].IsDisposed)
                            bricks[j].Dispose();
                    }
                    reset();
                }
            }
        }

        int Vx = 0, Vy = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                Vx = 8;
                Vy = 8;
                board.Width = 200;
                autoboard.Width = 70;
            }
            if (radioButton2.Checked)
            {
                Vx =10;
                Vy = 10;
            }
            if (radioButton3.Checked)
            {
                Vx = 14;
                Vy = 14;
            }
            //start = true;
            but_st = true;
            panel1.Hide();
            radioButton2.Hide();
            radioButton1.Hide();
            radioButton3.Hide();
            button1.Hide();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void move() {
            ball.Top += Vy;
            ball.Left += Vx;
            if (ball.Left > ClientSize.Width-32)
            {
                Vx = -Vx;
                ball.Left = ClientSize.Width-32;
            }
            else if (ball.Top > ClientSize.Height-32)
            {
                timer1.Stop();
                MessageBox.Show("Game Over.");
                MessageBox.Show("Press to Restart.");
                ball.Dispose();
                for (int i = 0; i < 25; i++) {
                    if(!bricks[i].IsDisposed)
                        bricks[i].Dispose();
                }
                reset();
            }
            else if (ball.Left <= 0)
            {
                Vx = -Vx;
                ball.Left = 0;
            }
            else if (ball.Top<=0) {
                Vy = -Vy;
                ball.Top =0;
            }
        }

        private void reset() {
            start = false;
            but_st = false;
            GenerateBricks();
            GenerateBall();
            Width = 516;
            Height = 550;
            panel1.Width = 150;
            panel1.Height = 200;
            panel1.Top = 100;
            panel1.Left = 176;
            board.Width = 150;
            board.Height = 25;
            autoboard.Width = 100;
            autoboard.Height = 30;
            autoboard.Top = 200;
            autoboard.Left = 176;
            board.Top = 465;
            board.Left = 180;
            boardnum = 25;
            button1.Show();
            radioButton1.Show();
            radioButton2.Show();
            radioButton3.Show();
            panel1.Show();
            timer1.Interval = 30;
            timer1.Start();
        }
    }
}
