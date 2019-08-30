using SKMNET.Client.Stromkreise.ML;
using System;
using System.Collections.Generic;

namespace SKMNET.Client.Stromkreise
{
    [Serializable]
    public class SK
    {
        public ushort Number { get; }

        public List<MlParameter> Parameters { get; private set; }

        private byte intensity;

        public byte Intensity 
        { 
            get => intensity;
            set {
                Set(value);
                dirty = true;
            }
        }

        public byte Attrib { get; set; }

        public SK(ushort number, byte intensity = 0)
        {
            Number = number;
            Parameters = new List<MlParameter>();
            Intensity = intensity;
            Attrib = 0;
        }

        public void SetIntensity(int intensity)
        {
            Intensity = (byte)intensity;
        }

        /// <summary>
        /// Set the Dimmer value (for packets)
        /// </summary>
        /// <param name="val">Dimmer</param>
        internal void SetDimmer(byte val)
        {
            intensity = val;
            dirty = false;
        }

        private void Set(byte val)
        {
            if (Parameters.Count > 0 && Parameters[0].ParNo == 0)
            {
                Parameters[0].Value = val << 8;
            }
            intensity = val;
        }

        internal bool dirty;

        public bool Anwahl => (Attrib & 0x01) != 0;
        public bool SkuErr => (Attrib & 0x02) != 0;
        public bool Maske => (Attrib & 0x04) != 0;
        public bool Bet => (Attrib & 0x08) != 0;
        public bool Modified => (Attrib & 0x10) != 0;
        public bool Sperr => (Attrib & 0x20) != 0;
        public bool Heller => (Attrib & 0x40) != 0;
        public bool Dunkler => (Attrib & 0x80) != 0;
    }
}
