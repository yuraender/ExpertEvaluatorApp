using ExpertEvaluator.Entities;
using ExpertEvaluator.Utils;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ExpertEvaluator.Forms {

    public partial class StatisticsForm : Form {

        public StatisticsForm() {
            InitializeComponent();
            ExpertAPI api = new ExpertAPI("https://ee.yuraender.ru");

            Statistics statistics = api.GetStatistics();
            label2.Text += statistics.Experts;
            foreach (KeyValuePair<string, int> country in statistics.Countries) {
                label3.Text += "\n" + country.Key + ": " + country.Value;
            }
            label4.Text += Math.Round(statistics.Concordance, 5);
            label5.Text += Math.Round(api.Calculate(Subject.DNS_SHOP), 5);
            label6.Text += Math.Round(api.Calculate(Subject.CITILINK), 5);
        }
    }
}
