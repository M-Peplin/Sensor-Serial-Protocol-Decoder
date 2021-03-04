﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorSerialProtocolDecoder.Tools
{
    public class SensorStatusSend
    {
        public string sendMessageOn()
        {
            string sendMessageON = "$PAMTC,EEC,ON";
            return sendMessageON;
        }

        public string sendMessageOff()
        {
            string sendMessageOff = "$PAMTC,EEC,OFF";
            return sendMessageOff;
        }

        public string sendMessageRangeDefaultAndStart()
        {
            string sendMessageFullRange = "$PAMTC,EEC,FULL";
            return sendMessageFullRange;
        }

        public string sendMessageDisplaySettings()
        {
            string displaySettings = "$PAMTC,EEC,Q";
            return displaySettings;
        }        
    }
}
