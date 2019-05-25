using System;
using System.Drawing;
using System.Windows.Forms;

namespace Games
{
    public partial class Game2 : Form
    {
        int width, height, time;
        int difficult = 0;
        int score = 0;
        TableLayoutPanel table;
        Label label;
        Timer timer;
        Form parent;
        bool isUsualExit = true;
        public Game2(Form parent)
        {
            InitializeComponent();
            this.Text = "Second game";
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
                label.Text = time.ToString();
                if (time <= 0)
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

        void StartGame()
        {
            Random rnd = new Random();
            width = rnd.Next(3, 7);
            height = width;

            table = new TableLayoutPanel();
            table.Dock = DockStyle.Fill;

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8f));
            for (int i = 1; i < width + 1; i++)
                table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / width));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8f));

            table.RowStyles.Add(new RowStyle(SizeType.Percent, 30f));
            for (int i = 1; i < height + 1; i++)
                table.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / height));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 15f));

            var rand_x = rnd.Next(1, width);
            var rand_y = rnd.Next(1, height);
            var rrnd_sad = rnd.Next(1, 4);
            var sad_image = new PictureBox();
            sad_image.ImageLocation = $"images//sad{rrnd_sad}.png";
            sad_image.SizeMode = PictureBoxSizeMode.StretchImage;
            sad_image.Dock = DockStyle.Fill;
            sad_image.Click += RightClick;
           

            for (int column = 0; column < width + 2; column++)
                for (int row = 0; row < height + 2; row++)
                {
                    if (column == 0 || row == 0 || column == (width + 1) || row == (height + 1))
                        table.Controls.Add(new Panel(), column, row);
                    else if (column == rand_x && row == rand_y)
                        table.Controls.Add(sad_image, rand_x, rand_y);
                    else
                    {
                        var rrnd = rnd.Next(1, 4);
                        var image = new PictureBox();
                        image.ImageLocation = $"images//fun{rrnd}.png";
                        image.SizeMode = PictureBoxSizeMode.StretchImage;
                        image.Dock = DockStyle.Fill;
                        image.Click += WrongClick;
                        table.Controls.Add(image, column, row);

                    }
                }
            Controls.Add(table);
            timer.Interval = 1000 - difficult;
            timer.Start();
        }

        void RightClick(object sender, EventArgs e)
        {
            timer.Stop();
            time = 10;
            label.Text = "10";
            difficult += 30;
            score += difficult;
            Controls.Remove(table);
            StartGame();
        }

        void WrongClick(object sender, EventArgs e)
        {
            time--;
        }

        void CloseForm(object sender, EventArgs e)
        {
            Close();
        }

        private void Game2_FormClosed_1(object sender, FormClosedEventArgs e)
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
