using System;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace posture
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SerialPort sensorPort = new SerialPort();

        string serialData;
        public int RightThigh { get; set; }
        public int LeftThigh { get; set; }
        public int RightButt { get; set; }
        public int LeftButt { get; set; }
        // Parsing serial data into 4 integer variables
        public void SerialData(string[] parsedInfo)
        {
            RightThigh = Convert.ToInt32(parsedInfo[0]);
            LeftThigh = Convert.ToInt32(parsedInfo[1]);
            RightButt = Convert.ToInt32(parsedInfo[2]);
            LeftButt = Convert.ToInt32(parsedInfo[3]);
        }

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

                this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                {
                    

                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (((bool)goodposture.IsChecked && !((bool)morethanthree.IsChecked)))
            {
                string file = @"C:\Users\ky\source\repos\posture\SurveyInput.txt";
                string text = "This user believes they have good posture and they sit for less than 3 hours";
                File.WriteAllText(file, text);
                Console.WriteLine(File.ReadAllText(file));
                Console.WriteLine();
            }
            else if (((bool)goodposture.IsChecked && !((bool)lessthanthree.IsChecked)))
            {
                string file = @"C:\Users\ky\source\repos\posture\SurveyInput.txt";
                string text = "This user believes they have good posture and they sit for more than 3 hours.";
                File.WriteAllText(file, text);
                Console.WriteLine(File.ReadAllText(file));
                Console.WriteLine();
            }
            else if (((bool)badposture.IsChecked && !((bool)lessthanthree.IsChecked)))
            {
                string file = @"C:\Users\ky\source\repos\posture\SurveyInput.txt";
                string text = "This user believes they have bad posture and they sit for less than 3 hours.";
                File.WriteAllText(file, text);
                Console.WriteLine(File.ReadAllText(file));
                Console.WriteLine();
            }
            else if (((bool)badposture.IsChecked && !((bool)morethanthree.IsChecked)))
            {
                string file = @"C:\Users\ky\source\repos\posture\SurveyInput.txt";
                string text = "This user believes they have bad posture and they sit for more than 3 hours.";
                File.WriteAllText(file, text);
                Console.WriteLine(File.ReadAllText(file));
                Console.WriteLine();
            }
        }
    }
}