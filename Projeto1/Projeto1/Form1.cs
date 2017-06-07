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

        public Form1() {
            InitializeComponent();
            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
        }

        private void trackBarTamanho_Scroll(object sender, EventArgs e) {
            buttonA.Size = new Size(trackBarTamanho.Value, trackBarTamanho.Value);
            buttonB.Size = new Size(trackBarTamanho.Value, trackBarTamanho.Value);
            labelTam.Text = trackBarTamanho.Value.ToString();
        }

        private void trackBarDistancia_Scroll(object sender, EventArgs e) {
            buttonA.Location = new Point(
                380 - trackBarDistancia.Value,
                buttonA.Location.Y);
            buttonB.Location = new Point(
                380 + trackBarDistancia.Value,
                buttonA.Location.Y);
            labelDist.Text = trackBarDistancia.Value.ToString();
        }

        private void buttonA_Click(object sender, EventArgs e) {
            if (timerTempoClick.Enabled) {
                if (WaitingBtnA) {
                    timerTempoClick.Stop();
                    chart1.Series[0].Points.AddY(tempoEsperado);
                    tempos_de_click.Enqueue(tempoEsperado);
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
                    tempos_de_click.Enqueue(tempoEsperado);
                    tempoEsperado = 0;
                }
            } else {
                WaitingBtnA = true;
                timerTempoClick.Start();
            }
        }

        private void timerTempoClick_Tick(object sender, EventArgs e) {
            tempoEsperado += 1;
        }

    }
}
