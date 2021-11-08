using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.Media;
using System.IO;

namespace WhatWhereWhen
{
    public partial class Form1 : Form
    {
        private int i = 0;
        private int rotationSpeed = 0;
        private int maxSpeed = 20;
        private float owlSizeKoeff = 0.17f;
        private SoundPlayer rotationMusic = new SoundPlayer("music/rotation.wav");
        private SoundPlayer gongSound= new SoundPlayer("music/gong.wav");
        private SoundPlayer beginEndSound = new SoundPlayer("music/beginEnd.wav");
        private SoundPlayer tenSecondsSound = new SoundPlayer("music/tenSeconds.wav");
        private SoundPlayer mainSound = new SoundPlayer("music/main.wav");
        private SoundPlayer roundEndSound = new SoundPlayer("music/roundEnd.wav");
        private SoundPlayer questionAttentionSound = new SoundPlayer("music/questionAttention.wav");
        private Image table = Image.FromFile("images/tableResult.png");
        private string[] cellColor = new string[12];
        private bool[] cellPlay = new bool[12];
        private bool arrowStopped = false;
        private int timeQ;
        private string sectorOwner;
        private string earlyAnswerColor = " ";
        private int questionNumber = 0;
        private int roundNumber = 0;
        private int rCorrectAnswers = 0;
        private int bCorrectAnswers = 0;
        private int yCorrectAnswers = 0;
        private int rRoundWin = 0;
        private int bRoundWin = 0;
        private int yRoundWin = 0;
        private int pictureWidthHeight;
        private int roundQuestions;
        private string[] dirs;
        private int arrayQuestionLen;

        public Form1()
        {
            for (int k = 0; k < 12; k++)
            {
                cellColor[k] = "w";
                cellPlay[k] = false;
            }
            InitializeComponent();
            dirs = Directory.GetDirectories(@"questions\", "*", SearchOption.TopDirectoryOnly);
            arrayQuestionLen = dirs.Length;
            Random rnd = new Random();
            int mixNumber = rnd.Next(arrayQuestionLen, arrayQuestionLen*2);
            for (int k = 0; k<mixNumber; k++)
            {
                int first = rnd.Next(arrayQuestionLen);
                int twice = rnd.Next(arrayQuestionLen);
                string t = dirs[first];
                dirs[first] = dirs[twice];
                dirs[twice] = t;
            }
            this.WindowState = FormWindowState.Maximized;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            mainSound.Play();
        }

        private void drawTable()
        {
            Console.WriteLine("DrawTable");
            table.Dispose();
            Bitmap res = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(res);
            Console.WriteLine(cellColor[1]);
            g.DrawImage(Image.FromFile("images/tableBackground.png"), 0, 0, pictureBox1.Width, pictureBox1.Height);
            for (int k = 0; k < 12; k++)
            {
                g.DrawImage(Image.FromFile("images/"+cellColor[k]+"Sector/"+ cellColor[k] + k + ".png"), (pictureBox1.Width - pictureWidthHeight)/2, (pictureBox1.Height - pictureWidthHeight) / 2, pictureWidthHeight, pictureWidthHeight);
                if (cellPlay[k])
                {
                    g.DrawImage(Image.FromFile("images/cells/cell" + k + ".png"), (pictureBox1.Width - pictureWidthHeight) / 2, (pictureBox1.Height - pictureWidthHeight) / 2, pictureWidthHeight, pictureWidthHeight);
                }
            }
            g.DrawImage(Image.FromFile("images/table.png"), (pictureBox1.Width - pictureWidthHeight) / 2, (pictureBox1.Height - pictureWidthHeight) / 2, pictureWidthHeight, pictureWidthHeight);
            res.Save("images/tableResult.png");
            table = Image.FromFile("images/tableResult.png");
            pictureBox1.BackgroundImage = table;
        }


    
        /* protected override void OnPaint(PaintEventArgs e)
         {
             e.Graphics.DrawImage(table, (this.Width - table_width_height) / 2, (this.Height - table_width_height) / 2, table_width_height, table_width_height);
         }*/

        private void changeSize()
        {
            bool[] visibility =
            {
                panelQuestion.Visible, panelAnswer.Visible, Yellow.Visible, Blue.Visible, Red.Visible, tableLayoutPanel3.Visible, buttonEarlyAnswer.Visible,
                showAnswer.Visible, showQuestionButton.Visible, panelEndRound.Visible, panelNoQuestions.Visible
            };
            panelQuestion.Visible = true;
            panelAnswer.Visible = true;
            Yellow.Visible = true;
            Blue.Visible = true;
            Red.Visible = true;
            tableLayoutPanel3.Visible = true;
            buttonEarlyAnswer.Visible = true;
            showAnswer.Visible = true;
            showQuestionButton.Visible = true;
            panelEndRound.Visible = true;
            panelNoQuestions.Visible = true;
            //this.Invalidate();
            //pictureBox1.Height = (int)(this.Height * 0.82);
            //pictureBox1.Width = (int)(this.Height * 0.82);
            Console.WriteLine(pictureBox1.Height);
            //pictureBox1.Location = new Point((this.Width - pictureBox1.Width) / 2, (this.Height - pictureBox1.Height) / 2);
            if (pictureBox1.Width > pictureBox1.Height)
            {
                pictureWidthHeight = pictureBox1.Height;
            }
            else
            {
                pictureWidthHeight = pictureBox1.Width;
            }
            pictureBox2.Height = (int)(pictureWidthHeight * owlSizeKoeff);
            pictureBox2.Width = (int)(pictureWidthHeight * owlSizeKoeff);
            pictureBox2.Location = new Point((pictureBox1.Width - pictureBox2.Width) / 2, (pictureBox1.Height - pictureBox2.Height) / 2);
            try
            {
                startGame.Font = new Font("Century Gothic", (int)(startGame.Width * 0.1));
                textBoxStart.Font = new Font("Century Gothic", (int)(textBoxStart.Width * 0.08));
                label4.Font = new Font("Century Gothic", (int)(label4.Width * 0.06));
                label2.Font = new Font("Century Gothic", (int)(label2.Width * 0.075));
                label3.Font = new Font("Century Gothic", (int)(label3.Width * 0.075));
                labelRotation.Font = new Font("Century Gothic", (int)(labelRotation.Width * 0.05));
                rCorrect.Font = new Font("Century Gothic", (int)(rCorrect.Width * 0.1));
                bCorrect.Font = new Font("Century Gothic", (int)(bCorrect.Width * 0.1));
                yCorrect.Font = new Font("Century Gothic", (int)(yCorrect.Width * 0.1));
                rWin.Font = new Font("Century Gothic", (int)(rWin.Width * 0.1));
                bWin.Font = new Font("Century Gothic", (int)(bWin.Width * 0.1));
                yWin.Font = new Font("Century Gothic", (int)(yWin.Width * 0.1));
                showQuestionButton.Font = new Font("Century Gothic", (int)(showQuestionButton.Width * 0.115));
                labelAnswerRight.Font = new Font("Century Gothic", (int)(labelAnswerRight.Width * 0.115), System.Drawing.FontStyle.Bold);
                labelAnswerAfterTimer.Font = new Font("Century Gothic", (int)(labelAnswerAfterTimer.Width * 0.08));
                labelTimer.Font = new Font("Rockwell", (int)(labelTimer.Width * 0.2));
                startTimer.Font = new Font("Century Gothic", (int)(startTimer.Width * 0.075));
                buttonEarlyAnswer.Font = new Font("Century Gothic", (int)(buttonEarlyAnswer.Width * 0.075));
                rEarly.Font = new Font("Century Gothic", (int)(rEarly.Width * 0.1));
                bEarly.Font = new Font("Century Gothic", (int)(bEarly.Width * 0.1));
                yEarly.Font = new Font("Century Gothic", (int)(yEarly.Width * 0.1));
                showAnswer.Font = new Font("Century Gothic", (int)(showAnswer.Width * 0.115));
                Red.Font = new Font("Century Gothic", (int)(Red.Width * 0.1));
                Blue.Font = new Font("Century Gothic", (int)(Blue.Width * 0.1));
                Yellow.Font = new Font("Century Gothic", (int)(Yellow.Width * 0.1));
                White.Font = new Font("Century Gothic", (int)(White.Width * 0.1));
                label1.Font = new Font("Century Gothic", (int)(label1.Width * 0.0225));
                buttonNewRound.Font = new Font("Century Gothic", (int)(buttonNewRound.Width * 0.05));
                labelEndRound.Font = new Font("Century Gothic", (int)(labelEndRound.Width * 0.04));
                okNoQuestions.Font = new Font("Century Gothic", (int)(okNoQuestions.Width * 0.05));
                label5.Font = new Font("Century Gothic", (int)(label5.Width * 0.04)); ;
            }
            catch{}
            panelQuestion.Visible = visibility[0];
            panelAnswer.Visible = visibility[1];
            Yellow.Visible = visibility[2];
            Blue.Visible = visibility[3];
            Red.Visible = visibility[4];
            tableLayoutPanel3.Visible = visibility[5];
            buttonEarlyAnswer.Visible = visibility[6];
            showAnswer.Visible = visibility[7];
            showQuestionButton.Visible = visibility[8];
            panelEndRound.Visible = visibility[9];
            panelNoQuestions.Visible = visibility[10];
            drawTable();
        }


        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (rotationSpeed == 0)
            {
                rotationMusic.Play();
                timer1.Enabled = true;
                timer2.Enabled = true;
            }
            rotationSpeed += (int) ((maxSpeed-rotationSpeed)*0.5);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            changeSize();
        }

        private void nextRotation(String colorLetter)
        {
            Red.Visible = false;
            Blue.Visible = false;
            Yellow.Visible = false;
            White.Visible = false;
            arrayQuestionLen--;
            questionNumber++;
            Console.WriteLine("index " + ((i + 3) / 6) % 12);
            cellColor[((i + 3) / 6) % 12] = colorLetter;
            cellPlay[((i + 3) / 6) % 12] = false;
            if (questionNumber - roundNumber % 3 == roundQuestions)
            {
                roundNumber++;
                questionNumber = roundNumber % 3;
                int[] rightAnswers = { rCorrectAnswers, bCorrectAnswers, yCorrectAnswers };
                int maximum = rightAnswers.Max();
                if (maximum != 0)
                {
                    if (rCorrectAnswers == maximum)
                    {
                        rRoundWin++;
                        rWin.Text = "1 ряд: " + rRoundWin.ToString();
                    }
                    if (bCorrectAnswers == maximum)
                    {
                        bRoundWin++;
                        bWin.Text = "2 ряд: " + bRoundWin.ToString();
                    }
                    if (yCorrectAnswers == maximum)
                    {
                        yRoundWin++;
                        yWin.Text = "3 ряд: " + yRoundWin.ToString();
                    }
                }
                i = 0;
                rCorrectAnswers = 0;
                bCorrectAnswers = 0;
                yCorrectAnswers = 0;
                for (int k = 0; k < 12; k++)
                {
                    cellColor[k] = "w";
                    cellPlay[k] = false;
                }
                labelEndRound.Text = "Раунд №" + roundNumber + " завершен.";
                roundEndSound.Play();
                panelEndRound.Visible = true;
            }
            drawTable();
            pictureBox2.Enabled = true;
            pictureBox2.Cursor = Cursors.Hand;
            labelRotation.Text = "Волчок крутит " + (questionNumber%3+1) + " ряд.";
            rCorrect.Text = rCorrectAnswers.ToString();
            bCorrect.Text = bCorrectAnswers.ToString();
            yCorrect.Text = yCorrectAnswers.ToString();
            panelQuestion.Visible = false;
            panelAnswer.Visible = false;
        }

        private void Red_Click(object sender, EventArgs e)
        {
            rCorrectAnswers++;
            nextRotation("r");
        }

        private void Blue_Click(object sender, EventArgs e)
        {
            bCorrectAnswers++;
            nextRotation("b");
        }

        private void Yellow_Click(object sender, EventArgs e)
        {
            yCorrectAnswers++;
            nextRotation("y");
        }

        private void White_Click(object sender, EventArgs e)
        {
            nextRotation("w");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (rotationSpeed != 0)
            {
                rotationSpeed -= 1;
                if (rotationSpeed == 0) arrowStopped = true;
                else if(rotationSpeed < 7 && pictureBox2.Enabled == true)
                {
                    pictureBox2.Enabled = false;
                    pictureBox2.Cursor = Cursors.Arrow;
                }
            }
            else if(arrowStopped)
            {
                gongSound.Play();
                cellPlay[((i + 3) / 6) % 12] = true;
                drawTable();
                showQuestionButton.Visible = true;
                timer1.Enabled = false;
                timer2.Enabled = false;
                arrowStopped = false;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (i != 0)
            {
                pictureBox1.Image = imageList1.Images[i % 72];
                pictureBox2.Image = imageList2.Images[(int)(rotationSpeed / 3.7)];
            }
            i += rotationSpeed;
        }

        private void showQuestionButton_Click(object sender, EventArgs e)
        {
            showQuestionButton.Visible = false;
            if (arrayQuestionLen != 0)
            {
                webBrowser1.Url = new Uri(Application.StartupPath + @"\" + dirs[arrayQuestionLen-1] + @"\question.html");
                sectorOwner = cellColor[((i + 3) / 6) % 12];
                switch (sectorOwner)
                {
                    case "w":
                        labelAnswerRight.Text = (questionNumber % 3 + 1) + " ряд";
                        break;
                    case "r":
                        labelAnswerRight.Text = "1 ряд";
                        break;
                    case "b":
                        labelAnswerRight.Text = "2 ряд";
                        break;
                    case "y":
                        labelAnswerRight.Text = "3 ряд";
                        break;
                }
                labelTimer.Visible = true;
                startTimer.Visible = true;
                pictureSectorOwner.Image = Image.FromFile("images/" + sectorOwner + "Sector.png");
                questionAttentionSound.Play();
                panelQuestion.Visible = true;
            }
            else
            {
                roundEndSound.Play();
                panelNoQuestions.Visible = true;
            }
        }

        

        private void timerQuestion_Tick(object sender, EventArgs e)
        {
            timeQ--;
            labelTimer.Text = (timeQ / 60).ToString("D2") + ":" + (timeQ % 60).ToString("D2");
            if (timeQ == 0)
            {
                beginEndSound.Play();
                timerQuestion.Enabled = false;
                buttonEarlyAnswer.Visible = false;
                showAnswer.Visible = true;
            }
            else if(timeQ == 10)
            {
                tenSecondsSound.Play();
            }
            else if (timeQ == 59)
            {
                buttonEarlyAnswer.Visible = true;
            }
        }

        private void buttonEarlyAnswer_Click(object sender, EventArgs e)
        {
            tableLayoutPanel3.Visible = true;
            buttonEarlyAnswer.Visible = false;
            timerQuestion.Enabled = false;
            labelTimer.Visible = false;
            labelTimer.Text = "01:00";
        }

            
        private void startTimer_Click(object sender, EventArgs e)
        {
            timeQ = 60;
            timerQuestion.Enabled = true;
            startTimer.Visible = false;
            beginEndSound.Play();
        }

        private void rEarly_Click(object sender, EventArgs e)
        {
            tableLayoutPanel3.Visible = false;
            showAnswer.Visible = true;
            earlyAnswerColor = "r";
        }

        private void bEarly_Click(object sender, EventArgs e)
        {
            tableLayoutPanel3.Visible = false;
            showAnswer.Visible = true;
            earlyAnswerColor = "b";
        }

        private void yEarly_Click(object sender, EventArgs e)
        {
            tableLayoutPanel3.Visible = false;
            showAnswer.Visible = true;
            earlyAnswerColor = "y";
        }

        private void showAnswer_Click(object sender, EventArgs e)
        {
            webBrowser2.Url = new Uri(Application.StartupPath + @"\" + dirs[arrayQuestionLen - 1] + @"\answer.html");
            switch (sectorOwner)
            {
                case "w":
                    switch(questionNumber % 3)
                    {
                        case 0:
                            Red.Visible = true;
                            break;
                        case 1:
                            Blue.Visible = true;
                            break;
                        case 2:
                            Yellow.Visible = true;
                            break;
                    }
                    break;
                case "r":
                    Red.Visible = true;
                    break;
                case "b":
                    Blue.Visible = true;
                    break;
                case "y":
                    Yellow.Visible = true;
                    break;
            }
            switch (earlyAnswerColor)
            {
                case "r":
                    Red.Visible = true;
                    break;
                case "b":
                    Blue.Visible = true;
                    break;
                case "y":
                    Yellow.Visible = true;
                    break;
            }
            earlyAnswerColor = " ";
            White.Visible = true;
            showAnswer.Visible = false;
            panelAnswer.Visible = true;
        }

        private void startGame_Click(object sender, EventArgs e)
        {
            roundQuestions = int.Parse(textBoxStart.Text);
            panelStart.Visible = false;
        }

        private void textBoxStart_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }

        private void buttonNewRound_Click(object sender, EventArgs e)
        {
            panelEndRound.Visible = false;
        }

        private void okNoQuestions_Click(object sender, EventArgs e)
        {
            panelNoQuestions.Visible = false;
        }






        /*private void arrowCreate()
        {
            for (int i = 0; i < 12; i++)
            {
                Image arrow = Image.FromFile("images/cell.png");
                Bitmap clone = new Bitmap(arrow.Width, arrow.Height);
                Graphics gfx = Graphics.FromImage(clone);
                gfx.TranslateTransform(arrow.Width / 2, arrow.Height / 2);
                gfx.RotateTransform(i * 30);
                gfx.TranslateTransform(-arrow.Width / 2, -arrow.Height / 2);
                gfx.DrawImage(arrow, 0, 0, arrow.Width, arrow.Height);
                clone.Save("images/cells/cell" + i + ".png");
            }
        }*/
    }
}
