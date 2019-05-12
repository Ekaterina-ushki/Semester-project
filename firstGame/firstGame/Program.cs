using System;
using System.Drawing;
using System.Windows.Forms;

namespace FirstGame
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
                label.Text = (Convert.ToInt32(label.Text) - 1).ToString();
                if (time == 0)
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
            width = rnd.Next(2, 5);
            height = rnd.Next(2, 5);

            table = new TableLayoutPanel();
            table.Dock = DockStyle.Fill;

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5f));
            for (int i = 1; i < width + 1; i++)
                table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / width));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5f));

            table.RowStyles.Add(new RowStyle(SizeType.Percent, 30f));
            for (int i = 1; i < height + 1; i++)
                table.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / height));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 5f));

            var allButton = width * height;
            var firstColor = 0;
            for (int column = 0; column < width + 2; column++)
                for (int row = 0; row < height + 2; row++)
                {
                    if (column == 0 || row == 0 || column == (width + 1) || row == (height + 1))
                        table.Controls.Add(new Panel(), column, row);
                    else
                    {
                        var rrnd = rnd.Next() % 2;
                        if (rrnd == 0)
                            firstColor += 1;
                        var button = new Button();
                        if (firstColor < allButton)
                        {
                            button.BackColor = rrnd == 0 ? Color.DarkMagenta : Color.Yellow;
                            button.Dock = DockStyle.Fill;
                            button.Click += MakeMove;
                            table.Controls.Add(button, column, row);
                        }
                        else
                        {
                            button.BackColor = Color.Yellow;
                            button.Dock = DockStyle.Fill;
                            button.Click += MakeMove;
                            table.Controls.Add(button, column, row);
                        }


                    }
                }
            Controls.Add(table);
            if (CheckWin())
                StartGame();
            else
                timer.Interval = 1000 - difficult;
            timer.Start();
        }

        void MakeMove(object sender, EventArgs e)
        {
            var position = table.GetCellPosition((Control)sender);
            var btn = (Button)sender;
            btn.BackColor = btn.BackColor == Color.DarkMagenta ? Color.Yellow : Color.DarkMagenta;
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
            var color = table.GetControlFromPosition(1, 1).BackColor;
            for (int column = 1; column < width + 1; column++)
                for (int row = 1; row < height + 1; row++)
                {
                    if (table.GetControlFromPosition(column, row).BackColor != color)
                        return false;
                }
            return true;
        }

        public static void Main()
        {
            Application.Run(new MyForm { ClientSize = new Size(600, 600) });
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            var result = MessageBox.Show("Хотите закончить игру?", "", MessageBoxButtons.YesNo);
            if (result != DialogResult.Yes) e.Cancel = true;
        }
    }
}
