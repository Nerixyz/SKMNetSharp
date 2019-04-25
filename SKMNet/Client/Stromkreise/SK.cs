using SKMNET.Client.Stromkreise.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Stromkreise
{
    [Serializable]
    public class SK
    {
        public ushort Number { get; }

        public List<MLParameter> Parameters { get; set; }

        private byte _intensity;

        public byte Intensity { get { return _intensity; } set {
                Set(value);
                dirty = true;
            }
        }

        public byte Attrib { get; set; }

        public SK(ushort Number, byte Intensity = 0)
        {
            this.Number = Number;
            Parameters = new List<MLParameter>();
            this.Intensity = Intensity;
            Attrib = 0;
        }

        public void SetIntensity(int intensity)
        {
            this.Intensity = (byte)intensity;
        }

        /// <summary>
        /// Set the Dimmer value (for packets)
        /// </summary>
        /// <param name="val">Dimmer</param>
        internal void SetDimmer(byte val)
        {
            _intensity = val;
            dirty = false;
        }

        private void Set(byte val)
        {
            if (Parameters.Count > 0 && Parameters[0].ParNo == 0)
            {
                Parameters[0].Value = val << 8;
            }
            _intensity = val;
        }

        internal bool dirty = false;

        public bool Anwahl   { get { return (Attrib & 0x01) != 0; } }
        public bool SKUErr   { get { return (Attrib & 0x02) != 0; } }
        public bool Maske    { get { return (Attrib & 0x04) != 0; } }
        public bool Bet      { get { return (Attrib & 0x08) != 0; } }
        public bool Modified { get { return (Attrib & 0x10) != 0; } }
        public bool Sperr    { get { return (Attrib & 0x20) != 0; } }
        public bool Heller   { get { return (Attrib & 0x40) != 0; } }
        public bool Dunkler  { get { return (Attrib & 0x80) != 0; } }
    }
}
