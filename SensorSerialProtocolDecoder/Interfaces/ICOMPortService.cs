using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensorSerialProtocolDecoder.Services;

namespace SensorSerialProtocolDecoder.Services
{
    public interface ICOMPortService
    {
        SerialPort createSerialPort(string baudRate, string name);        
    }
}
