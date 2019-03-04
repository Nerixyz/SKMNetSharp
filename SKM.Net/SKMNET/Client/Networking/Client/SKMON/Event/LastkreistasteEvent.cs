using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Client
{
    public class LastkreistasteEvent : Event
    {
        private readonly byte bdst;
        private readonly short lkno;

        // TODO Enum?
        public LastkreistasteEvent(short lkno, byte bdst = 0)
        {
            this.bdst = bdst;
            this.lkno = lkno;
        }

        public override int GetEventInteger()
        {
            byte[] data = BitConverter.GetBytes(lkno);
            Array.Reverse(data);
            return 0x05000000 | (bdst << 4 * 8) | (data[0] << 2 * 8) | data[1];
        }
    }
}
