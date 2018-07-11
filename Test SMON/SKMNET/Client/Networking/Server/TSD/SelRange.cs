using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Server.TSD
{
    [Serializable]
    class SelRange : SPacket
    {
        public override int HeaderLength => 14;
        public ushort fixture;
        public ushort fixpar;
        public ushort val16;
        public ushort res1;
        public ushort res2;
        public bool last;
        public ushort count;

        public SelRangeData[] arr;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            fixture = buffer.ReadUShort();
            fixpar = buffer.ReadUShort();
            val16 = buffer.ReadUShort();
            res1 = buffer.ReadUShort();
            res2 = buffer.ReadUShort();
            last = buffer.ReadUShort() != 0;
            count = buffer.ReadUShort();
            arr = new SelRangeData[count];
            for(int i = 0; i < count; i++)
            {
                arr[i] = new SelRangeData(
                    buffer.ReadByte(),
                    buffer.ReadByte(),
                    buffer.ReadByte(),
                    buffer.ReadByte(),
                    buffer.ReadUShort(),
                    buffer.ReadUShort(),
                    buffer.ReadString(8));
            }
            return this;
        }

        [Serializable]
        public class SelRangeData
        {
            public byte start;
            public byte end;
            public byte defaultVal;
            public byte flags;
            public ushort res1;
            public ushort res2;
            public string name;

            public SelRangeData(byte start, byte end, byte defaultVal, byte flags, ushort res1, ushort res2, string name)
            {
                this.start = start;
                this.end = end;
                this.defaultVal = defaultVal;
                this.flags = flags;
                this.res1 = res1;
                this.res2 = res2;
                this.name = name;
            }
        }
    }
}
