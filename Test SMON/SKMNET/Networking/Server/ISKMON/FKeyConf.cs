using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Networking.Server.ISKMON
{
    class FKeyConf : Header
    {
        public override int HeaderLength => 4;

        public ushort cmd;
        public ushort count;
        public FKeyConfEntry[] entries;

        public override Header ParseHeader(byte[] data)
        {
            cmd = BitConverter.ToUInt16(data, 0);
            count = BitConverter.ToUInt16(data, 2);
            entries = new FKeyConfEntry[count];
            for(int i = 0; i < count; i++)
            {
                entries[i] = new FKeyConfEntry(BitConverter.ToUInt16(data, i * 24 + 4), Encoding.ASCII.GetString(data, i * 24 + 4 + 2, 22));
            }
            return this;
        }

        public class FKeyConfEntry
        {
            public ushort fkeynr;
            public string label;

            public FKeyConfEntry(ushort fkeynr, string label)
            {
                this.fkeynr = fkeynr;
                this.label = label;
            }
        }
    }
}
