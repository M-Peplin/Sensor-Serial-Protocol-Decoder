using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensorSerialProtocolDecoder.Services;

namespace SensorSerialProtocolDecoder.Model
{
    public class SensorModel
    {

        new SendStatusService sensorStatusSend = new SendStatusService();
        new DecodeService decodeSentence = new DecodeService();
        new COMPortService comPortSercice = new COMPortService();      


        public void test()
        {
          //  int z = status.testowanie();
        }

    }   
}
