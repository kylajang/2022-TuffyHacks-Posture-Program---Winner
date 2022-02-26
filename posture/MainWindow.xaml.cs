using System;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace posture
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SerialPort sensorPort = new SerialPort();

        string serialData;

        public MainWindow()
        {
            InitializeComponent();

            try
            {
                sensorPort.PortName = "COM4";
                sensorPort.BaudRate = 115200;
                sensorPort.Parity = Parity.None;
                sensorPort.StopBits = StopBits.One;
                sensorPort.Handshake = Handshake.None;

                sensorPort.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            sensorPort.DataReceived += new SerialDataReceivedEventHandler(sensorPort_DataReceived);
        }

        private void sensorPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                serialData = sensorPort.ReadLine();
                Console.WriteLine(serialData);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
