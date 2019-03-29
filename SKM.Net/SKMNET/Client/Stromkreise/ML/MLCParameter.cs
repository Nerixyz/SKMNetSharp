using System;

namespace SKMNET.Client.Stromkreise.ML
{
    [Serializable]
    public class MLCParameter
    {
        public Enums.SelRangeDisp Disp { get; set; }
        public short Number { get; set; }
        public string Name { get; set; }

        public MLCParameter(short number, Enums.SelRangeDisp disp, string name)
        {
            this.Number = number;
            this.Disp = disp;
            this.Name = name;
        }
    }
}
