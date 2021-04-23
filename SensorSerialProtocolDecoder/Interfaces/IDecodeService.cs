using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorSerialProtocolDecoder.Interfaces
{
    public interface IDecodeService
    {
        void decodeEnvelopeData(IAsyncResult result, string message);

        void saveDataToFile(string data);

        void recordDataToFile(string data);

        string showMessage(int messageMode, string portMessage1, string portMessage2);

        void showMessages(int messageMode, string portMessage1, string portMessage2, Action<string> portMessageReturned,
            Action<SerialPort, SerialPort, Action<string>, Action<string>> readingData);

        void LoadFileToDecode(string path, ref string data);

        void LoadAllDataFiles();

        void Decode(string data);
    }
}
