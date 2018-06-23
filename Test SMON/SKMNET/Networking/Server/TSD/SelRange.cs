using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Networking.Server.TSD
{
    class SelRange : Header
    {
        public override int HeaderLength => 16;
        public ushort command;
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
            command = BitConverter.ToUInt16(data, 0);
            fixture = BitConverter.ToUInt16(data, 2);
            fixpar = BitConverter.ToUInt16(data, 4);
            val16 = BitConverter.ToUInt16(data, 6);
            res1 = BitConverter.ToUInt16(data, 8);
            res2 = BitConverter.ToUInt16(data, 10);
            last = BitConverter.ToUInt16(data, 12) != 0;
            count = BitConverter.ToUInt16(data, 14);
            arr = new SelRangeData[count];
            for(int i = 0; i < count; i++)
            {
                arr[i] = new SelRangeData(
                    data[i * 16 + 16],
                    data[i * 16 + 17],
                    data[i * 16 + 18],
                    data[i * 16 + 19],
                    BitConverter.ToUInt16(data, i * 16 + 20),
                    BitConverter.ToUInt16(data, i * 16 + 22),
                    Encoding.ASCII.GetString(data, i * 16 + 24, 8));
            }
            return this;
        }

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
