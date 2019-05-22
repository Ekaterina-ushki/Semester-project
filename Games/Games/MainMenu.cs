using System;
using System.Drawing;
using System.Windows.Forms;

namespace Games
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            BackColor = Color.LightGray;
            InitializeComponent();
        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var form = new Game1(this);
            Hide();
            form.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var form = new Game2(this);
            Hide();
            form.Show();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            var result = MessageBox.Show("Хотите закрыть игру?", "", MessageBoxButtons.YesNo);
            if (result != DialogResult.Yes) e.Cancel = true;
        }
    }
}
