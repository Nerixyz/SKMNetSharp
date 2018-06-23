using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Networking.Server.ISKMON
{
    [Serializable]
    class FKeyConf : Header
    {
        public override int HeaderLength => 4;

        public ushort cmd;
        public ushort count;
        public FKeyConfEntry[] entries;

        public override Header ParseHeader(byte[] data)
        {
            count = ByteUtils.ToUShort(data, 0);
            entries = new FKeyConfEntry[count];
            for(int i = 0; i < count; i++)
            {
                entries[i] = new FKeyConfEntry(ByteUtils.ToUShort(data, i * 24 + 2), ByteUtils.ToString(data, i * 24 + 2 + 2, 22));
            }
            return this;
        }

        [Serializable]
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
