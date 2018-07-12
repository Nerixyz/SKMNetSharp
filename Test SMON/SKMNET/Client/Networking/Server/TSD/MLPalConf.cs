using SKMNET.Client.Stromkreise.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Server.TSD
{
    [Serializable]
    class MLPalConf : SPacket
    {
        public override int HeaderLength => 8;
        
        public bool absolute; /* Should Update the whole configuration */
        public ushort Mlpaltype { get; set; }/* MLPalFlag */
        public bool last;

        public List<ConfEntry> Entries { get; } = new List<ConfEntry>();

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            absolute = buffer.ReadUShort() == 0;
            Mlpaltype = buffer.ReadUShort();
            last = buffer.ReadUShort() != 0;
            int pointer = 6;
            while(pointer < buffer.Length)
            {
                short palno = buffer.ReadShort();
                if (palno == 0)
                    break;
                short length = buffer.ReadShort();
                string text = buffer.ReadString(length);
                Entries.Add(new ConfEntry(palno, text));
                pointer += 4 + length;
            }
            return this;
        }

        public override Enums.Response ProcessPacket(LightingConsole console, ConnectionHandler handler, int type)
        {
            if (absolute)
            {
                console.IPal.Clear();
                console.FPal.Clear();
                console.CPal.Clear();
                console.BPal.Clear();
            }
            if ((Mlpaltype & 0x0070) == 0)
            {
                MLPal.MLPalFlag flag = (Mlpaltype & 0x0001) != 0 ? MLPal.MLPalFlag.I : (Mlpaltype & 0x0002) != 0 ? MLPal.MLPalFlag.F : (Mlpaltype & 0x0004) != 0 ? MLPal.MLPalFlag.C : MLPal.MLPalFlag.B;
                foreach (var pal in Entries)
                {
                    switch (flag)
                    {
                        case MLPal.MLPalFlag.I:
                            {
                                console.IPal.Add(new MLPal(flag, pal.Text, pal.Palno));
                                break;
                            }
                        case MLPal.MLPalFlag.F:
                            {
                                console.FPal.Add(new MLPal(flag, pal.Text, pal.Palno));
                                break;
                            }
                        case MLPal.MLPalFlag.C:
                            {
                                console.CPal.Add(new MLPal(flag, pal.Text, pal.Palno));
                                break;
                            }
                        case MLPal.MLPalFlag.B:
                            {
                                console.BPal.Add(new MLPal(flag, pal.Text, pal.Palno));
                                break;
                            }
                    }
                }
            }
            return Enums.Response.OK;
        }

        [Serializable]
        public class ConfEntry
        {
            public short Palno { get; set; }
            public string Text { get; set; }

            public ConfEntry(short palno, string text)
            {
                this.Palno = palno;
                this.Text = text;
            }
        }
    }
}
