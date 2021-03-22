using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensorSerialProtocolDecoder.Services;

namespace SensorSerialProtocolDecoder.Interfaces
{
    public interface ICOMPortService
    {
        SerialPort createSerialPort(string baudRate, string name, Action<string> portStatus);

        void closeSerialPort(SerialPort serialPort, Action<string> portStatus);

        string checkPortStatus(SerialPort serialPort);        

        void ReadMessage(SerialPort serialPort, Action<string> receivedMessage);
        void ReadMessages(SerialPort serialPort1, SerialPort serialPort2, Action<string> receivedMessage1, Action<string> receivedMessage2);
    }
}
