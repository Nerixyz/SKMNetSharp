using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Server.ISKMON
{
    [Serializable]
    class SKGConf : SPacket
    {
        public override int HeaderLength => 4;
        public ushort count;
        public SKGConfEntry[] entries;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            count = buffer.ReadUShort();
            entries = new SKGConfEntry[count];
            for(int i = 0; i < count; i++)
            {
                entries[i] = new SKGConfEntry(buffer.ReadUShort(), buffer.ReadString(8));
            }
            return this;
        }

        [Serializable]
        public class SKGConfEntry
        {
            public ushort skgnum;
            public string skgname;

            public SKGConfEntry(ushort skgnum, string skgname)
            {
                this.skgnum = skgnum;
                this.skgname = skgname;
            }
        }
    }
}
