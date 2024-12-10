using ExpertEvaluator.Entities;
using ExpertEvaluator.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ExpertEvaluator.Forms {

    public partial class RankForm : Form {

        private readonly Expert _expert;
        private readonly ExpertAPI API;

        private int rowIndex = -1;
        private int columnIndex = -1;

        public RankForm(Expert expert) {
            InitializeComponent();
            _expert = expert;
            API = new ExpertAPI("http://me.yuraender.ru:8083");

            // Настройка взаимодействия с DataGridView
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AllowUserToOrderColumns = false;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.ScrollBars = ScrollBars.None;

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

            // Включение возможности перетаскивания строк
            dataGridView1.AllowDrop = true;
            dataGridView1.CellMouseDown += dataGridView1_CellMouseDown;
            dataGridView1.CellMouseMove += dataGridView1_CellMouseMove;
            dataGridView1.CellMouseUp += dataGridView1_CellMouseUp;
            dataGridView1.DragDrop += dataGridView1_DragDrop;
            dataGridView1.DragOver += dataGridView1_DragOver;
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;

            // Привязка данных к DataGridView
            dataGridView1.Columns.Add("Meow", "Meow");
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            foreach (Criterion item in API.GetCriteria()) {
                dataGridView1.Rows.Add(item);
            }

            int length = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows) {
                int l = ((Criterion) row.Cells[0].Value).Name.Length;
                if (l > length) {
                    length = l;
                }
            }
            if (length <= 30) {
                Width = 430;
            } else if (length <= 60) {
                Width = 550;
            } else if (length <= 90) {
                Width = 800;
            } else {
                Width = 850;
            }
            Height += 20 * dataGridView1.Rows.Count - 45;
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                rowIndex = e.RowIndex;
                columnIndex = e.ColumnIndex;
                dataGridView1.DoDragDrop(dataGridView1[e.ColumnIndex, e.RowIndex].Value, DragDropEffects.Move);
            }
        }

        private void dataGridView1_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e) {
            if (e.Button == MouseButtons.Left && rowIndex != -1 && columnIndex != -1) {
                dataGridView1.DoDragDrop(dataGridView1[columnIndex, rowIndex].Value, DragDropEffects.Move);
            }
        }

        private void dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e) {
            rowIndex = -1;
            columnIndex = -1;
        }

        private void dataGridView1_DragDrop(object sender, DragEventArgs e) {
            Point clientPoint = dataGridView1.PointToClient(new Point(e.X, e.Y));
            int targetRowIndex = dataGridView1.HitTest(clientPoint.X, clientPoint.Y).RowIndex;
            if (targetRowIndex >= 0 && rowIndex != -1 && columnIndex != -1) {
                DataGridViewRow rowToMove = dataGridView1.Rows[rowIndex];
                dataGridView1.Rows.RemoveAt(rowIndex);
                dataGridView1.Rows.Insert(targetRowIndex, rowToMove);
            }
        }

        private void dataGridView1_DragOver(object sender, DragEventArgs e) {
            e.Effect = DragDropEffects.Move;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e) {
            dataGridView1.ClearSelection();
        }

        private void button1_Click(object sender, EventArgs e) {
            List<Criterion> order = new List<Criterion>();
            foreach (DataGridViewRow row in dataGridView1.Rows) {
                order.Add((Criterion) row.Cells[0].Value);
            }
            foreach (Subject subject in Enum.GetValues(typeof(Subject))) {
                ScoreForm scoreForm = new ScoreForm(_expert, order, subject);
                scoreForm.FormClosed += (s, ev) => {
                    if (subject == Subject.CITILINK) {
                        MessageBox.Show("Благодарим Вас за экспертное заключение!");
                        Application.Exit();
                    }
                };
                Hide();
                scoreForm.ShowDialog(this);
            }
        }
    }
}
