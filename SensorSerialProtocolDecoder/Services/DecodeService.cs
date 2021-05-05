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

        public void Decode2()
        {
            int counter = 1;
            string directoryPath = $@"Data";
            string dataToAdd = "";
            string data = "";

            for (int file = 0; file < Directory.GetFiles(directoryPath).Length; file++)
            {
                LoadFileToDecode($@"Data\Data{counter.ToString()}.txt", ref data);
                List<string> splittedData = new List<string>();
                int j = 0;
                for (int i = 0; i < data.Length; i++)
                {
                    dataToAdd = "";
                    while (!data[i].Equals(',') && (i < data.Length - 1))
                    {
                        dataToAdd = dataToAdd + $"{ data[i] }";
                        i++;
                    }
                    splittedData.Add(dataToAdd);

                    if (data[i].Equals(','))
                    {
                   //     j++;
                    }
                }
                string JoinedSplittedData = "";
                ToDecimal(splittedData, ref JoinedSplittedData);
                //string JoinedSplittedData = string.Join("", splittedData);
                SaveDecodedDataToFile(JoinedSplittedData, $@"Data\Data{counter.ToString()}.txt");
            }
        }

        public void ToDecimal(List<string> data, ref string combinedData)
        {
            bool readSentence = true;
            string timeStamp = "";
            bool isBufforWritten = true;
            int? buffor = null;
            int i = 0;
            

            while (readSentence == true)
            {
                try
                {
                    if (data.ElementAt(i).Contains("TS"))
                    {                        
                        timeStamp = data.ElementAt(i + 1);
                        data[i] = data.ElementAt(i);
                        i++;
                    }
                    else if((i == 1) || (i == 2) || (i == 3) || (i == 6))
                    {
                        data[i] = data.ElementAt(i);
                        i++;
                    }                    
                    else if (data.ElementAt(i).Contains(timeStamp))
                    {
                        data[i] = data.ElementAt(i);
                        i++;
                        data[i] = data.ElementAt(i);
                        readSentence = false;
                    }
                    else
                    {                        
                        data[i] = int.Parse(data.ElementAt(i), System.Globalization.NumberStyles.HexNumber).ToString();
                        i++;
                        //buffor = int.Parse(data.ElementAt(i), System.Globalization.NumberStyles.HexNumber);
                        //data[i] = buffor.ToString();

                    }
                }
                catch
                {
                    data[i] = data.ElementAt(i);
                    i++;
                }
            }
            /*
            try
            {
                //buffor = Convert.ToInt32(data.ElementAt(i), 16);                    
                if (data.ElementAt(i).Contains("TS"))
                {
                    //data[i] = $"\n{ data.ElementAt(i) }";                        
                    data[i] = data.ElementAt(i);                        
                }
                else if (data.ElementAt(i).Contains("ES"))
                {
                    data[i] = data.ElementAt(i);
                    i++;
                    data[i] = data.ElementAt(i);
                }
                else
                {
                    buffor = int.Parse(data.ElementAt(i), System.Globalization.NumberStyles.HexNumber);
                    //buffor = int.Parse(data.ElementAt(i));
                    isBufforWritten = true;
                }
            }
            catch
            {

            }

            if (buffor != null && isBufforWritten == true)
            {
                data[i] = buffor.ToString();
            }                
           // isBufforWritten = false;
            */

            combinedData = CombinedString(data);            
        }

        public string CombinedString(List<string> data)
        {
            string combinedData = "";
            combinedData = string.Join(",", data);
            return combinedData;
        }

        public void SaveDecodedDataToFile(string buffor, string InitialPath)
        {
            _comPortService.CheckDirectoryExists($@"Decoded\Data");
            string pathDecoded = $@"Decoded\{InitialPath}";
            using (StreamWriter sw = File.AppendText(pathDecoded))
            {                
                sw.Write(buffor);
            }
        }

    }
}
