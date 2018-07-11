﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Stromkreise.ML
{
    [Serializable]
    public partial class MLParameter
    {
        public string Name { get; }
        public short ParNo { get; set; }
        (short, short) range;
        short defaultVal;
        byte flags;
        public double Value { get; set; }
        public string Display { get; set; }
        public string PalName { get; set; }

        public MLParameter(string name, (short, short) range, short parNo = -1, double value = 0, short defaultVal = 0, byte flags = 0)
        {
            this.Name = name;
            this.range = range;

            this.ParNo = parNo;
            this.defaultVal = defaultVal;
            this.flags = flags;
            this.Value = value;
            this.Display = Value.ToString();
            this.PalName = string.Empty;
        }
    }
}
