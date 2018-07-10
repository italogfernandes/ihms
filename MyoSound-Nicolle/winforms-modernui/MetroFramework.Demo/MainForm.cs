using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.IO.Ports;
using MetroFramework.Forms;
using ZedGraph;

namespace MetroFramework.Demo
{
    public partial class MainForm : MetroForm
    {
        private SerialPort minhaporta = new SerialPort();
        ZedGraphControl zg1 = new ZedGraphControl();
        LineItem emgLine;
        double valor_x = 0;
        double valor_limiar = 1.0;
        double valor_anterior = 0.0;
        System.Media.SoundPlayer soundplayer1;
        System.Media.SoundPlayer soundplayer2;
        System.Media.SoundPlayer soundplayer3;
        System.Media.SoundPlayer soundplayer4;

        public MainForm()
        {
            InitializeComponent();
        }

        #region Menu de Configurações
        private void metroTileSwitch_Click(object sender, EventArgs e)
        {
            var m = new Random();
            int next = m.Next(0, 13);
            metroStyleManager.Style = (MetroColorStyle)next;
        }

        private void metroTile1_Click(object sender, EventArgs e)
        {
            metroStyleManager.Theme = metroStyleManager.Theme == MetroThemeStyle.Light ? MetroThemeStyle.Dark : MetroThemeStyle.Light;
        }
        #endregion

        #region Menu Inicio
        private void metroTile3_Click(object sender, EventArgs e)
        {
            metroTabControl1.SelectedTab = metroTabPagePortaSerial;
        }

        private void metroTile4_Click(object sender, EventArgs e)
        {
            metroTabControl1.SelectedTab = metroTabPageGrafico;
        }

        private void metroTile5_Click(object sender, EventArgs e)
        {
            metroTabControl1.SelectedTab = metroTabPageMusica;
        }

        private void metroTile6_Click(object sender, EventArgs e)
        {
            metroTabControl1.SelectedTab = metroTabPageConfiguracoes;
        }
        #endregion

        private void MainForm_Load(object sender, EventArgs e)
        {
            adicionar_items_nas_combo_box();
            adicionar_grafico();
            carregar_sons();

        }

        private void carregar_sons()
        {
            soundplayer1 = new System.Media.SoundPlayer(@"d:\piano.wav");
        }

        private void adicionar_grafico()
        {
            zg1.Dock = DockStyle.Fill;
            zg1.GraphPane = new GraphPane(new RectangleF(), "Dados Lidos", "Index", "Volts");
            //emgLine.Color = Color.Blue; 
            emgLine = new LineItem("EMG", new double[1000], new double[1000], Color.Blue, SymbolType.None);

            //emgLine = new LineItem("EMG");
            //emgLine.Symbol = Symbol.None;
            //emgLine.AddPoint(0, 0.0);
            //emgLine.AddPoint(1, 2.0);
            zg1.GraphPane.CurveList.Add(emgLine);
            zg1.AxisChange();
            zg1.Invalidate();
            zg1.Show();
            metroPanel2.Controls.Add(zg1);
        }

        #region Menu Porta Serial
        private void adicionar_items_nas_combo_box()
        {
            //Portas Seriais
            metroComboBoxSerialPorts.Items.Clear();
            if (SerialPort.GetPortNames().Length > 0)
            {
                foreach (string nome_da_porta_serial in SerialPort.GetPortNames())
                {
                    metroComboBoxSerialPorts.Items.Add(nome_da_porta_serial);
                }
                metroComboBoxSerialPorts.SelectedIndex = 0;
                disable_or_enable_serial_options(true);
                metroToggleSerialConn.Enabled = true;
            }
            else
            {
                metroComboBoxSerialPorts.Items.Add("Não Encontrado");
                metroComboBoxSerialPorts.SelectedIndex = 0;
                disable_or_enable_serial_options(false);
                metroToggleSerialConn.Enabled = false;
            }

            //Baudrates
            metroComboBoxBaudRates.Items.Clear();
            metroComboBoxBaudRates.Items.Add(115200);
            metroComboBoxBaudRates.Items.Add(57600);
            metroComboBoxBaudRates.Items.Add(38400);
            metroComboBoxBaudRates.Items.Add(28800);
            metroComboBoxBaudRates.Items.Add(19200);
            metroComboBoxBaudRates.Items.Add(14400);
            metroComboBoxBaudRates.Items.Add(9600);
            metroComboBoxBaudRates.Items.Add(4800);
            metroComboBoxBaudRates.Items.Add(2400);
            metroComboBoxBaudRates.Items.Add(1200);
            metroComboBoxBaudRates.Items.Add(600);
            metroComboBoxBaudRates.Items.Add(300);
            metroComboBoxBaudRates.SelectedIndex = 0;
        }
        private void disable_or_enable_serial_options(bool enabled)
        {
            metroComboBoxSerialPorts.Enabled = enabled;
            metroComboBoxBaudRates.Enabled = enabled;
        }
        #endregion

        private void metroToggleSerialConn_CheckedChanged(object sender, EventArgs e)
        {
            if (metroToggleSerialConn.Checked)
            {
                if (!minhaporta.IsOpen)
                {
                    minhaporta.PortName = (string)metroComboBoxSerialPorts.SelectedItem;
                    minhaporta.BaudRate = (int)metroComboBoxBaudRates.SelectedItem;
                    try
                    {
                        minhaporta.Open();
                        timerLeitura.Start();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        metroToggleSerialConn.Checked = false;
                    }
                }
            }
            else
            {
                if (minhaporta.IsOpen)
                {
                    try
                    {
                        timerLeitura.Stop();
                        minhaporta.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        metroToggleSerialConn.Checked = true;
                    }
                }
            }
        }

        private void timerLeitura_Tick(object sender, EventArgs e)
        {
            if (minhaporta.BytesToRead > 5)
            {
                string linha_lida = minhaporta.ReadLine();
                double valor_lido = Convert.ToDouble(linha_lida);
                if (metroTabControl1.SelectedTab == metroTabPagePortaSerial)
                {
                    //Console.WriteLine("Updating text");
                    metroTextBoxValoresLidos.Text = linha_lida + "\n" + metroTextBoxValoresLidos.Text;
                }
                else if (metroTabControl1.SelectedTab == metroTabPageGrafico)
                {
                    if (metroToggleGrafico.Checked)
                    {
                        //Console.WriteLine("Updating chart");
                        emgLine.AddPoint(valor_x, valor_lido);
                        valor_x += 50.0 / 1000.0; //50 ms

                        if (emgLine.Points.Count > 20 * 30) //20Hz * 30s
                        {
                            emgLine.Points.RemoveAt(0);
                        }

                        zg1.AxisChange();
                        zg1.Invalidate();
                    }
                }
                else if (metroTabControl1.SelectedTab == metroTabPageMusica)
                {
                    if (metroToggleMusica.Checked)
                    {
                        metroProgressSpinnerSom.Value = (int)(valor_lido * 100);
                        metroLabel42.Text = valor_lido.ToString() + " V";
                        //Se antes era menor e agora é maior
                        if (valor_anterior <= valor_limiar && valor_lido > valor_limiar)
                        {
                            contracao_comecou();
                        }
                        else if (valor_anterior > valor_limiar && valor_lido <= valor_limiar) //Se antes era maior e agora é menor
                        {
                            contracao_terminou();
                        }
                    }
                }
                if(valor_lido != valor_anterior)
                {
                    valor_anterior = valor_lido;
                }
            }
        }

        private void contracao_comecou()
        {
            metroProgressSpinnerSom.Spinning = true;
            metroLabelTocando.Text = "Tocando";
            metroLabel41.UseStyleColors = true;
            metroLabel42.UseStyleColors = true;

            executar_envento(true);
        }

        private void contracao_terminou()
        {
            metroProgressSpinnerSom.Spinning = false;
            metroLabelTocando.Text = "Pausado";
            metroLabel41.UseStyleColors = false;
            metroLabel42.UseStyleColors = false;

            executar_envento(false);
        }

        private void executar_envento(bool contracao)
        {
            if (metroRadioButtonYouTube.Checked)
            {
                SendKeys.Send(" ");
            } else if (metroRadioButtonTeclas.Checked)
            {
                if (contracao)
                {
                    SendKeys.Send(metroTextBox5.Text);
                } else
                {
                    SendKeys.Send(metroTextBox6.Text);
                }
            } else if (metroRadioButtonTeclasEspeciais.Checked)
            {
                if (contracao)
                {
                    SendKeys.Send(metroComboBox3.SelectedText);
                }
                else
                {
                    SendKeys.Send(metroComboBox4.SelectedText);
                }
            } else if (metroRadioButtonSom.Checked)
            {
                if (contracao)
                {
                    soundplayer1.PlayLooping();
                }
                else
                {
                    soundplayer1.Stop();
                }
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            timerLeitura.Stop();
            minhaporta.Close();
        }

        private void metroTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            adicionar_items_nas_combo_box();
            metroTextBoxValoresLidos.Text = "";
            valor_x = 0;
            emgLine.Points.Clear();
        }

        private void metroToggleMusica_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void metroTrackBarLimiar_Scroll(object sender, ScrollEventArgs e)
        {
            valor_limiar = metroTrackBarLimiar.Value / 100.0;
            metroLabelLimiar.Text = valor_limiar.ToString() + " V";
            metroLabel40.Text = valor_limiar.ToString() + " V";
            metroTrackBar2.Value = metroTrackBarLimiar.Value;
            metroProgressSpinner4.Value = 500 - metroTrackBarLimiar.Value;
        }

        private void metroToggleGrafico_CheckedChanged(object sender, EventArgs e)
        {
            metroProgressSpinner4.Visible = metroToggleGrafico.Checked;
        }

        private void metroTrackBar2_Scroll(object sender, ScrollEventArgs e)
        {
            valor_limiar = metroTrackBar2.Value / 100.0;
            metroLabelLimiar.Text = valor_limiar.ToString() + " V";
            metroLabel40.Text = valor_limiar.ToString() + " V";
            metroTrackBarLimiar.Value = metroTrackBar2.Value;
            metroProgressSpinner4.Value = 500 - metroTrackBar2.Value;
        }

        private void metroLabel44_Click(object sender, EventArgs e)
        {
            metroPanel4.Visible = !metroPanel4.Visible;
        }

        private void metroComboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
