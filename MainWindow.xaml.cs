using System;
using System.IO.Ports;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace WPF_SerialCommunication
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        SerialPort serialPort;
        string[] serialPorts;

        public MainWindow()
        {
            InitializeComponent();
            InitializeSerialPorts();
        }

        private void InitializeSerialPorts()
        {
            serialPorts = SerialPort.GetPortNames();
            if (serialPorts.Count() != 0)
            {
                foreach (string serial in serialPorts)
                {
                    var serialItems = SerialPortNamesCmbBox.Items;
                    if (!serialItems.Contains(serial)) // if the serial is not yet inside the comboBox
                    {
                        SerialPortNamesCmbBox.Items.Add(serial); // add a serial port name to combo box
                    }
                }
            }
        }

        #region WPF to Arduino connection
        bool isConnectedToArduino = false;

        public object LblSliderBrightness { get; private set; }

        private void ConnectToArduino()
        {
            try
            {
                string selectedSerialPort = SerialPortNamesCmbBox.SelectedItem.ToString(); // gets the selected port
                serialPort = new SerialPort(selectedSerialPort, 9600);
                serialPort.Open();
                SerialPortConnectBtn.Content = "Disconnect";
                LedControlPanel.IsEnabled = true;
                isConnectedToArduino = true;
            } 
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("The selected serial port is busy!", "Busy", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("There is no serial port!", "Empty Serial Port", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void DisconnectFromArduino()
        {
            SerialPortConnectBtn.Content = "Connect";
            LedControlPanel.IsEnabled = false;
            isConnectedToArduino = false;
            ResetControl();
            serialPort.Close();
        }

        private void ResetControl()
        {
            LedOffBtn.Background = Brushes.DarkGray;
            LedOnBtn.Background = Brushes.DarkGray;
            SliderBrightness.Value = 0;
            LblSliderBrightness.Content = "0";

        }
        private void SerialPortConnectBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!isConnectedToArduino)
            {
                ConnectToArduino();
            }
            else
            {
                DisconnectFromArduino();
            }
        }
        #endregion


        private void LedOnBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RefreshBtn_Click(object sender, RoutedEventArgs e)
        {
            InitializeSerialPorts();
        }

        
    }
}
