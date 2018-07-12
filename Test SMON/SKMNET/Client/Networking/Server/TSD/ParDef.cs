using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Server.TSD
{
    class ParDef : SPacket
    {
        public override int HeaderLength => 6;
        
        public bool last;
        public ushort count;
        public ParDefData[] arr;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            last = buffer.ReadUShort() != 0;
            count = buffer.ReadUShort();
            arr = new ParDefData[count];
            for(int i = 0; i < count; i++)
            {
                arr[i] = new ParDefData(
                    buffer.ReadShort(),
                    buffer.ReadShort(),
                    buffer.ReadShort(),
                    buffer.ReadShort(),
                    buffer.ReadString(8));
            }
            return this;
        }

        public override Enums.Response ProcessPacket(LightingConsole console, ConnectionHandler handler, int type)
        {
            console.Prefabs.Clear();
            foreach(ParDefData data in arr)
            {
                console.Prefabs.Add(new Stromkreise.ML.ParPrefab(data.parno, data.dispMode, data.parName));
            }
            return Enums.Response.OK;
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
