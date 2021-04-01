using System;
using System.Collections.Generic;
using System.IO;
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
        public delegate void OnDataRead();
        public event OnDataRead dataRead;
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
                return ("Port closed");
            }
            else if (serialPort.IsOpen)
            {
                return (serialPort.PortName + " open");
            }
            else
            {
                return (serialPort.PortName + " closed");
            }
        }

        public void closeSerialPort(SerialPort serialPort, Action<string> portStatus)
        {
            try
            {
                if (serialPort != null && serialPort.IsOpen)
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
                else
                {
                    MessageBox.Show("Serial port is not opened!");
                }
            }
            catch (NullReferenceException e)
            {                
                MessageBox.Show(e.Message.ToString());
            }
            finally
            {
                serialPort?.Dispose();
            }
            
            string status = checkPortStatus(serialPort);
            portStatus(status);            
        }
        

        static string dataIN, dataINport1, dataINport2;        
        public void ReadMessage(SerialPort serialPort, Action<string> receivedMessage)
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

        public void ReadMessages(SerialPort serialPort1, SerialPort serialPort2, Action<string> receivedMessage1, Action<string> receivedMessage2)
        {
            if(serialPort1 != null)
                serialPort1.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler1);
            if (serialPort2 != null)
                serialPort2.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler2);            

            void DataReceivedHandler1(object sender, SerialDataReceivedEventArgs e)
            {
                SerialPort sp = (SerialPort)sender;
                string buffor = sp.ReadExisting();
                //dataIN += "\n " + buffor;
                dataINport1 += buffor;
                receivedMessage1(dataINport1);
            }

            void DataReceivedHandler2(object sender, SerialDataReceivedEventArgs e)
            {
                SerialPort sp = (SerialPort)sender;
                string buffor = sp.ReadExisting();
                //dataIN += "\n " + buffor;
                dataINport2 += buffor;
                receivedMessage2(dataINport2);
            }            
        }

        public delegate void CombinedMessageEventHandler();
        public event CombinedMessageEventHandler OnCombineMessage;
        string combinedMessageString;

        //temporary - deadline
        string path = @"D:\Sonda\TestData.txt";
        //DateTime dt = new DateTime();
        int counter = 0, counterModulo = 0;
        string pathAdditive;
        

        public void ReadCombinedMessage(SerialPort serialPort1, SerialPort serialPort2, 
            Action<string> receivedMessage1, Action<string> receivedMessage2, Action<string> combinedMessage, bool recording=true)
        {
            if (serialPort1 != null)
                serialPort1.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler1);
            if (serialPort2 != null)
                serialPort2.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler2);

            //temporary file writing - deadline - this has to be moved somewhere else and improved

            //string indata = "";
            CheckDirectoryExists($@"Data");
            pathAdditive = CheckFileExists(pathAdditive);

            void DataReceivedHandler1(object sender, SerialDataReceivedEventArgs e)
            {
                pathAdditive = $@"Data\Data{counterModulo.ToString()}.txt";                
                SerialPort sp = (SerialPort)sender;
                string buffor = sp.ReadExisting();                
                dataINport1 += buffor;
                dataINport1 = CutStringIfTooLong(dataINport1, buffor);
                combinedMessageString = buffor;
                receivedMessage1(dataINport1);

                combinedMessage(combinedMessageString);

                if (recording == true)
                {
                    recordStringToFile(buffor);
                }
            }

            void DataReceivedHandler2(object sender, SerialDataReceivedEventArgs e)
            {
                SerialPort sp = (SerialPort)sender;
                string buffor = sp.ReadExisting();
                //dataIN += "\n " + buffor;
                dataINport2 += buffor;
                combinedMessageString += buffor;
                receivedMessage2(dataINport2);
                combinedMessage(combinedMessageString);
            }
            //receivedMessage(serialPort.ReadExisting());
        }

        public string CheckFileExists(string path)
        {
            while(File.Exists(path))
            {
                counterModulo += 1;
                path = $@"Data\Data{counterModulo.ToString()}.txt";
            }

            return path;
        }

        public void CheckDirectoryExists(string path)
        {
            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public string addTimeToString(string str)
        {            
            str = $" *** {Convert.ToString(DateTime.Now)} *** \n{str}";
            return str;
        }

        public void recordStringToFile(string buffor)
        {
            using (StreamWriter sw = File.AppendText(pathAdditive))
            {
                if (buffor.Contains("TS"))
                {
                    buffor = addTimeToString(buffor);
                    counter += 1;
                    if (counter % 150 == 0)
                    {
                        counterModulo = counterModulo += 1;
                    }
                }
                sw.Write(buffor);
            }
        }

        public string CutStringIfTooLong(string str, string buffor)
        {
            if (str.Length > 15000)
            {
                str.Remove(0, buffor.Length);
            }
            return str;
        }
    }
}
