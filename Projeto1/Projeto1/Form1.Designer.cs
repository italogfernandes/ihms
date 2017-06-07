namespace Projeto1 {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.buttonA = new System.Windows.Forms.Button();
            this.buttonB = new System.Windows.Forms.Button();
            this.trackBarDistancia = new System.Windows.Forms.TrackBar();
            this.trackBarTamanho = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelTam = new System.Windows.Forms.Label();
            this.labelDist = new System.Windows.Forms.Label();
            this.timerTempoClick = new System.Windows.Forms.Timer(this.components);
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarDistancia)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTamanho)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonA
            // 
            this.buttonA.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonA.Location = new System.Drawing.Point(390, 150);
            this.buttonA.Name = "buttonA";
            this.buttonA.Size = new System.Drawing.Size(20, 20);
            this.buttonA.TabIndex = 0;
            this.buttonA.Text = "A";
            this.buttonA.UseVisualStyleBackColor = true;
            this.buttonA.Click += new System.EventHandler(this.buttonA_Click);
            // 
            // buttonB
            // 
            this.buttonB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonB.Location = new System.Drawing.Point(410, 150);
            this.buttonB.Name = "buttonB";
            this.buttonB.Size = new System.Drawing.Size(20, 20);
            this.buttonB.TabIndex = 1;
            this.buttonB.Text = "B";
            this.buttonB.UseVisualStyleBackColor = true;
            this.buttonB.Click += new System.EventHandler(this.buttonB_Click);
            // 
            // trackBarDistancia
            // 
            this.trackBarDistancia.Location = new System.Drawing.Point(9, 399);
            this.trackBarDistancia.Maximum = 350;
            this.trackBarDistancia.Minimum = 10;
            this.trackBarDistancia.Name = "trackBarDistancia";
            this.trackBarDistancia.Size = new System.Drawing.Size(316, 45);
            this.trackBarDistancia.TabIndex = 2;
            this.trackBarDistancia.Value = 10;
            this.trackBarDistancia.Scroll += new System.EventHandler(this.trackBarDistancia_Scroll);
            // 
            // trackBarTamanho
            // 
            this.trackBarTamanho.Location = new System.Drawing.Point(12, 335);
            this.trackBarTamanho.Maximum = 100;
            this.trackBarTamanho.Minimum = 20;
            this.trackBarTamanho.Name = "trackBarTamanho";
            this.trackBarTamanho.Size = new System.Drawing.Size(313, 45);
            this.trackBarTamanho.TabIndex = 3;
            this.trackBarTamanho.Value = 20;
            this.trackBarTamanho.Scroll += new System.EventHandler(this.trackBarTamanho_Scroll);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 319);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Tamanho dos botões:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 383);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Distância entre os botões:";
            // 
            // labelTam
            // 
            this.labelTam.AutoSize = true;
            this.labelTam.Location = new System.Drawing.Point(130, 319);
            this.labelTam.Name = "labelTam";
            this.labelTam.Size = new System.Drawing.Size(19, 13);
            this.labelTam.TabIndex = 6;
            this.labelTam.Text = "20";
            // 
            // labelDist
            // 
            this.labelDist.AutoSize = true;
            this.labelDist.Location = new System.Drawing.Point(150, 383);
            this.labelDist.Name = "labelDist";
            this.labelDist.Size = new System.Drawing.Size(19, 13);
            this.labelDist.TabIndex = 7;
            this.labelDist.Text = "20";
            // 
            // timerTempoClick
            // 
            this.timerTempoClick.Interval = 1;
            this.timerTempoClick.Tick += new System.EventHandler(this.timerTempoClick_Tick);
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(353, 311);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(419, 249);
            this.chart1.TabIndex = 9;
            this.chart1.Text = "chart1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.labelDist);
            this.Controls.Add(this.labelTam);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.trackBarTamanho);
            this.Controls.Add(this.trackBarDistancia);
            this.Controls.Add(this.buttonB);
            this.Controls.Add(this.buttonA);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "Form1";
            this.Text = "Primeiro Trabalho de IHMS";
            ((System.ComponentModel.ISupportInitialize)(this.trackBarDistancia)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTamanho)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonA;
        private System.Windows.Forms.Button buttonB;
        private System.Windows.Forms.TrackBar trackBarDistancia;
        private System.Windows.Forms.TrackBar trackBarTamanho;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelTam;
        private System.Windows.Forms.Label labelDist;
        private System.Windows.Forms.Timer timerTempoClick;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
    }
}

