using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Networking.Server.T98
{
    class SKRegData : Header
    {
        public override int HeaderLength => 6;
        public ushort start;
        public bool update; /* should display update */
        public ushort count;
        public ushort[] data;

        public override Header ParseHeader(byte[] data)
        {
            start = ByteUtils.ToUShort(data, 0);
            update = ByteUtils.ToUShort(data, 2) != 0x0;
            count = ByteUtils.ToUShort(data, 4);
            this.data = new ushort[count];
            for (int i = 0; i < count; i++)
            {
                this.data[i] = ByteUtils.ToUShort(data, i * 2 + 6);
            }
            return this;
        }
    }
}
