﻿using System;
using System.Collections.Generic;
using System.IO;
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
    }
}
