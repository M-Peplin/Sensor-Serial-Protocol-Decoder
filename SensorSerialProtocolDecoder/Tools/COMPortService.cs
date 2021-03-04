using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorSerialProtocolDecoder.Tools
{
    public class COMPortService
    {
        
        SerialPort createSerialPort(int baudRate)
        {
            SerialPort mySerialPort = new SerialPort("COM1");
            //mySerialPort.BaudRate = baudRate;
            mySerialPort.BaudRate = 115200;

            return mySerialPort;
        }
       
    }
}
