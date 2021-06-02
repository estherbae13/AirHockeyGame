/* Esther Bae 
 * Mr. T
 * June 2, 2021
 * Two-player air Hockey game that uses paddles and a puck
 * The player who gets to 3 points first wins
 * */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.Threading;

namespace AirHockeyGame
{
    public partial class Form1 : Form
    {
        //global variables
        Rectangle player1 = new Rectangle(10, 120, 10, 70);
        Rectangle player2 = new Rectangle(570, 120, 10, 70);
        Rectangle puck = new Rectangle(290, 155, 10, 10);
        Rectangle p1Net = new Rectangle(-100, 105, 105, 105);
        Rectangle p2Net = new Rectangle(590, 105, 105, 105);

        int player1Score = 0;
        int player2Score = 0;

        int playerSpeed = 4;
        int puckXSpeed = -5;
        int puckYSpeed = 5;

        bool wDown = false;
        bool aDown = false;
        bool sDown = false;
        bool dDown = false;
        bool upArrowDown = false;
        bool leftArrowDown = false;
        bool downArrowDown = false;
        bool rightArrowDown = false;

        Pen whitePen = new Pen(Color.White, 6);
        SolidBrush blackBrush = new SolidBrush(Color.Black);
        SolidBrush blueBrush = new SolidBrush(Color.CornflowerBlue);
        SolidBrush purpleBrush = new SolidBrush(Color.DarkViolet);

        SoundPlayer hitWall = new SoundPlayer(Properties.Resources.hitWall);
        SoundPlayer goal = new SoundPlayer(Properties.Resources.goal);
        SoundPlayer win = new SoundPlayer(Properties.Resources.win);
        SoundPlayer hitPlayer = new SoundPlayer(Properties.Resources.hitPlayer);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = true;
                    break;
                case Keys.A:
                    aDown = true;
                    break;
                case Keys.S:
                    sDown = true;
                    break;
                case Keys.D:
                    dDown = true;
                    break;
                case Keys.Up:
                    upArrowDown = true;
                    break;
                case Keys.Left:
                    leftArrowDown = true;
                    break;
                case Keys.Down:
                    downArrowDown = true;
                    break;
                case Keys.Right:
                    rightArrowDown = true;
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = false;
                    break;
                case Keys.A:
                    aDown = false;
                    break;
                case Keys.S:
                    sDown = false;
                    break;
                case Keys.D:
                    dDown = false;
                    break;
                case Keys.Up:
                    upArrowDown = false;
                    break;
                case Keys.Left:
                    leftArrowDown = false;
                    break;
                case Keys.Down:
                    downArrowDown = false;
                    break;
                case Keys.Right:
                    rightArrowDown = false;
                    break;
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            //move puck
            puck.X += puckXSpeed;
            puck.Y += puckYSpeed;

            //move player 1
            if (wDown == true && player1.Y > 0)
            {
                player1.Y -= playerSpeed;
            }

            if (aDown == true && player1.X > 0)
            {
                player1.X -= playerSpeed;
            }

            if (sDown == true && player1.Y < this.Height - player1.Height)
            {
                player1.Y += playerSpeed;
            }

            if (dDown == true && player1.X < this.Width - player1.Width)
            {
                player1.X += playerSpeed;
            }

            //move player 2
            if (upArrowDown == true && player2.Y > 0)
            {
                player2.Y -= playerSpeed;
            }

            if (leftArrowDown == true && player2.X > 0)
            {
                player2.X -= playerSpeed;
            }

            if (downArrowDown == true && player2.Y < this.Height - player2.Height)
            {
                player2.Y += playerSpeed;
            }

            if (rightArrowDown == true && player2.X < this.Width - player2.Width)
            {
                player2.X += playerSpeed;
            }

            //puck collision with walls (exl. net)
            if (puck.Y < 0 || puck.Y > this.Height - puck.Height)
            {
                puckYSpeed *= -1;

                hitWall.Play();
            }
            if (puck.X < 0 || puck.X > this.Width - puck.Width)
            {
                puckXSpeed *= -1;

                hitWall.Play();
            }

            //puck collision with player!!!
            if (player1.IntersectsWith(puck))
            {
                puckXSpeed *= -1;
                if (puck.X <= player1.X)
                {
                    puck.X = player1.X - player1.Width;
                }
                else
                {
                    puck.X = player1.X + player1.Width;
                }

                hitPlayer.Play();
            }
            else if (player2.IntersectsWith(puck))
            {
                puckXSpeed *= -1;
                if (puck.X <= player2.X)
                {
                    puck.X = player2.X - player2.Width;
                }
                else
                {
                    puck.X = player2.X + player2.Width;
                }

                hitPlayer.Play();
            }

            //points scored
            if (puck.IntersectsWith(p1Net))
            {
                player2Score++;
                p2ScoreLabel.Text = $"{player2Score}";

                puck.X = 290;
                puck.Y = 155;

                player1.Y = 120;
                player2.Y = 120;

                goal.Play();
            }
            else if (puck.IntersectsWith(p2Net))
            {
                player1Score++;
                p1ScoreLabel.Text = $"{player1Score}";

                puck.X = 290;
                puck.Y = 155;

                player1.Y = 120;
                player2.Y = 120;

                goal.Play();
            }

            //player who wins 3 points first wins!!! (stop game)
            if (player1Score == 3)
            {
                gameTimer.Enabled = false;
                winLabel.Visible = true;
                winLabel.Text = "Player 1 Wins!!";

                win.Play();
            }
            else if (player2Score == 3)
            {
                gameTimer.Enabled = false;
                winLabel.Visible = true;
                winLabel.Text = "Player 2 Wins!!";

                win.Play();
            }

            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //show all graphics (players + puck + lines)
            e.Graphics.DrawLine(whitePen, 295, 500, 295, 0);
            e.Graphics.DrawEllipse(whitePen, -55, 105, 105, 105);
            e.Graphics.DrawEllipse(whitePen, 550, 105, 105, 105);
            e.Graphics.FillRectangle(blackBrush, puck);
            e.Graphics.FillRectangle(blueBrush, player1.X, player1.Y, player1.Width, player1.Height);
            e.Graphics.FillRectangle(purpleBrush, player2.X, player2.Y, player2.Width, player2.Height);
        }
    }
}