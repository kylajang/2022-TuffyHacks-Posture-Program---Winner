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
        // SerialPort object for collecting sensor data
        SerialPort sensorPort = new SerialPort();

        // Timer for measuring how long the user has been sitting down
        DispatcherTimer sitDownTimer = new DispatcherTimer();

        // Sensor data variables
        string serialData;
        bool readingSerialData = false;

        // Sitting down timer variables
        int timeSatDown;
        int maxSitDownTime = 3600;

        public int RightThigh { get; set; }
        public int LeftThigh { get; set; }
        public int RightButt { get; set; }
        public int LeftButt { get; set; }
        // Parsing serial data into 4 integer variables
        public void SerialData(string[] parsedInfo)
        {
            try
            {
                LeftThigh = Convert.ToInt32(parsedInfo[0]);
                RightThigh = Convert.ToInt32(parsedInfo[1]);
                LeftButt = Convert.ToInt32(parsedInfo[2]);
                RightButt = Convert.ToInt32(parsedInfo[3]);
            }
            catch
            {

            }
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
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            sensorPort.DataReceived += new SerialDataReceivedEventHandler(sensorPort_DataReceived);

            // Initialize serial port received data handler
            sensorPort.DataReceived += new SerialDataReceivedEventHandler(sensorPort_DataReceived);

            // Initialize sitting down timer
            // Call SitDownTimer_Tick every second
            sitDownTimer.Tick += SitDownTimer_Tick;
            sitDownTimer.Interval = new TimeSpan(0, 0, 1);
        }

        // Function to update the total time the user has been sitting down
        private void SitDownTimer_Tick(object sender, EventArgs e)
        {
            timeSatDown++;
            // If the user has been sitting down for one hour, show a break reminder message
            if (timeSatDown > maxSitDownTime)
            {
                while (MessageBox.Show("You've been sitting down for one hour! Please take a few minutes to walk and stretch.", "Break Time!", MessageBoxButton.OK, MessageBoxImage.Information) != MessageBoxResult.OK)
                {

                }
                // Reset the total time sat down 
                timeSatDown = 0;
            }
        }

        private void sensorPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                serialData = sensorPort.ReadLine();
                Console.WriteLine(serialData);

                string[] serialDataArray = serialData.Split(',');

                SerialData(serialDataArray);

                this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                {
                    //connected to label and tells user to lean left, right, or forward based off right thigh input
                    if (RightButt > 999 && LeftButt > 999)
                    {
                        rightthigh.Content = "Right Thigh Output: " + RightThigh.ToString();
                        userdirection.Content = "\nlean forward";
                    }
                    else if (RightThigh < 945)
                    {
                        rightthigh.Content = "Right Thigh Output: " + RightThigh.ToString();
                        userdirection.Content = "\nlean to the right";
                    }
                    else if (RightThigh > 955)
                    {
                        rightthigh.Content = "Right Thigh Output: " + RightThigh.ToString();
                        userdirection.Content = "\nlean to the left";
                    }
                    else
                    {
                        rightthigh.Content = "Right Thigh Output: " + RightThigh.ToString();
                        userdirection.Content = "\ngood posture";
                    }
                    //connected to label and tells user to lean left, right, or forward based off left thigh input
                    if (RightButt > 999 && LeftButt > 999)
                    {
                        leftthigh.Content = "Left Thigh Output: " + LeftThigh.ToString();
                        userdirection.Content = "\nlean forward";
                    }
                    else if (LeftThigh < 945)
                    {
                        leftthigh.Content = "Left Thigh Output: " + LeftThigh.ToString();
                        userdirection.Content = "\nlean to the left";
                    }
                    else if (LeftThigh > 955)
                    {
                        leftthigh.Content = "Left Thigh Output: " + LeftThigh.ToString();
                        userdirection.Content = "\nlean to the right";
                    }
                    else
                    {
                        leftthigh.Content = "Left Thigh Output: " + LeftThigh.ToString();
                        userdirection.Content = "\ngood posture";
                    }
                    //connected to label and tells user to lean left, right, or back based off right butt input
                    if (RightButt < 995 && LeftButt < 995)
                    {
                        rightbutt.Content = "Right Butt Output: " + RightButt.ToString();
                        userdirection.Content = "\nlean back";
                    }
                    else if (RightButt < 995)
                    {
                        rightbutt.Content = "Right Butt Output: " + RightButt.ToString();
                        userdirection.Content = "\nlean to the right";
                    }
                    else if (RightButt > 1005)
                    {
                        rightbutt.Content = "Right Butt Output: " + RightButt.ToString();
                        userdirection.Content = "\nlean to the left";
                    }
                    else
                    {
                        rightbutt.Content = "Right Butt Output: " + RightButt.ToString();
                        userdirection.Content = "\ngood posture";
                    }
                    //connected to labels and tells user to lean left, right, or back based off left butt input
                    if (RightButt < 995 && LeftButt < 995)
                    {
                        leftbutt.Content = "Left Butt Output: " + LeftButt.ToString();
                        userdirection.Content = "\nlean back";
                    }
                    else if (LeftButt < 995)
                    {
                        leftbutt.Content = "Left Butt Output: " + LeftButt.ToString();
                        userdirection.Content = "\nlean to the left";
                    }
                    else if (LeftButt > 1005)
                    {
                        leftbutt.Content = "Left Butt Output: " + LeftButt.ToString();
                        userdirection.Content = "\nlean to the right";
                    }
                    else
                    {
                        leftbutt.Content = "Left Butt Output: " + LeftButt.ToString();
                        userdirection.Content = "\ngood posture";
                    }

                }));
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (((bool)goodposture.IsChecked && !((bool)morethanthree.IsChecked)))
            {
                string file = @"C:\Users\timkh\source\repos\posture\SurveyInput.txt";
                string text = "This user believes they have good posture and they sit for less than 3 hours";
                File.WriteAllText(file, text);
                Console.WriteLine(File.ReadAllText(file));
                Console.WriteLine();
            }
            else if (((bool)goodposture.IsChecked && !((bool)lessthanthree.IsChecked)))
            {
                string file = @"C:\Users\timkh\source\repos\posture\SurveyInput.txt";
                string text = "This user believes they have good posture and they sit for more than 3 hours.";
                File.WriteAllText(file, text);
                Console.WriteLine(File.ReadAllText(file));
                Console.WriteLine();
            }
            else if (((bool)badposture.IsChecked && !((bool)lessthanthree.IsChecked)))
            {
                string file = @"C:\Users\timkh\source\repos\posture\SurveyInput.txt";
                string text = "This user believes they have bad posture and they sit for less than 3 hours.";
                File.WriteAllText(file, text);
                Console.WriteLine(File.ReadAllText(file));
                Console.WriteLine();
            }
            else if (((bool)badposture.IsChecked && !((bool)morethanthree.IsChecked)))
            {
                string file = @"C:\Users\tkheang\source\repos\posture\SurveyInput.txt";
                string text = "This user believes they have bad posture and they sit for more than 3 hours.";
                File.WriteAllText(file, text);
                Console.WriteLine(File.ReadAllText(file));
                Console.WriteLine();
            }

            if (readingSerialData == false && sensorPort.IsOpen == false)
            {
                readingSerialData = true;
                startStopButton.Content = "Stop";
                sensorPort.Open();
                sitDownTimer.Start();
            }
            else
            {
                readingSerialData = false;
                startStopButton.Content = "Start";
                sensorPort.Close();
                sitDownTimer.Stop();
                timeSatDown = 0;
            }
        }
    }
}