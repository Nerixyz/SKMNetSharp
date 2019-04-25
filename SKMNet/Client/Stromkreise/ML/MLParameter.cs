using SKMNET.Client.Networking.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Stromkreise.ML
{
    [Serializable]
    public partial class MLParameter
    {
        public string Name { get; set; }
        public short ParNo { get; set; }
        public (byte Start, byte End) Range { get; set; }
        public short DefaultVal { get; set; }
        public byte Flags { get; set; }
        public double Value { get; set; }
        public string Display { get; set; }
        public string PalName { get; set; }

        [NonSerialized]
        public SK SK;

        public MLParameter(string name, short parNo = -1, double value = 0)
        {
            this.Name = name;
            this.ParNo = parNo;
            this.Value = value;
            this.Display = Value.ToString();
            this.PalName = string.Empty;
        }

        public ParSelect MakeTriggerMLCPacket()
        {
            return new ParSelect(ParNo);
        }
    }
}
