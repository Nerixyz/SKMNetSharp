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
        public bool Setpoint { get => (Flags & 0x01) != 0; }
        /// <summary>
        /// Only absolute values no increment/decrement
        /// </summary>
        public bool Encskip  { get => (Flags & 0x02) != 0; }
        /// <summary>
        /// Display DMX-Value (0-255)
        /// </summary>
        public bool DMXVal   { get => (Flags & 0x04) != 0; }
        /// <summary>
        /// Display percentage (0-100)
        /// </summary>
        public bool Relval   { get => (Flags & 0x08) != 0; }


        public MLCParameter(short number, Enums.SelRangeDisp disp, string name)
        {
            this.Number = number;
            this.Disp = disp;
            this.Name = name;
        }
    }
}
