using System;

namespace SKMNET.Client.Stromkreise.ML
{
    [Serializable]
    public class MLCParameter
    {
        public Enums.SelRangeDisp Disp { get; internal set; }
        public short Number { get; internal set; }
        public string Name { get; internal set; }
        public (byte start, byte end) Range { get; internal set; }
        public byte Default { get; internal set; }
        public byte Flags { get; internal set; }
        /// <summary>
        /// No linear value (only defaultvalue)
        /// </summary>
        public bool Setpoint => (Flags & 0x01) != 0;

        /// <summary>
        /// Only absolute values no increment/decrement
        /// </summary>
        public bool Encskip => (Flags & 0x02) != 0;

        /// <summary>
        /// Display DMX-Value (0-255)
        /// </summary>
        public bool DMXVal => (Flags & 0x04) != 0;

        /// <summary>
        /// Display percentage (0-100)
        /// </summary>
        public bool Relval => (Flags & 0x08) != 0;


        public MLCParameter(short number, Enums.SelRangeDisp disp, string name)
        {
            Number = number;
            Disp = disp;
            Name = name;
        }
    }
}
