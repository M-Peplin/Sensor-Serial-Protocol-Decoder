using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SensorSerialProtocolDecoder.Tools
{
    class DecodeSentence
    {

        byte[] bytes;
        private static UdpClient listener;
        private static IPEndPoint source;

        public void DecodeEnvelopeData(IAsyncResult result, string message)
        {
            listener = (UdpClient)result.AsyncState;
            source = new IPEndPoint(0,0);

            try
            {
                bytes = listener.EndReceive(result, ref source);
                string msg = (Encoding.ASCII.GetString(bytes, 0, bytes.Length));
            }
            catch (ObjectDisposedException)
            {
                MessageBox.Show("Something didn't work");
            }
            
        }
    }
}
