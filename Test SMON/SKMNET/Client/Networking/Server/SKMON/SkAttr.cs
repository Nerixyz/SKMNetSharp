using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Networking.Server.SKMON
{
    class SkAttr : SPacket
    {
        public override int HeaderLength => 0;

        public ushort start;
        public ushort count;
        public byte[] data;

        public override SPacket ParseHeader(byte[] data)
        {
            start = ByteUtils.ToUShort(data, 0);
            count = ByteUtils.ToUShort(data, 2);
            this.data = new byte[count];
            for(int i = 0; i < count; i++)
            {
                this.data[i] = data[i + 4];
            }
            return this;
        }
    }
}
