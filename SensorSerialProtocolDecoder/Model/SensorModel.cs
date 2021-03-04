using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensorSerialProtocolDecoder.Tools;

namespace SensorSerialProtocolDecoder.Model
{
    public class SensorModel
    {

        new SensorStatusSend sensorStatusSend = new SensorStatusSend();
        new DecodeSentence decodeSentence = new DecodeSentence();
        new COMPortService comPortSercice = new COMPortService();

        public void test()
        {
          //  int z = status.testowanie();
        }

    }   
}
