using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Networking.Server
{
    class BTastConf : Header
    {
        public override int HeaderLength => 4;

        public BTastConfEntry[] entries;
        public ushort count;

        public override Header ParseHeader(byte[] data)
        {
            count = ByteUtils.ToUShort(data, 0);
            entries = new BTastConfEntry[count];
            for(int i = 0; i < count; i++)
            {
                entries[i] = new BTastConfEntry(ByteUtils.ToUShort(data, i * 8), ByteUtils.ToString(data, i * 8 + 2, 6));
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
