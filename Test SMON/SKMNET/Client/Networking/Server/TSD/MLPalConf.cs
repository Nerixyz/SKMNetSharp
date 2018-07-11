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
