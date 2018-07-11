﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Networking.Server.ISKMON
{
    [Serializable]
    class FKeyConf : SPacket
    {
        public override int HeaderLength => 4;
        
        public ushort count;
        public FKeyConfEntry[] entries;

        public override SPacket ParseHeader(ByteBuffer buffer)
        {
            count = buffer.ReadUShort();
            entries = new FKeyConfEntry[count];
            for(int i = 0; i < count; i++)
            {
                entries[i] = new FKeyConfEntry(buffer.ReadUShort(), buffer.ReadString(22));
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
