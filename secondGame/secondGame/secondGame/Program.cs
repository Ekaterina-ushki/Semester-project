using System;
using System.Drawing;
using System.Windows.Forms;

namespace secondGame
{
    class MyForm : Form
    {
        int width, height, time;
        int difficult = 0;
        int score = 0;
        TableLayoutPanel table;
        Label label;
        Timer timer;

        public MyForm()
        {
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

            StartGame();
        }

        void GameOver()
        {
            timer.Stop();
            label.Font = new Font(label.Font.Name, 30, label.Font.Style);
            label.Text = $"Игра окончена. Результат: {score}";
            for (int column = 1; column < width + 1; column++)
                for (int row = 1; row < height + 1; row++)
                    table.GetControlFromPosition(column, row).Enabled = false;
        }

        void StartGame()
        {
            Random rnd = new Random();
            width = rnd.Next(3, 7);
            height = rnd.Next(3, 7);

            table = new TableLayoutPanel();
            table.Dock = DockStyle.Fill;

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5f));
            for (int i = 1; i < width + 1; i++)
                table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f/width));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5f));

            table.RowStyles.Add(new RowStyle(SizeType.Percent, 40f));
            for (int i = 1; i < height + 1; i++)
                table.RowStyles.Add(new RowStyle(SizeType.Percent, 100f/height));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 5f));

            var rand_x = rnd.Next(1, width);
            var rand_y = rnd.Next(1, height);
            var rrnd_sad = rnd.Next(1, 4);
            var sad_image = new PictureBox();
            sad_image.ImageLocation = $"images//sad{rrnd_sad}.jpg";
            sad_image.SizeMode = PictureBoxSizeMode.StretchImage;
            sad_image.Dock = DockStyle.Fill;
            sad_image.Click += RightClick;
            table.Controls.Add(sad_image, rand_x, rand_y);

            for (int column = 0; column < width + 2; column++)
                for (int row = 0; row < height + 2; row++)
                {
                    if (column == 0 || row == 0 || column == (width + 1) || row == (height + 1))
                        table.Controls.Add(new Panel(), column, row);
                    else if (!(table.GetControlFromPosition(column, row) is PictureBox))
                    { 
                        var rrnd = rnd.Next(1,4);
                        var image = new PictureBox();
                        image.ImageLocation = $"images//fun{rrnd}.jpg";
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


        public static void Main()
        {
            Application.Run(new MyForm { ClientSize = new Size(600, 600) });
        }

        /*protected override void OnFormClosing(FormClosingEventArgs e)
        {
            var result = MessageBox.Show("Хотите закончить игру?", "", MessageBoxButtons.YesNo);
            if (result != DialogResult.Yes) e.Cancel = true;
        }*/
    }
}
