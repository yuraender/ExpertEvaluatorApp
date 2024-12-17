using ExpertEvaluator.Entities;
using ExpertEvaluator.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ExpertEvaluator.Forms {

    public partial class ScoreForm : Form {

        private readonly Expert _expert;
        private readonly Subject _subject;
        private readonly ExpertAPI API;

        public ScoreForm(Expert expert, List<Criterion> order, Subject subject) {
            InitializeComponent();
            _expert = expert;
            _subject = subject;
            API = new ExpertAPI("https://ee.yuraender.ru");

            Text += _subject == Subject.DNS_SHOP ? "DNS – интернет-магазин" : "Ситилинк – интернет-магазин";
            if (subject == Subject.DNS_SHOP) {
                button1.Text = "Продолжить";
            }

            // Настройка взаимодействия с DataGridView
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AllowUserToOrderColumns = false;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.ScrollBars = ScrollBars.None;
            dataGridView1.DataError += dataGridView1_DataError;
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;

            // Настройка внешнего вида DataGridView
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.BackgroundColor = Color.LightGray;
            dataGridView1.DefaultCellStyle.BackColor = Color.White;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dataGridView1.DefaultCellStyle.Font = new Font("Arial", 10);
            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Установка высоты строк, основываясь на содержимом
            dataGridView1.ColumnHeadersVisible = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            // Привязка данных к DataGridView
            dataGridView1.Columns.Add("Meow", "Meow");
            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            DataGridViewComboBoxColumn scoreColumn = new DataGridViewComboBoxColumn();
            scoreColumn.HeaderText = "Bark";
            scoreColumn.ValueType = typeof(int);
            scoreColumn.DataSource = new List<int> {
                1, 2, 3, 4, 5, 6, 7, 8, 9, 10
            };
            scoreColumn.Width = 50;
            dataGridView1.Columns.Add(scoreColumn);
            foreach (Criterion item in order) {
                dataGridView1.Rows.Add(item, 1);
            }

            int length = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows) {
                int l = ((Criterion) row.Cells[0].Value).Name.Length;
                if (l > length) {
                    length = l;
                }
            }
            if (length <= 30) {
                Width = 450;
            } else if (length <= 60) {
                Width = 600;
            } else if (length <= 90) {
                Width = 850;
            } else {
                Width = 900;
            }
            Height += 25 * dataGridView1.Rows.Count - 45;
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e) {
            e.Cancel = true;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e) {
            dataGridView1.ClearSelection();
        }

        private void button1_Click(object sender, EventArgs e) {
            List<CriterionScore> evaluation = new List<CriterionScore>();
            foreach (DataGridViewRow row in dataGridView1.Rows) {
                Criterion criterion = (Criterion) row.Cells[0].Value;
                int rank = dataGridView1.Rows.Count - row.Index;
                int score = (int) row.Cells[1].Value;
                evaluation.Add(new CriterionScore {
                    Criterion = criterion,
                    Expert = _expert,
                    Subject = _subject,
                    Rank = rank,
                    Score = score
                });
            }
            API.SendEvaluation(evaluation);
            Close();
        }
    }
}
