using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Stromkreise.ML
{
    class MLParameter
    {
        public string Name { get; }
        public short ParNo { get; set; }
        (short, short) range;
        short defaultVal;
        byte flags;
        public short Value { get; set; }

        public MLParameter(string name, (short, short) range, short parNo = -1, short value = 0, short defaultVal = 0, byte flags = 0)
        {
            this.Name = name;
            this.range = range;
            this.ParNo = parNo;
            this.defaultVal = defaultVal;
            this.flags = flags;
            this.Value = value;
        }



        // future use
        public enum Flags
        {
            Red = 0x1,
            Green = 0x2,
            Blue = 0x4,
            White = 0x8,
            Dimmer = 0x80,
        }
    }
}
