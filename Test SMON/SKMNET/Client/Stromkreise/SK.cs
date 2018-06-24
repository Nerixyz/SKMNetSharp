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
        public ushort DMXOut;
        public ushort DMXLine;
        public List<MLParameter> Parameters { get; set; }
        public byte Intensity { get; set; }
        public byte Attrib { get; set; }

        public SK(ushort Number, byte Intensity = 0, ushort DMXAdress = ushort.MaxValue, ushort DMXLine = 1)
        {
            this.Number = Number;
            this.Intensity = Intensity;
            if (DMXAdress == ushort.MaxValue)
                DMXOut = Number;
            else
                DMXOut = DMXAdress;
            this.DMXLine = DMXLine;
            Attrib = 0;
        }

        public bool Anwahl() { return (Attrib & 0x01) != 0; }
        public bool SKUErr() { return (Attrib & 0x02) != 0; }
        public bool Maske() { return (Attrib & 0x04) != 0; }
        public bool Bet() { return (Attrib & 0x08) != 0; }
        public bool Modified() { return (Attrib & 0x10) != 0; }
        public bool Sperr() { return (Attrib & 0x20) != 0; }
        public bool Heller() { return (Attrib & 0x40) != 0; }
        public bool Dunkler() { return (Attrib & 0x80) != 0; }

        public void AddParameter(MLParameter param)
        {
            if(Parameters == null)
            {
                Parameters = new List<MLParameter>();
            }
            if(param.ParNo == -1)
            {
                param.ParNo = (short)Parameters.Count;
            }
            Parameters.Insert(param.ParNo, param);
        }
    }
}
