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
        public byte Intensity { get { return _intensity; } set {
                Set(value);
                dirty = true;
            }
        }
        private byte _intensity;
        public byte Attrib { get; set; }
        private bool active;

        [NonSerialized]
        private readonly LightingConsole console;

        public SK(ushort Number, LightingConsole console, byte Intensity = 0)
        {
            this.active = Intensity > 0;

            this.console = console;

            this.Number = Number;
            this.Parameters = new List<MLParameter>();
            this.Intensity = Intensity;
            this.Attrib = 0;
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
            if (val > 0 && !active)
            {
                active = true;
                if(!console.ActiveSK.Contains(this))
                    console.ActiveSK.Add(this);
            }
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
