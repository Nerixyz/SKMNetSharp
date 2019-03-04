using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Client
{
    public class StellermeldungEvent : Event
    {
        private readonly byte bdst;
        private readonly byte value;
        private readonly byte stellno;

        // TODO make Enum
        public StellermeldungEvent(byte stellno, byte value, byte bdst = 0)
        {
            this.bdst = bdst;
            this.stellno = stellno;
            this.value = value;
        }

        public override int GetEventInteger()
        {
            return 0x06000000 | (bdst << 16) | (value << 8) | stellno;
        }
    }
}
