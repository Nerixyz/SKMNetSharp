using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Server.SKMON
{
    class SkAttr : SPacket
    {
        public override int HeaderLength => 0;

        public ushort start;
        public ushort count;
        public byte[] data;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            start = buffer.ReadUShort();
            count = buffer.ReadUShort();
            this.data = new byte[count];
            for(int i = 0; i < count; i++)
            {
                this.data[i] = buffer.ReadByte();
            }
            return this;
        }
    }
}
