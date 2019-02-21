using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Tasten
{
    [Serializable]
    public class Taste
    {
        public ushort Data { get; }
        public ushort TastNR { get { return (ushort)( Data & 0x3fff); } }
        public bool Zweiflankentaste { get { return (Data & 0x8000) != 0; } }
        public bool Lamp { get { return (Data & 0x4000) != 0; } }

        public string Name { get; }

        public LampState State { get; set; }

        public Taste(ushort data, string name)
        {
            this.Data = data;
            this.Name = name;
            this.State = LampState.Aus;
        }

        [Serializable]
        public enum LampState
        {
            Aus,
            An,
            Blinken,
            ABlinken
        }
    }
}
