using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Client
{
    class MacroEvent : Event
    {
        readonly byte bdst;
        readonly byte macroNoMSB;
        readonly byte macroNoLSB;
        
        public MacroEvent(byte LSB, byte MSB = 0, byte bdst = 0)
        {
            this.bdst = bdst;
            this.macroNoLSB = LSB;
            this.macroNoMSB = MSB;
        }

        public override int GetEventInteger()
        {
            return 0x06000000 | (bdst << 16) | (macroNoMSB << 8) | macroNoLSB;
        }
    }
}
