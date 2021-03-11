﻿using System;
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

            portStatus("Open");
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
                       
    }
}
