using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorSerialProtocolDecoder.Interfaces
{
    public interface IDecodeService
    {
        void decodeEnvelopeData(IAsyncResult result, string message);

        void saveDataToFile(string data);
    }
}
