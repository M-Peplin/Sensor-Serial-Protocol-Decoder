using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorSerialProtocolDecoder.Tools
{
    public class COMPortModel
    {
        private int portId;

        public int PortId
        {
            get
            {
                return portId;
            }
            set
            {
                portId = value;
            }
        }


        private string portBaudRate;

        public string PortBaudRate
        {
            get
            {
                return portBaudRate;
            }
            set
            {
                portBaudRate = value;
            }
        }
    }
}
