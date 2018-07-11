using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Networking.Server.TSD
{
    [Serializable]
    class MLPalConf : SPacket
    {
        public override int HeaderLength => 8;
        
        public bool absolute; /* Should Update the whole configuration */
        public ushort Mlpaltype { get; set; }/* MLPalFlag */
        public bool last;

        public List<ConfEntry> Entries { get; } = new List<ConfEntry>();

        public override SPacket ParseHeader(byte[] data)
        {
            absolute = ByteUtils.ToUShort(data, 0) == 0;
            Mlpaltype = ByteUtils.ToUShort(data, 2);
            last = ByteUtils.ToUShort(data, 4) != 0;
            int pointer = 6;
            while(pointer < data.Length)
            {
                short palno = ByteUtils.ToShort(data, pointer);
                if (palno == 0)
                    break;
                short length = ByteUtils.ToShort(data, pointer + 2);
                string text = ByteUtils.ToString(data, pointer + 4, length);
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
