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

        public void LoadFileToDecode(string path, ref string data)
        {            
            if(File.Exists(path))
            {
                 data = File.ReadAllText(path);
            }
        }

        public void LoadAllDataFiles()
        {
            //To be finished
            int counter = 1;
            string path = $@"Data\Data{counter.ToString()}.txt";
            string data = "";
            while (File.Exists(path))
            {
                LoadFileToDecode(path, ref data);
                Decode(data);
                counter++;
                path = $@"Data\Data{counter.ToString()}.txt";
            }
        }
        string dane = "TS,   3008,    0,1,03,02,150, b0,05,22,31,15,38,11,3c,11,41,11,44,OFF0, 00,ff,ec,ae,a7,a2,b0,92,69,71,71,85,5f,57,6c,64,53,3f,60,5d,56,60,47,40,3d,4a,42,4b,3e,30,3c,1b,43,2a,25,2c,17,1e,25,24,10,23,34,1a,0e,17,0e,0f,15,0a,22,19,0f,1a,0b,0b,06,15,04,03,03,11,06,00,0d,00,11,03,03,11,05,07,05,01,05,03,02,0e,00,02,02,01,00,0a,05,01,02,02,01,02,00,05,00,01,02,09,01,0b,03,00,03,01,00,02,03,00,01,01,00,09,00,01,05,01,00,00,00,02,00,02,03,08,00,05,06,00,01,00,04,06,00,00,02,00,08,07,01,06,04,00,00,08,09,00,00,05,00,05,00,00,01,03,01,01,03,03,00,01,00,01,00,00,00,0a,08,00,00,01,00,06,03,00,01,01,01,01,03,00,00,03,04,00,02,00,0d,03,00,07,00,02,00,02,00,01,00,00,00,00,00,03,09,03,00,00,01,01,00,00,00,01,01,00,03,0d,00,00,00,01,00,01,00,01,01,03,01,01,04,01,01,02,05,03,00,03,00,00,07,01,0c,01,02,00,00,00,04,04,07,00,00,09,07,00,08,07,00,00,02,01,00,05,00,08,03,00,05,02,00,03,04,05,0d,09,00,00,03,01,02,03,01,00,02,00,00,02,00,03,00,00,01,01,00,04,03,00,01,00,03,01,05,02,01,09,04,01,00,00,10,00,01,05,00,00,00,00,01,00,01,00,00,00,01,00,0b,04,02,06,01,00,00,00,02,07,0b,04,01,00,08,03,00,01,02,02,02,00,00,02,00,01,00,00,00,00,00,00,00,05,00,00,02,00,00,00,00,01,00,00,01,01,01,07,04,02,00,04,00,01,00,06,00,01,01,05,00,00,03,07,06,00,01,01,01,03,00,01,00,00,05,06,00,02,00,00,04,01,01,00,03,00,03,05,04,09,00,03,01,06,02,00,04,01,04,01,03,00,01,01,00,00,01,00,06,01,00,00,02,00,00,00,03,02,05,00,00,00,00,00,06,00,02,0b,06,00,01,00,03,00,00,02,00,00,01,05,00,02,0a,00,00,04,00,03,00,04,05,02,04,01,00,00,00,06,06,03,00,03,02,00,00,01,04,00,06,00,00,01,00,00,01,0b,01,01,00,01,00,0a,00,00,00,00,01,03,02,00,04,03,01,00,01,00,04,07,00,00,00,01,00,07,01,02,00,01,00,04,00,00,00,04,01,03,03,01,04,02,01,00,00,02,02,00,05,04,00,01,00,00,08,00,00,02,05,02,07,02,01,01,01,0c,05,01,08,00,05,00,01,00,04,00,03,00,02,00,00,02,02,00,00,00,00,02,0f,00,01,00,02,00,01,00,00,00,00,00,02,02,04,0c,02,02,01,04,03,06,00,0a,03,07,05,00,00,00,07,01,03,00,01,00,01,02,00,00,05,09,00,02,02,02,00,00,01,06,00,03,01,00,00,01,00,02,00,00,04,00,03,06,02,00,00,01,00,00,00,03,05,02,04,02,01,05,0f,01,00,0f,02,00,02,00,00,00,01,00,02,00,00,01,03,00,00,00,01,05,00,00,00,00,00,01,01,02,00,00,07,02,02,00,01,00,02,02,01,02,03,02,0b,00,01,00,02,00,00,04,08,02,01,01,00,00,01,01,01,00,0a,0b,01,0b,0a,01,00,00,06,00,00,04,00,00,00,01,00,00,01,00,02,00,01,04,02,00,01,00,02,00,00,06,00,03,00,00,01,00,00,03,00,00,00,04,00,10,00,00,05,00,00,04,00,00,00,01,00,01,05,00,00,08,00,02,00,01,04,00,01,01,00,01,01,00,01,00,02,05,07,08,00,08,05,00,02,06,04,01,02,01,03,03,01,03,02,01,11,03,01,00,00,00,00,01,00,03,02,00,00,00,05,02,00,01,03,00,01,0d,06,00,05,02,01,00,03,01,01,07,03,00,00,00,04,03,01,00,0a,00,00,03,01,00,04,00,02,02,00,02,00,02,03,00,00,05,00,00,02,00,01,01,0d,05,00,04,02,08,04,04,00,01,00,05,ES,   3008";
        public void Decode(string data)
        {
            int index = 0;
            string[] splittedData = new string[2800];            
            int j = 0;
            for(int i = 0; i< data.Length; i++)
            {
                while(!data[i].Equals(',') && (i < data.Length - 1) )
                {                    
                    splittedData[j] = $"{ splittedData[j] }{ data[i] }";
                    i++;
                }
                if(data[i].Equals(','))
                {
                    j++;
                }
            }
        }

        public void Decode2(string data)
        {
            int index = 0;
            string dataToAdd = "";
            List<string> splittedData = new List<string>();
            int j = 0;
            for(int i = 0; i < data.Length; i++)
            {
                dataToAdd = "";
                while(!data[i].Equals(',') && (i < data.Length - 1))
                {
                    dataToAdd = dataToAdd + $"{ data[i] }";                    
                    i++;
                }
                splittedData.Add(dataToAdd);

                if (data[i].Equals(','))
                {                    
                    j++;
                }
            }
            ToDecimal(splittedData);
        }

        public void ToDecimal(List<string> data)
        {
            int? buffor = null;
            for(int i = 0; i < data.Count; i++)
            {
                if(i > 3 && i != 7)
                {
                    try
                    {
                        //buffor = Convert.ToInt32(data.ElementAt(i), 16);
                        buffor = int.Parse(data.ElementAt(i), System.Globalization.NumberStyles.HexNumber);
                    }
                    catch
                    {

                    }
                }                

                if(buffor != null)
                {
                    data[i] = buffor.ToString();
                }                
            }
            string combinedData = CombinedString(data);
        }

        public string CombinedString(List<string> data)
        {
            string combinedData = "";
            combinedData = string.Join(",", data);
            return combinedData;
        }

    }
}
