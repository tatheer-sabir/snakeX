using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace snake_game_0._1
{

    public partial class snakegame : Form
    {
        public string startsoundPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "game-start.wav"
        );
        public string eatsoundPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "Food-Eating.wav"
        );
        public string gameoversoundPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "game-over.wav"
        );




        private Color darkGreen = Color.FromArgb(27, 94, 32);      // #1B5E20
        private Color green = Color.FromArgb(56, 142, 60);         // #388E3C
        private Color lightGreen = Color.FromArgb(67, 160, 71);    // #43A047
        private Color paleGreen = Color.FromArgb(102, 187, 106);   // #66BB6A
        private Color veryLightGreen = Color.FromArgb(165, 214, 167); // #A5D6A7
        private Color mintGreen = Color.FromArgb(200, 230, 201);   // #C8E6C9



        public int highscore = 0;
        string highscorePath = "highscore.txt";
        public Color panelbackcolor = Color.White;
        public Brush head = Brushes.DarkGreen;
        public Brush body = Brushes.LightGreen;
        public Brush foodcolor = Brushes.Green;
        public int difficultychoice;
        public int snakespeed;
        public int score = 0;
        public int moving = 0;
        public List<Circle> snake = new List<Circle>();
        public int maxwidth;
        public int maxheight;
        public List<Circle> food = new List<Circle>();
        public Circle singlefood = new Circle();

        Random rand = new Random();

        bool goleft, goright, goup, godown;



        public void passthroughborder()
        {
            if (snake[0].x < 1)
            {
                snake[0].x = maxwidth;
            }
            if (snake[0].x > maxwidth)
            {
                snake[0].x = 1;
            }
            if (snake[0].y < 1)
            {
                snake[0].y = maxheight;
            }
            if (snake[0].y > maxheight)
            {
                snake[0].y = 1;
            }
        }
        public void HardBorderCheck()
        {
            // Dies if hitting border
            if (snake[0].x == 0 || snake[0].x == maxwidth + 1 ||
                snake[0].y == 0 || snake[0].y == maxheight + 1)
            {
                Thread.Sleep(1000);
                gameover();
            }
        }
        private void CenterStartContent()
        {
            contentPanel.Left = (welcomepanel.Width - contentPanel.Width) / 2;
            contentPanel.Top = (welcomepanel.Height - contentPanel.Height) / 2;
        }
        private void CenterInstructionContent()
        {
            instructioncontentpanel.Left = (instructionpnl.Width - instructioncontentpanel.Width) / 2;
            instructioncontentpanel.Top = (instructionpnl.Height - instructioncontentpanel.Height) / 2;
        }
        private void CentersettingContent()
        {
            settingcontentpnl.Left = (settingpnl.Width - settingcontentpnl.Width) / 2;
            settingcontentpnl.Top = (settingpnl.Height - settingcontentpnl.Height) / 2;
        }
        private void CenterEndContent()
        {
            gameovercontentpanel.Left = (gameoverpnl.Width - gameovercontentpanel.Width) / 2;
            gameovercontentpanel.Top = (gameoverpnl.Height - gameovercontentpanel.Height) / 2;
        }








        public snakegame()
        {
            InitializeComponent();
            new Setting();
            this.KeyPreview = true;

            gamepnl.DoubleBuffered(true);

        }

        private void snakegame_Load(object sender, EventArgs e)
        {
            // Apply green theme to form and panels
            this.BackColor = mintGreen;

            // Apply green to panels
            welcomepanel.BackColor = mintGreen;
            gameoverpnl.BackColor = mintGreen;
            settingpnl.BackColor = mintGreen;
            instructionpnl.BackColor = mintGreen;

            // Apply green to content panels
            contentPanel.BackColor = Color.White;
            instructioncontentpanel.BackColor = Color.White;
            settingcontentpnl.BackColor = Color.White;
            gameovercontentpanel.BackColor = Color.White;

            // Apply green to menu strips
            menuStrip1.BackColor = darkGreen;
            menuStrip1.ForeColor = Color.White;
            menuStrip2.BackColor = darkGreen;
            menuStrip2.ForeColor = Color.White;

            // Apply green to buttons
            startbtn.BackColor = green;
            startbtn.ForeColor = Color.White;
            startbtn.FlatStyle = FlatStyle.Flat;
            startbtn.FlatAppearance.BorderColor = darkGreen;
            startbtn.FlatAppearance.BorderSize = 2;
            startbtn.FlatAppearance.MouseOverBackColor = lightGreen;
            startbtn.FlatAppearance.MouseDownBackColor = darkGreen;

            backbtn.BackColor = green;
            backbtn.ForeColor = Color.White;
            backbtn.FlatStyle = FlatStyle.Flat;
            backbtn.FlatAppearance.BorderColor = darkGreen;
            backbtn.FlatAppearance.BorderSize = 2;
            backbtn.FlatAppearance.MouseOverBackColor = lightGreen;
            backbtn.FlatAppearance.MouseDownBackColor = darkGreen;

            insbackbtn.BackColor = green;
            insbackbtn.ForeColor = Color.White;
            insbackbtn.FlatStyle = FlatStyle.Flat;
            insbackbtn.FlatAppearance.BorderColor = darkGreen;
            insbackbtn.FlatAppearance.BorderSize = 2;
            insbackbtn.FlatAppearance.MouseOverBackColor = lightGreen;
            insbackbtn.FlatAppearance.MouseDownBackColor = darkGreen;

            newgamebtn.BackColor = green;
            newgamebtn.ForeColor = Color.White;
            newgamebtn.FlatStyle = FlatStyle.Flat;
            newgamebtn.FlatAppearance.BorderColor = darkGreen;
            newgamebtn.FlatAppearance.BorderSize = 2;
            newgamebtn.FlatAppearance.MouseOverBackColor = lightGreen;
            newgamebtn.FlatAppearance.MouseDownBackColor = darkGreen;

            exitgamebtn.BackColor = green;
            exitgamebtn.ForeColor = Color.White;
            exitgamebtn.FlatStyle = FlatStyle.Flat;
            exitgamebtn.FlatAppearance.BorderColor = darkGreen;
            exitgamebtn.FlatAppearance.BorderSize = 2;
            exitgamebtn.FlatAppearance.MouseOverBackColor = lightGreen;
            exitgamebtn.FlatAppearance.MouseDownBackColor = darkGreen;

            // Apply green to labels
            gameoverlabel.ForeColor = darkGreen;
            scorelabel.ForeColor = darkGreen;
            highscoretxt.ForeColor = darkGreen;
            label2.ForeColor = darkGreen;

            // Apply green to radio buttons and checkboxes
            easychoice.ForeColor = darkGreen;
            mediumchoice.ForeColor = darkGreen;
            hardchoice.ForeColor = darkGreen;
            circlesnake.ForeColor = darkGreen;
            squaresnake.ForeColor = darkGreen;
            circlefood.ForeColor = darkGreen;
            squarefood.ForeColor = darkGreen;
            singlfood.ForeColor = darkGreen;
            mulfood.ForeColor = darkGreen;



            restart();
            CenterEndContent();
            CenterStartContent();
            CentersettingContent();
            CenterInstructionContent();
        }

        private void snakegame_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goleft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goright = false;
            }
            if (e.KeyCode == Keys.Up)
            {
                goup = false;
            }
            if (e.KeyCode == Keys.Down)
            {
                godown = false;
            }


        }

        private void snakegame_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left && Setting.direction != "right")
            {
                goleft = true;
                moving = 0;

            }
            if (e.KeyCode == Keys.Right && Setting.direction != "left")
            {
                goright = true;
                moving = 0;


            }
            if (e.KeyCode == Keys.Up && Setting.direction != "down")
            {
                goup = true;
                moving = 0;


            }
            if (e.KeyCode == Keys.Down && Setting.direction != "up")
            {
                godown = true;
                moving = 0;


            }
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            if (e.KeyCode == Keys.Space)
            {
                if (moving == 0)
                {
                    moving = 1;
                }
                else
                {
                    moving = 0;
                }
            }

        }

        private void startbtn_Click(object sender, EventArgs e)
        {
        }

        private void gamepnl_Paint(object sender, PaintEventArgs e)
        {
            int borderThickness = 16; // adjust thickness
            Rectangle borderRect = new Rectangle(0, 0, gamepnl.Width, gamepnl.Height + 5);

            using (LinearGradientBrush borderColor = new LinearGradientBrush(
                borderRect,
                Color.Green,
                lightGreen,
                45f))
            using (Pen borderPen = new Pen(borderColor, borderThickness))
            {
                borderPen.Alignment = PenAlignment.Inset;

                e.Graphics.DrawRectangle(
                    borderPen,
                    0,
                    0,
                    gamepnl.Width,
                    gamepnl.Height + 5
                );
            }




            gamepnl.BackColor = panelbackcolor;

            Graphics canvas = e.Graphics;

            Brush snakecolor;

            for (int i = 0; i < snake.Count; i++)
            {
                if (i == 0)
                {
                    snakecolor = head;
                }
                else
                {
                    snakecolor = body;
                }

                if (circlesnake.Checked == true)
                {
                    canvas.FillEllipse(snakecolor, new Rectangle(
                        snake[i].x * Setting.width,
                        snake[i].y * Setting.height,
                        Setting.width,
                        Setting.height
                    ));
                }

                if (squaresnake.Checked == true)
                {
                    e.Graphics.FillRectangle(
                       snakecolor,
                       snake[i].x * Setting.width,
                       snake[i].y * Setting.height,
                       Setting.width,
                       Setting.height
                       );
                }
            }


            for (int i = 0; i < food.Count; i++)
            {

                // Get the base color from your food brush
                Color baseFoodColor = ((SolidBrush)foodcolor).Color;

                // Calculate 60% lighter version
                Color lightshadeoffoodcolor = Color.FromArgb(
                    255, // full alpha
                    baseFoodColor.R + (int)((255 - baseFoodColor.R) * 0.4),
                    baseFoodColor.G + (int)((255 - baseFoodColor.G) * 0.4),
                    baseFoodColor.B + (int)((255 - baseFoodColor.B) * 0.4)
                );

                // Draw food with green glow effect
                Rectangle foodRect = new Rectangle(
                        food[i].x * Setting.width,
                        food[i].y * Setting.height,
                        Setting.width,
                        Setting.height
                    );

                // Outer glow
                Rectangle glowRect = new Rectangle(
                    foodRect.X - 3,
                    foodRect.Y - 3,
                    foodRect.Width + 6,
                    foodRect.Height + 6
                );

                if (circlefood.Checked == true)
                {
                    // Draw glow
                    canvas.FillEllipse(new SolidBrush(Color.FromArgb(100, lightshadeoffoodcolor)), glowRect);
                    // Draw food
                    canvas.FillEllipse(foodcolor, foodRect);
                    // Draw highlight
                    Rectangle highlightRect = new Rectangle(
                        foodRect.X + foodRect.Width / 4,
                        foodRect.Y + foodRect.Height / 4,
                        foodRect.Width / 2,
                        foodRect.Height / 2
                    );
                    canvas.FillEllipse(new SolidBrush(lightshadeoffoodcolor), highlightRect);
                }
                else if (squarefood.Checked == true)
                {
                    // Draw glow
                    canvas.FillRectangle(new SolidBrush(Color.FromArgb(100, lightshadeoffoodcolor)), glowRect);
                    // Draw food
                    canvas.FillRectangle(foodcolor, foodRect);
                    // Draw highlight
                    Rectangle highlightRect = new Rectangle(
                        foodRect.X + foodRect.Width / 4,
                        foodRect.Y + foodRect.Height / 4,
                        foodRect.Width / 2,
                        foodRect.Height / 2
                    );
                    canvas.FillRectangle(new SolidBrush(lightshadeoffoodcolor), highlightRect);
                }
            }


        }

        private void easytimer_Tick(object sender, EventArgs e)
        {
            maxwidth = (gamepnl.Width / Setting.width);
            maxheight = (gamepnl.Height / Setting.height);

            maxheight = maxheight - 1;
            maxwidth = maxwidth - 2;




            if (goleft)
            {
                Setting.direction = "left";
            }
            if (goright)
            {
                Setting.direction = "right";
            }
            if (goup)
            {
                Setting.direction = "up";
            }
            if (godown)
            {
                Setting.direction = "down";
            }

            if (moving == 0)
            {
                for (int i = snake.Count - 1; i >= 0; i--)
                {
                    if (i == 0)
                    {
                        if (Setting.direction == "left")
                        {
                            snake[i].x--;
                        }
                        else if (Setting.direction == "right")
                        {
                            snake[i].x++;
                        }
                        else if (Setting.direction == "up")
                        {
                            snake[i].y--;
                        }
                        else if (Setting.direction == "down")
                        {
                            snake[i].y++;
                        }
                        else
                        {
                            MessageBox.Show("Errror");
                        }


                        if (difficultychoice == 1)
                        {
                            passthroughborder();
                        }
                        else
                        {
                            HardBorderCheck();
                        }


                        for (int k = 0; k < food.Count; k++)
                        {
                            if (snake[i].x == food[k].x && snake[i].y == food[k].y)
                            {
                                eatfood(k);
                            }
                        }

                        for (int j = 1; j < snake.Count - 1; j++)
                        {
                            if (snake[i].x == snake[j].x && snake[i].y == snake[j].y)
                            {
                                Thread.Sleep(1000);
                                gameover();
                            }
                        }
                    }
                    else
                    {
                        snake[i].x = snake[i - 1].x;
                        snake[i].y = snake[i - 1].y;
                    }
                }
            }
            gamepnl.Invalidate();
        }

        private void redToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelbackcolor = Color.Red;
        }

        private void blueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelbackcolor = Color.Blue;
        }

        private void greenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelbackcolor = Color.Green;
        }

        private void reToolStripMenuItem_Click(object sender, EventArgs e)
        {
            head = Brushes.Red;
        }

        private void purpleToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            head = Brushes.Purple;
        }

        private void orangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            head = Brushes.Orange;
        }

        private void redToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            body = Brushes.Red;
        }

        private void purpleToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            body = Brushes.Purple;
        }

        private void orangeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            body = Brushes.Orange;
        }

        private void redToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            foodcolor = Brushes.Red;
        }

        private void orangeToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            foodcolor = Brushes.Orange;
        }

        private void purpleToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            foodcolor = Brushes.Purple;
        }

        private void blueToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            foodcolor = Brushes.Blue;
        }

        private void yellowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foodcolor = Brushes.Yellow;
        }





        private void selectdiff_Enter(object sender, EventArgs e)
        {

        }

        private void welcomepanel_Paint(object sender, PaintEventArgs e)
        {
            menuStrip1.Visible = false;
            gamepnl.Visible = false;
            gameoverpnl.Visible = false;
            welcomepanel.Dock = DockStyle.Fill;
            if (easychoice.Checked || mediumchoice.Checked || hardchoice.Checked)
            {
                startbtn.Enabled = true;
            }
            else
            {
                startbtn.Enabled = false;
            }
        }

        private void startbtn_Click_2(object sender, EventArgs e)
        {
            SoundPlayer startgame = new SoundPlayer(startsoundPath);
            startgame.Play();


            restart();
            if (easychoice.Checked)
            {
                difficultychoice = 1;
            }
            else if (mediumchoice.Checked)
            {
                difficultychoice = 2;
            }
            else if (hardchoice.Checked)
            {
                difficultychoice = 3;
            }


            if (difficultychoice == 1)
            {
                easytimer.Interval = 100;
            }
            else if (difficultychoice == 2)
            {
                easytimer.Interval = 60;
            }
            else if (difficultychoice == 3)
            {
                easytimer.Interval = 40;
            }
            else
            {
                Console.WriteLine("Invalid input");
            }


            welcomepanel.Visible = false;
            gamepnl.Visible = true;
            menuStrip1.Visible = true;
            gameoverpnl.Visible = false;
            gamepnl.Dock = DockStyle.Fill;
            menuStrip2.Visible = false;

        }

        private void snakegame_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Space)
            {
                e.IsInputKey = true;
            }
        }

        private void neToolStripMenuItem_Click(object sender, EventArgs e)
        {
            menuStrip2.Visible = true;
            welcomepanel.Visible = true;
            gamepnl.Visible = false;
            gameoverpnl.Visible = false;
            menuStrip1.Visible = false;
            restart();

        }

        private void pauseGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            easytimer.Stop();
        }

        private void resumeGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            easytimer.Start();
        }

        private void newgamebtn_Click(object sender, EventArgs e)
        {
            restart();
            gameoverpnl.Visible = false;
            welcomepanel.Visible = true;
            menuStrip2.Visible = true;
        }

        private void gameoverpnl_Paint(object sender, PaintEventArgs e)
        {

        }

        private void exitgamebtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void scorelabel_Click(object sender, EventArgs e)
        {

        }

        private void gameoverlabel_Click(object sender, EventArgs e)
        {

        }

        private void gamepnl_Resize(object sender, EventArgs e)
        {
        }

        private void welcomepanel_Resize(object sender, EventArgs e)
        {
            CenterStartContent();
        }

        private void selectdiff_Enter_1(object sender, EventArgs e)
        {

        }

        private void gameoverpnl_Resize(object sender, EventArgs e)
        {
            CenterEndContent();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void openSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            settingpnl.Dock = DockStyle.Fill;
            settingpnl.Visible = true;
            gamepnl.Visible = false;
            gameoverpnl.Visible = false;
            welcomepanel.Visible = false;
        }

        private void settingpnl_Resize(object sender, EventArgs e)
        {
            CentersettingContent();
        }

        private void backbtn_Click(object sender, EventArgs e)
        {
            menuStrip2.Visible = true;
            settingpnl.Visible = false;
            gamepnl.Visible = false;
            gameoverpnl.Visible = false;
            welcomepanel.Visible = true;
            instructionpnl.Visible = false;
        }

        private void openSettingsToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            settingpnl.Dock = DockStyle.Fill;
            menuStrip2.Visible = false;
            settingpnl.Visible = true;
            gamepnl.Visible = false;
            gameoverpnl.Visible = false;
            welcomepanel.Visible = false;
            instructionpnl.Visible = false;
        }

        private void settingpnl_Paint(object sender, PaintEventArgs e)
        {

        }

        private void contentPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void settingcontentpnl_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {


        }

        private void openInstructionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            instructionpnl.Dock = DockStyle.Fill;
            menuStrip2.Visible = false;
            settingpnl.Visible = false;
            gamepnl.Visible = false;
            gameoverpnl.Visible = false;
            welcomepanel.Visible = false;
            instructionpnl.Visible = true;


            label2.Text =
                "HOW TO PLAY\n\n" +
                "• Press the Arrow Keys to move the snake.\n" +
                "• Left Arrow  = Move Left\n" +
                "• Right Arrow = Move Right\n" +
                "• Up Arrow    = Move Up\n" +
                "• Down Arrow  = Move Down\n\n" +
                "• Press SPACE to Pause or Resume the game.\n" +
                "• Press ESC to Exit the game instantly.\n\n" +
                "GAME RULES\n" +
                "• Eat food to grow your snake.\n" +
                "• Avoid hitting your own body.\n" +
                "• In Easy Mode, you pass through borders.\n" +
                "• In Medium/Hard Mode, hitting the border ends the game.\n\n" +
                "             GOOD LUCK! \n" +
                "Score high and beat the highscore!";
        }

        private void insbackbtn_Click(object sender, EventArgs e)
        {
            menuStrip2.Visible = true;
            settingpnl.Visible = false;
            gamepnl.Visible = false;
            gameoverpnl.Visible = false;
            welcomepanel.Visible = true;
            instructionpnl.Visible = false;
        }

        private void instructionpnl_Resize(object sender, EventArgs e)
        {
            CenterInstructionContent();
        }

        private void greenToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            foodcolor = Brushes.Green;
        }

        private void greenToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            head = Brushes.DarkGreen;
        }

        private void lightGreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            body = Brushes.LightGreen;
        }

        public void restart()
        {

            body = Brushes.LightGreen;
            head = Brushes.DarkGreen;
            foodcolor = Brushes.Green;
            gamepnl.BackColor = Color.White;



            settingpnl.Visible = false;
            instructionpnl.Visible = false;



            gamepnl.Dock = DockStyle.Fill;
            score = 0;
            food.Clear();

            scorelabel.Text = score.ToString();

            if (File.Exists(highscorePath))
            {
                int.TryParse(File.ReadAllText(highscorePath), out highscore);
            }
            else
            {
                highscore = 0;
            }

            scoretxt.Text = "0";












            maxwidth = (gamepnl.Width / Setting.width);
            maxheight = (gamepnl.Height / Setting.height);

            maxheight = maxheight - 1;
            maxwidth = maxwidth - 1;

            snake.Clear();


            Circle snakehead = new Circle { x = 5, y = 10 };
            snake.Add(snakehead);

            Circle body1 = new Circle { x = 6, y = 10 };
            snake.Add(body1);

            Circle body2 = new Circle { x = 7, y = 10 };
            snake.Add(body2);


            if (singlfood.Checked == true)
            {
                for (int i = 1; i < 2; i++)
                {
                    Circle fooditem = new Circle
                    {
                        x = rand.Next(2, maxwidth - 2),
                        y = rand.Next(2, maxheight - 2)
                    };
                    food.Add(fooditem);
                }
            }

            if (singlfood.Checked == false)
            {
                for (int i = 1; i < 7; i++)
                {
                    Circle fooditem = new Circle
                    {
                        x = rand.Next(2, maxwidth - 2),
                        y = rand.Next(2, maxheight - 2)
                    };
                    food.Add(fooditem);
                }
            }

            easytimer.Start();
            moving = 1;

        }

        public void gameover()
        {

            SoundPlayer gameoversound = new SoundPlayer(gameoversoundPath);
            gameoversound.Play();


            easytimer.Stop();
            menuStrip1.Visible = false;
            gameoverpnl.Visible = false;
            welcomepanel.Visible = false;
            gameoverpnl.Visible = true;
            gameoverpnl.Dock = DockStyle.Fill;
            scorelabel.Text = score.ToString();

            if (score > highscore)
            {
                highscore = score;
                File.WriteAllText(highscorePath, highscore.ToString());
            }

            highscoretxt.Text = highscore.ToString();
        }

        public void eatfood(int k)
        {

            SoundPlayer eatsound = new SoundPlayer(eatsoundPath);
            eatsound.Play();


            Circle newcircle = new Circle
            {
                x = snake[snake.Count - 1].x,
                y = snake[snake.Count - 1].y
            };

            snake.Add(newcircle);

            food[k] = new Circle
            {
                x = rand.Next(2, maxwidth - 2),
                y = rand.Next(2, maxheight - 2)
            };

            score += 10;

            scoretxt.Text = score.ToString();


        }
    }
}
