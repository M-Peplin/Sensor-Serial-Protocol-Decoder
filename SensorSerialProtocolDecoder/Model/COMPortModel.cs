using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorSerialProtocolDecoder.Tools
{
    public class COMPortModel
    {
        public COMPortService portService = new COMPortService();
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


        private string portName;

        public string PortName
        {
            get
            {
                return portName;
            }
            set
            {
                portName = value;
            }
        }
    }
}
