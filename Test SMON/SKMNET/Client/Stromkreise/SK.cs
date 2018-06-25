using SKMNET.Client.Stromkreise.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Stromkreise
{
    [Serializable]
    class SK
    {
        public ushort Number { get; }
        public List<MLParameter> Parameters { get; set; }
        public byte Intensity { get; set; }
        public byte Attrib { get; set; }

        public SK(ushort Number, byte Intensity = 0)
        {
            this.Number = Number;
            this.Intensity = Intensity;
            Attrib = 0;
            Parameters = new List<MLParameter>();
        }

        public bool Anwahl() { return (Attrib & 0x01) != 0; }
        public bool SKUErr() { return (Attrib & 0x02) != 0; }
        public bool Maske() { return (Attrib & 0x04) != 0; }
        public bool Bet() { return (Attrib & 0x08) != 0; }
        public bool Modified() { return (Attrib & 0x10) != 0; }
        public bool Sperr() { return (Attrib & 0x20) != 0; }
        public bool Heller() { return (Attrib & 0x40) != 0; }
        public bool Dunkler() { return (Attrib & 0x80) != 0; }
    }
}
