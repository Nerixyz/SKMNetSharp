using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Networking.Server.ISKMON
{
    class SKGConf : Header
    {
        public override int HeaderLength => 4;
        public ushort command;
        public ushort count;
        public SKGConfEntry[] entries;

        public override Header ParseHeader(byte[] data)
        {
            command = BitConverter.ToUInt16(data, 0);
            count = BitConverter.ToUInt16(data, 2);
            entries = new SKGConfEntry[count];
            for(int i = 0; i < count; i++)
            {
                entries[i] = new SKGConfEntry(BitConverter.ToUInt16(data, i * 10 + 4), Encoding.ASCII.GetString(data, i * 10 + 6, 8));
            }
            return this;
        }

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
