using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Networking.Server.TSD
{
    [Serializable]
    class SelRange : Header
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

        public override Header ParseHeader(byte[] data)
        {
            fixture = ByteUtils.ToUShort(data, 0);
            fixpar = ByteUtils.ToUShort(data, 2);
            val16 = ByteUtils.ToUShort(data, 4);
            res1 = ByteUtils.ToUShort(data, 6);
            res2 = ByteUtils.ToUShort(data, 8);
            last = ByteUtils.ToUShort(data, 10) != 0;
            count = ByteUtils.ToUShort(data, 12);
            arr = new SelRangeData[count];
            for(int i = 0; i < count; i++)
            {
                arr[i] = new SelRangeData(
                    data[i * 16 + HeaderLength],
                    data[i * 16 + HeaderLength + 1],
                    data[i * 16 + HeaderLength + 2],
                    data[i * 16 + HeaderLength + 3],
                    ByteUtils.ToUShort(data, i * 16 + HeaderLength + 4),
                    ByteUtils.ToUShort(data, i * 16 + HeaderLength + 6),
                    ByteUtils.ToString(data, i * 16 + HeaderLength + 8, 8));
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
