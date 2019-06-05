using System;
using System.Drawing;
using System.Windows.Forms;

namespace Games
{
    public partial class Game1 : Form
    {
        int width, height, time;
        int difficult = 0;
        int score = 0;
        TableLayoutPanel table;
        Label label;
        Timer timer;
        Form parent;
        bool isUsualExit = true;

        public Game1(Form parent)
        {
            InitializeComponent();
            this.Text = "First game";
            this.parent = parent;
            BackColor = Color.DarkGray;
            label = new Label
            {
                Text = "10",
                Height = 100,
                Top = 20,
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleCenter
            };

            label.Font = new Font(label.Font.Name, 40, label.Font.Style);
            Controls.Add(label);

            time = 10;
            timer = new Timer();
            timer.Tick += (sender, args) =>
            {
                time--;
                label.Text = (Convert.ToInt32(label.Text) - 1).ToString();
                if (time == 0)
                    GameOver();
            };
            Button close = new Button
            {
                Text = "В главное меню",
                Dock = DockStyle.Bottom,
                Height = 50

            };
            close.Click += CloseForm;
            Controls.Add(close);
            StartGame();
        }

        void StartGame()
        {
            Random rnd = new Random();
            width = rnd.Next(2, 5);
            height = width;

            table = new TableLayoutPanel();
            table.Dock = DockStyle.Fill;

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5f));
            for (int i = 1; i < width + 1; i++)
                table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / width));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5f));

            table.RowStyles.Add(new RowStyle(SizeType.Percent, 30f));
            for (int i = 1; i < height + 1; i++)
                table.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / height));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 15f));

            var allImage = width * height;
            var firstImage = 0;
            for (int column = 0; column < width + 2; column++)
                for (int row = 0; row < height + 2; row++)
                {
                    if (column == 0 || row == 0 || column == (width + 1) || row == (height + 1))
                        table.Controls.Add(new Panel(), column, row);
                    else
                    {
                        var rrnd = rnd.Next() % 2;
                        if (rrnd == 0)
                            firstImage += 1;
                        var image = new PictureBox();
                        if (firstImage < allImage)
                        {
                            image.ImageLocation = $"images//cat{rrnd}.png";
                            image.SizeMode = PictureBoxSizeMode.StretchImage;
                            image.Dock = DockStyle.Fill;
                            image.Click += MakeMove;
                            table.Controls.Add(image, column, row);
                        }
                        else
                        {
                            image.ImageLocation = "images//cat1.png";
                            image.Dock = DockStyle.Fill;
                            image.Click += MakeMove;
                            table.Controls.Add(image, column, row);
                        }


                    }
                }
            Controls.Add(table);
            //if (CheckWin())
            //    StartGame();
            //else
            timer.Interval = 1000 - difficult;
            timer.Start();
        }

        void GameOver()
        {
            timer.Stop();
            for (int column = 1; column < width + 1; column++)
                for (int row = 1; row < height + 1; row++)
                    table.GetControlFromPosition(column, row).Enabled = false;
            var result = MessageBox.Show($"Ваш результат: {score}\nХотите начать заново?", "", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                timer.Stop();
                time = 10;
                label.Text = "10";
                timer.Interval = 1000;
                difficult = 0;
                score = 0;
                Controls.Remove(table);
                StartGame();
            }
            else
            {
                isUsualExit = false;
                Close();
            }
        }

        void MakeMove(object sender, EventArgs e)
        {
            var position = table.GetCellPosition((Control)sender);
            var im = (PictureBox)sender;
            im.ImageLocation = im.ImageLocation == $"images//cat1.png" ? $"images//cat0.png" : $"images//cat1.png";
            if (CheckWin())
            {
                timer.Stop();
                time = 10;
                label.Text = "10";
                difficult += 30;
                score += difficult;
                Controls.Remove(table);
                StartGame();
            }
        }

        bool CheckWin()
        {
            var image = ((PictureBox)table.GetControlFromPosition(1, 1)).ImageLocation;
            for (int column = 1; column < width + 1; column++)
                for (int row = 1; row < height + 1; row++)
                    if (((PictureBox)table.GetControlFromPosition(column, row)).ImageLocation != image)
                        return false;
            return true;
        }

       
        void CloseForm(object sender, EventArgs e)
        {
            Close();
        }

        private void Game1_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.parent.Show();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (isUsualExit)
            {
                timer.Stop();
                var result = MessageBox.Show("Хотите закончить игру?", "", MessageBoxButtons.YesNo);
                if (result != DialogResult.Yes)
                {
                    e.Cancel = true;
                    timer.Start();
                }
            }
        }
    }
}