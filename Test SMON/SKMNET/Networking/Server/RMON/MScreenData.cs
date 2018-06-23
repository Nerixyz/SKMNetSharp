using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Networking.Server.RMON
{
    class MScreenData : Header
    {
        public override int HeaderLength => 6;

        public const ushort MON_MASK = 0x000f;
        public const ushort MON_HM_FLAG = 0x8000;
        public const byte MON_MAX = 4;

        ushort monitor;
        ushort start;
        ushort count;
        ushort[] data;

        public override Header ParseHeader(byte[] data)
        {
            monitor = BitConverter.ToUInt16(data, 0);
            start = BitConverter.ToUInt16(data, 2);
            count = BitConverter.ToUInt16(data, 4);

            this.data = new ushort[count];
            for(int i = 0; i < count; i++){
                this.data[i] = BitConverter.ToUInt16(data, 6 + i * 2);
            }

            return this;
        }
    }
}
