using System;
using System.IO.Ports;
using System.Windows;
using System.Windows.Threading;

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

        public MainWindow()
        {
            InitializeComponent();

            // Initialize serial port for reading sensor data
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
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

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

        // Serial port received data event handler
        private void sensorPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                // Read sensor data one line at a time and output to the console and GUI
                serialData = sensorPort.ReadLine();
                Console.WriteLine(serialData);

                this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                {
                    sensorOutput.Content = "Sensor Data: " + serialData;
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Start/Stop button click event handler
        private void startStopButton_Click(object sender, RoutedEventArgs e)
        {
            // If "start" button is clicked, start reading serial data and start the sit down timer
            if (readingSerialData == false && sensorPort.IsOpen == false)
            {
                readingSerialData = true;
                startStopButton.Content = "Stop";
                sensorPort.Open();
                sitDownTimer.Start();
            }
            // If "stop" button is clicked, stop reading serial data and stop the sit down timer
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
