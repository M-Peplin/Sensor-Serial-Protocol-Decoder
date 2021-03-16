using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SensorSerialProtocolDecoder.Interfaces;

namespace SensorSerialProtocolDecoder.Services
{
    public class COMPortService : ICOMPortService
    {    

        public SerialPort createSerialPort(string baudRate, string name, Action<string> portStatus)
        {
            SerialPort serialPort = new SerialPort(name);
            int baud = 0;           
            try
            {
                baud = Int32.Parse(baudRate);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            serialPort.BaudRate = baud;
            serialPort.Parity = Parity.None;
            serialPort.DataBits = 8;
            serialPort.StopBits = StopBits.One;
            serialPort.Handshake = Handshake.None;
            serialPort.RtsEnable = true;
            serialPort.ReadTimeout = 1000;

            //serialPort.BaudRate = 9600
            //mySerialPort.BaudRate = 115200;

            try
            {
                serialPort.Open();
            }
            catch (Exception)
            {

                MessageBox.Show("Serial port unavalible.");
            }

            //portStatus = checkPortStatus(serialPort);

            //portStatus = checkPortStatus;
            //portStatus(serialPort); 
            string status = checkPortStatus(serialPort);

            portStatus(status);
            return serialPort;
        }    
        
        public string checkPortStatus(SerialPort serialPort)
        {
            if(serialPort == null)
            {
                return "closed";
            }
            else if (serialPort.IsOpen)
            {
                return "open";
            }
            else
            {
                return "closed";
            }
        }

        public void closeSerialPort(SerialPort serialPort, Action<string> portStatus)
        {
            if(serialPort.IsOpen)
            {
                try
                {
                    serialPort.Close();
                }
                catch
                {
                    MessageBox.Show("Error");
                }
            }
            
            string status = checkPortStatus(serialPort);
            portStatus(status);            
        }

        public void testSendMessage(SerialPort serialPort)
        {
            if(serialPort.IsOpen)
            {
                try
                {
                    //serialPort.WriteLine("Testowy message");
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        static string dataIN;        
        public void testReadMessage(SerialPort serialPort, Action<string> receivedMessage)
        {     
            serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            //string indata = "";

            void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
            {
                SerialPort sp = (SerialPort)sender;
                string buffor = sp.ReadExisting();
                //dataIN += "\n " + buffor;
                dataIN += buffor;
                receivedMessage(dataIN);
            }
            //receivedMessage(serialPort.ReadExisting());
        }        
        
       
                       
    }
}
