using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Projeto1 {
    public partial class Form1 : Form {
        bool WaitingBtnA, WaitingBtnB;
        Queue<int> tempos_de_click = new Queue<int>(100);
        int tempoEsperado = 0;
        double ID, A, W;
        Color CorInicial;

        public Form1() {
            InitializeComponent();
            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            calculaID();
            CorInicial = buttonA.BackColor;
        }

        private void calculaID() {
            A = (double) trackBarDistancia.Value * 2;
            W = (double) trackBarTamanho.Value;
            ID = Math.Log(2 * A / W, 2);
            labelID.Text = ID.ToString();
            chart1.Series[0].Points.Clear();
            buttonA.BackColor = CorInicial;
            buttonB.BackColor = CorInicial;

        }

        private void trackBarTamanho_Scroll(object sender, EventArgs e) {
            buttonA.Size = new Size(trackBarTamanho.Value, trackBarTamanho.Value);
            buttonB.Size = new Size(trackBarTamanho.Value, trackBarTamanho.Value);
            labelTam.Text = trackBarTamanho.Value.ToString();
            calculaID();
        }

        private void trackBarDistancia_Scroll(object sender, EventArgs e) {
            buttonA.Location = new Point(
                380 - trackBarDistancia.Value,
                buttonA.Location.Y);
            buttonB.Location = new Point(
                380 + trackBarDistancia.Value,
                buttonA.Location.Y);
            labelDist.Text = trackBarDistancia.Value.ToString();
            calculaID();
        }

        private void buttonA_Click(object sender, EventArgs e) {
            if (timerTempoClick.Enabled) {
                if (WaitingBtnA) {
                    timerTempoClick.Stop();
                    chart1.Series[0].Points.AddY(tempoEsperado);
                    //richTextBox1.Text += ID.ToString() + "\t" + tempoEsperado.ToString() + "\n";
                    tempos_de_click.Enqueue(tempoEsperado);
                    if (chart1.Series[0].Points.Count() >= 30) {
                        buttonA.BackColor = Color.Green;
                        buttonB.BackColor = Color.Green;
                    }
                    tempoEsperado = 0;
                }
            } else {
                WaitingBtnB = true;
                timerTempoClick.Start();
            }
        }

        private void buttonB_Click(object sender, EventArgs e) {
            if (timerTempoClick.Enabled) {
                if (WaitingBtnB) {
                    timerTempoClick.Stop();
                    chart1.Series[0].Points.AddY(tempoEsperado);
                    //richTextBox1.Text += ID.ToString() + "\t" + tempoEsperado.ToString() + "\n";
                    tempos_de_click.Enqueue(tempoEsperado);
                    if(chart1.Series[0].Points.Count() >= 30) {
                        buttonA.BackColor = Color.Green;
                        buttonB.BackColor = Color.Green;
                    }
                    tempoEsperado = 0;
                }
            } else {
                WaitingBtnA = true;
                timerTempoClick.Start();
            }
        }

        private void buttonAnotar_Click(object sender, EventArgs e) {
            double media_de_tempos = 0.0;
            media_de_tempos = tempos_de_click.Average();
            tempos_de_click.Clear();
            richTextBox1.Text +=A.ToString() + "\t" +
                W.ToString() + "\t" +
                ID.ToString() + "\t" + media_de_tempos.ToString() + "\n";
        }

        private void timerTempoClick_Tick(object sender, EventArgs e) {
            tempoEsperado += 1;
        }

    }
}
