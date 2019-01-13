using SKMNET.Client.Stromkreise;
using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Client
{
    /// <summary>
    /// DMX-orientierte Parameterwerte
    /// </summary>
    public class DmxData : CPacket
    {
        public override short Type => 21;
        private const short BdStNo = 0;
        private const short subCmd = 0;
        private readonly Enums.FixParDst dst;
        private List<SK> SKs;
        private List<byte[]> data;

        public override byte[] GetDataToSend()
        {
            ByteBuffer buf = new ByteBuffer().
                Write(BdStNo).
                Write(subCmd).
                Write((short)dst);
            if(SKs != null)
            {
                buf.WriteShort(1);
                SKs.Sort(new Comparison<SK>((SK n1, SK n2) =>
                {
                    if (n1.Number > n2.Number) return -1;
                    else if (n2.Number > n1.Number) return 1;
                    else return 0;
                }));
                // assume they are all on the same line
                buf.WriteShort(1);
                int ptr = 0;
                for(int i = 0; i < 512; i++)
                {
                    if(SKs.Count > ptr && SKs[ptr].Number == i + 1)
                    {
                        buf.Write(SKs[ptr].Intensity);
                        ptr++;
                    }
                    else
                    {
                        buf.Write((byte)0);
                    }
                }
            }
            else if(data != null)
            {
                buf.Write(data.Count);

                for(int i = 0; i < data.Count; i++)
                {
                    buf.WriteShort((short)i);
                    buf.Write(data[i]);
                }
            }
            else
            {
                buf.WriteShort(0);
            }
            return buf.ToArray();
        }
        
        public DmxData(List<SK> SKs = null, Enums.FixParDst dst = Enums.FixParDst.Current)
        {
            this.SKs = SKs;
            this.dst = dst;
            //throw new NotImplementedException("sknum != dmxout");
        }

        public DmxData(List<byte[]> data, Enums.FixParDst dst = Enums.FixParDst.Current)
        {
            this.dst = dst;
            if (data.Count == 0)
                return;
            if (data.Count > 8)
                throw new NotSupportedException("Cannot send more than 8 arrays at once");

            for(int i = 0; i < data.Count; i++)
            {
                if (data[i] == null)
                {
                    data.Remove(data[i]);
                    continue;
                }
                if(data[i].Length != 512)
                {
                    byte[] holder = new byte[512];
                    Array.Copy(data[i], holder, Math.Min(data[i].Length, 512));
                }
            }
            this.data = data;
        }
    }
}
