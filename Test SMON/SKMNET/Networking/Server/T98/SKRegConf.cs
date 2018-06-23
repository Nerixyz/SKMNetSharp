using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Networking.Server.T98
{
    [Serializable]
    class SKRegConf : Header
    {
        public override int HeaderLength => 0;

        public ushort start;
        public bool clear; /* should clear */
        public ushort count;
        public ushort[] data;

        public override Header ParseHeader(byte[] data)
        {
            start = ByteUtils.ToUShort(data, 0);
            clear = ByteUtils.ToUShort(data, 2) != 0x0;
            count = ByteUtils.ToUShort(data, 4);
            this.data = new ushort[count];
            for(int i = 0; i < count; i++)
            {
                this.data[i] = ByteUtils.ToUShort(data, i * 2 + 6);
            }
            return this;
        }
    }
}
