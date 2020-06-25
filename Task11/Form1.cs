using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Task11
{
    public partial class Form1 : Form
    {
        Random rand = new Random();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();

            double prob1 = (double)numericProb1.Value * 0.01;
            double prob2 = (double)numericProb2.Value * 0.01;
            double prob3 = (double)numericProb3.Value * 0.01;
            double prob4 = (double)numericProb4.Value * 0.01;
            int experiments = (int)numericExperiments.Value;

            double average = (prob1 * 1 + prob2 * 2 + prob3 * 3 + prob4 * 4 + (1 - (prob1 + prob2 + prob3 + prob4)) * 5);
            double variance = (prob1 * Math.Pow(1 - average, 2) + prob2 * Math.Pow(2 - average, 2) + prob3 * Math.Pow(3 - average, 2) + prob4 * Math.Pow(4 - average, 2) + (1 - (prob1 + prob2 + prob3 + prob4)) * Math.Pow(5 - average, 2));

            List<int> data = new List<int>();
            for (int i = 0; i < 5; i++)
            {
                data.Add(0);
            }

            for (int i = 0; i < experiments; i++)
            {
                data[GenerateEvent(prob1, prob2, prob3, prob4)]++;
            }

            List<double> empir = new List<double>();

            for (int i = 0; i < 5; i++)
            {
                double h = (double)data[i] / experiments;
                empir.Add(h);
                chart1.Series[0].Points.AddXY(i + 1, h);
            }

            double mean = 0.0;
            for (int i = 0; i < data.Count; i++)
            {
                mean += (i + 1) * data[i];
            }

            mean /= experiments;
            AvgLabel.Text = "Average: " + mean + " (error: " + (int)CountVarError(mean, average) + "%)";
            double realVariance = 0;

            for (int i = 0; i < empir.Count; i++)
            {
                realVariance += empir[i] * Math.Pow((i + 1) - mean, 2);
            }

            VarLabel.Text = "Variance: " + realVariance + " (error: " + (int)CountVarError(realVariance, variance) + "%)";

            double chiCritical = 11.070;
            double chi = 0;
            chi += (Math.Pow(data[0], 2) / (experiments * prob1));
            chi += (Math.Pow(data[1], 2) / (experiments * prob2));
            chi += (Math.Pow(data[2], 2) / (experiments * prob3));
            chi += (Math.Pow(data[3], 2) / (experiments * prob4));
            chi += (Math.Pow(data[4], 2) / (experiments * (1 - (prob1 + prob2 + prob3 + prob4))));
            chi -= experiments;

            if (chi > chiCritical)
            {
                ChiLabel.Text = "Chi-Squared: " + chi.ToString() + " > " + chiCritical + " is false";
            }
            else
            {
                ChiLabel.Text = "Chi-Squared: " + chi.ToString() + " < " + chiCritical + " is true";
            }

        }

        private int GenerateEvent(double prob1, double prob2, double prob3, double prob4)
        {
            double x = rand.NextDouble();

            x -= prob1;
            if (x <= 0)
                return 0;

            x -= prob2;
            if (x <= 0)
                return 1;

            x -= prob3;
            if (x <= 0)
                return 2;

            x -= prob4;
            if (x <= 0)
                return 3;

            return 4;
        }

        private double CountMeanError(double mean, double average)
        {
            double absolute = Math.Abs(mean - average);
            return (absolute / Math.Abs(average)) * 100;
        }

        private double CountVarError(double realVariance, double variance)
        {
            double absolute = Math.Abs(realVariance - variance);
            return (absolute / Math.Abs(variance)) * 100;
        }

    }
}
