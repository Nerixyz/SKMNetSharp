using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Networking.Server.TSD
{
    class MLPalConf : Header
    {
        public override int HeaderLength => 8;

        public ushort command;
        public bool absolute; /* Should Update the whole configuration */
        public ushort mlpaltype; /* MLPalFlag */
        public bool last;

        public List<ConfEntry> entries;

        public override Header ParseHeader(byte[] data)
        {
            command = BitConverter.ToUInt16(data, 0);
            absolute = BitConverter.ToUInt16(data, 2) == 0;
            mlpaltype = BitConverter.ToUInt16(data, 4);
            last = BitConverter.ToUInt16(data, 6) != 0;
            int pointer = HeaderLength;
            entries = new List<ConfEntry>();
            while(pointer < data.Length)
            {
                short palno = BitConverter.ToInt16(data, pointer);
                if (palno == 0)
                    break;
                short length = BitConverter.ToInt16(data, pointer + 2);
                string text = Encoding.ASCII.GetString(data, pointer + 4, length);
                entries.Add(new ConfEntry(palno, text));
                pointer += 4 + length;
            }
            return this;
        }

        public class ConfEntry
        {
            public short palno;
            public string text;

            public ConfEntry(short palno, string text)
            {
                this.palno = palno;
                this.text = text;
            }
        }
    }
}
