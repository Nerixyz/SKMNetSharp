using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Client
{
    public class StellermeldungEvent : Event
    {
        private readonly byte value;
        private readonly byte stellno;

        // TODO make Enum
        public StellermeldungEvent(byte stellno, byte value)
        {
            this.stellno = stellno;
            this.value = value;
        }

        public override int GetEventInteger(LightingConsole console)
        {
            return 0x06000000 | (((byte)console.BdstNo) << 16) | (value << 8) | stellno;
        }
    }
}
