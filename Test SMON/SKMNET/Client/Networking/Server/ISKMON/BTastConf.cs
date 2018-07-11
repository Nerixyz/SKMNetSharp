using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Server
{
    class BTastConf : SPacket
    {
        public override int HeaderLength => 4;

        public BTastConfEntry[] entries;
        public ushort count;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            count = buffer.ReadUShort();
            entries = new BTastConfEntry[count];
            for(int i = 0; i < count; i++)
            {
                entries[i] = new BTastConfEntry(buffer.ReadUShort(), buffer.ReadString(6));
            }
            return this;
        }

        [Serializable]
        public class BTastConfEntry
        {
            public ushort Tastnr { get; }
            public string Name { get; }

            public BTastConfEntry(ushort tastnr, string name)
            {
                this.Tastnr = tastnr;
                this.Name = name;
            }

            public ushort GetNumber()
            {
                return (ushort)( Tastnr & 0x3fff);
            }
        }
    }
}
