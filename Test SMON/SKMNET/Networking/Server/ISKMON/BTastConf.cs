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
        public ushort cmd;
        public ushort count;

        public override Header ParseHeader(byte[] data)
        {
            cmd = BitConverter.ToUInt16(data, 0);
            count = BitConverter.ToUInt16(data, 2);
            entries = new BTastConfEntry[count];
            for(int i = 0; i < count; i++)
            {
                entries[i] = new BTastConfEntry(BitConverter.ToUInt16(data, i * 8), Encoding.ASCII.GetString(data, i * 8 + 2, 6));
            }
            return this;
        }

        public class BTastConfEntry
        {
            public ushort tastnr;
            public string name;

            public BTastConfEntry(ushort tastnr, string name)
            {
                this.tastnr = tastnr;
                this.name = name;
            }

            public ushort GetNumber()
            {
                return (ushort)( tastnr & 0x3fff);
            }
        }
    }
}
