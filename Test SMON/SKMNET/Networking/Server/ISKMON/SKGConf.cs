using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Networking.Server.ISKMON
{
    [Serializable]
    class SKGConf : Header
    {
        public override int HeaderLength => 4;
        public ushort count;
        public SKGConfEntry[] entries;

        public override Header ParseHeader(byte[] data)
        {
            count = ByteUtils.ToUShort(data, 0);
            entries = new SKGConfEntry[count];
            for(int i = 0; i < count; i++)
            {
                entries[i] = new SKGConfEntry(ByteUtils.ToUShort(data, i * 10 + 2), ByteUtils.ToString(data, i * 10 + 4, 8));
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
