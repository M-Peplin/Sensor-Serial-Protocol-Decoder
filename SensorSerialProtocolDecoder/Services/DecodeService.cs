using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using SensorSerialProtocolDecoder.Interfaces;

namespace SensorSerialProtocolDecoder.Services
{
    class DecodeService : IDecodeService
    {
        private readonly ICOMPortService _comPortService;
        public DecodeService(ICOMPortService comPortService)
        {
            this._comPortService = comPortService;
        }

        byte[] bytes;
        private static UdpClient listener;
        private static IPEndPoint source;

        public void decodeEnvelopeData(IAsyncResult result, string message)
        {
            listener = (UdpClient)result.AsyncState;
            source = new IPEndPoint(0,0);

            try
            {
                bytes = listener.EndReceive(result, ref source);
                string msg = (Encoding.ASCII.GetString(bytes, 0, bytes.Length));
            }
            catch (ObjectDisposedException)
            {
                MessageBox.Show("Something didn't work");
            }            
        }

        public void saveDataToFile(string data)
        {            
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog.RestoreDirectory = true;

            {
                if(saveFileDialog.ShowDialog() == true)
                {
                    File.WriteAllText(saveFileDialog.FileName, data);
                }
            }
        }

        public void recordDataToFile(string data)
        {            
            string path = @"D:\Sonda\TestData";             
            File.AppendAllText(path, data);
        }

        public string showMessage(int messageMode, string portMessage1, string portMessage2)
        {
            //message modes 1-3;
            if(messageMode == 1)
            {
                return portMessage1;
            }
            else if(messageMode == 2)
            {
                return portMessage2;
            }
            else if(messageMode == 3)
            {
                return combineMessages(portMessage1, portMessage2);
            }
            else
            {
                return "Error - please enter valid message mode (1-3)";
            }
        }

        public void showMessages(int messageMode, string portMessage1, string portMessage2, Action<string> portMessageReturned,
            Action<SerialPort, SerialPort, Action<string>, Action<string>> readingData)
        {
            //message modes 1-3;
            if (messageMode == 1)
            {
                portMessageReturned(portMessage1);                
            }
            else if (messageMode == 2)
            {
                portMessageReturned(portMessage2);
            }
            else if (messageMode == 3)
            {
                portMessageReturned(combineMessages(portMessage1, portMessage2));
            }            
        }

        public string combineMessages(string message1, string message2)
        {
            string lastLineMessage1, lastLineMessage2;
            //lastLineMessage1 = message1.Split('\n').Last();
            //lastLineMessage2 = message2.Split('\n').Last();
            string combinedMessage = "";
            //return combinedMessage = lastLineMessage1 + "\n" + lastLineMessage2;
            return message1 + message2;
        }

        public void LoadFileToDecode()
        {
            string path = $@"Data\Data1.txt";
            if(File.Exists(path))
            {
                string data = File.ReadAllText(path);
            }
            else
            {
                MessageBox.Show("No files found!");
            }
            
        }

    }
}
