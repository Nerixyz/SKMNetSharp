﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Client
{
    public class MacroEvent : Event
    {
        private readonly byte macroNoMSB;
        private readonly byte macroNoLSB;

        public MacroEvent(byte LSB, byte MSB = 0)
        {
            this.macroNoLSB = LSB;
            this.macroNoMSB = MSB;
        }

        public override int GetEventInteger(LightingConsole console)
        {
            return 0x06000000 | (((byte)console.BdstNo) << 16) | (macroNoMSB << 8) | macroNoLSB;
        }
    }
}
