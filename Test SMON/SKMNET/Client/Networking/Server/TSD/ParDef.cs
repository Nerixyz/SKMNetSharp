using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Networking.Server.TSD
{
    class ParDef : SPacket
    {
        public override int HeaderLength => 6;

        public ushort command; /* = SKMON_MLC_PARDEF */
        public bool last;
        public ushort count;
        public ParDefData[] arr;

        public override SPacket ParseHeader(byte[] data)
        {
            command = ByteUtils.ToUShort(data, 0);
            last = ByteUtils.ToUShort(data, 2) != 0;
            count = ByteUtils.ToUShort(data, 4);
            arr = new ParDefData[count];
            for(int i = 0; i < count; i++)
            {
                arr[i] = new ParDefData(
                    ByteUtils.ToShort(data, i * 16 + HeaderLength),
                    ByteUtils.ToShort(data, i * 16 + HeaderLength + 2),
                    ByteUtils.ToShort(data, i * 16 + HeaderLength + 4),
                    ByteUtils.ToShort(data, i * 16 + HeaderLength + 6),
                    ByteUtils.ToString(data, i * 16 + HeaderLength + 8, 8));
            }
            return this;
        }

        public class ParDefData
        {
            public short parno;
            public Enums.SelRangeDisp dispMode;
            public short dispOrder;
            public short reserve2;
            public string parName;

            public ParDefData(short parno, short dispMode, short dispOrder, short reserve2, string parName)
            {
                this.parno = parno;
                this.dispMode = (Enums.SelRangeDisp)dispMode;
                this.dispOrder = dispOrder;
                this.reserve2 = reserve2;
                this.parName = parName;
            }
        }
    }
}
