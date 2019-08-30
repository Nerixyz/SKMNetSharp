using System;
using System.Collections.Generic;
using SKMNET.Client.Stromkreise;

namespace SKMNET.Client.Networking.Client.Ext
{
    /// <summary>
    /// DMX-orientierte Parameterwerte
    /// </summary>
    public class DmxData : CPacket
    {
        public override short Type => 21;
        private const short SUB_CMD = 0;
        public readonly Enums.FixParDst Dst;
        private readonly List<SK> sks;
        private readonly List<byte[]> data;

        public override byte[] GetDataToSend(LightingConsole console)
        {
            ByteBuffer buf = new ByteBuffer().Write(console.BdstNo).Write(SUB_CMD).Write((short) Dst);
            if (sks != null)
            {
                buf.WriteShort(1);
                sks.Sort((n1, n2) =>
                {
                    if (n1.Number > n2.Number) return -1;
                    return n2.Number > n1.Number ? 1 : 0;
                });

                // assume they are all on the same line
                buf.WriteShort(1);
                int ptr = 0;
                for (int i = 0; i < 512; i++)
                {
                    if (sks.Count > ptr && sks[ptr].Number == i + 1)
                    {
                        buf.Write(sks[ptr].Intensity);
                        ptr++;
                    }
                    else
                    {
                        buf.Write((byte) 0);
                    }
                }
            }
            else if (data != null)
            {
                buf.Write(data.Count);

                for (int i = 0; i < data.Count; i++)
                {
                    buf.WriteShort((short) i);
                    buf.Write(data[i]);
                }
            }
            else
            {
                buf.WriteShort(0);
            }

            return buf.ToArray();
        }

        public DmxData(List<SK> sks = null, Enums.FixParDst dst = Enums.FixParDst.Current)
        {
            this.sks = sks;
            Dst = dst;
            //throw new NotImplementedException("sknum != dmxout");
        }

        public DmxData(List<byte[]> data, Enums.FixParDst dst = Enums.FixParDst.Current)
        {
            Dst = dst;
            if (data.Count == 0)
                return;
            if (data.Count > 8)
                throw new NotSupportedException("Cannot send more than 8 arrays at once");

            for (int i = 0; i < data.Count; i++)
            {
                if (data[i] == null)
                {
                    data.Remove(data[i]);
                    continue;
                }

                if (data[i].Length == 512) continue;

                byte[] holder = new byte[512];
                Array.Copy(data[i], holder, Math.Min(data[i].Length, 512));
            }

            this.data = data;
        }
    }
}