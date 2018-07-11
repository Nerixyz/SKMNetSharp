using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Server.T98
{
    [Serializable]
    class SKRegData : SPacket
    {
        public override int HeaderLength => 6;

        public ushort start;
        public bool update; /* should display update */
        public ushort count;
        public byte[] data;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            start = buffer.ReadUShort();
            update = buffer.ReadUShort() != 0x0;
            count = buffer.ReadUShort();
            this.data = new byte[count];
            for (int i = 0; i < count; i++)
            {
                this.data[i] = buffer.ReadByte();
            }
            return this;
        }
    }
}
