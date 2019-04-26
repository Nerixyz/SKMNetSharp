﻿using SKMNET.Client.Networking.Client;
using System;
using System.Globalization;

namespace SKMNET.Client.Stromkreise.ML
{
    [Serializable]
    public class MLParameter
    {
        public string Name { get; set; }
        public short ParNo { get; }
        public (byte Start, byte End) Range { get; set; }
        public short DefaultVal { get; set; }
        public byte Flags { get; set; }
        public double Value { get; set; }
        public string Display { get; set; }
        public string PalName { get; set; }

        [NonSerialized]
        public SK Sk;

        public MLParameter(string name, short parNo = -1, double value = 0)
        {
            Name = name;
            ParNo = parNo;
            Value = value;
            Display = Value.ToString(CultureInfo.InvariantCulture);
            PalName = string.Empty;
        }

        public ParSelect MakeTriggerMLCPacket()
        {
            return new ParSelect(ParNo);
        }
    }
}
