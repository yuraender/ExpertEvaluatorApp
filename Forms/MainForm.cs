using ExpertEvaluator.Entities;
using ExpertEvaluator.Utils;
using System;
using System.Windows.Forms;

namespace ExpertEvaluator.Forms {

    public partial class MainForm : Form {

        public MainForm() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(textBox1.Text)) {
                MessageBox.Show("Введите ваше имя.");
                return;
            }
            ExpertAPI api = new ExpertAPI("http://me.yuraender.ru:8083");
            int id = api.Login(textBox1.Text);
            if (id != -1) {
                RankForm rankForm = new RankForm(new Expert { ID = id });
                rankForm.FormClosing += (s, ev) => {
                    Application.Exit();
                };
                Hide();
                rankForm.ShowDialog(this);
            } else {
                MessageBox.Show("Вы уже дали экспертное заключение.");
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            StatisticsForm statisticsForm = new StatisticsForm();
            statisticsForm.ShowDialog(this);
        }
    }
}
