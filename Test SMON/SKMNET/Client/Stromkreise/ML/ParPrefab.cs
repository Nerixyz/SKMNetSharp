using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Stromkreise.ML
{
    [Serializable]
    public class ParPrefab
    {
        public Enums.SelRangeDisp Disp { get; set; }
        public short Number { get; set; }
        public string Name { get; set; }

        public ParPrefab(short number, Enums.SelRangeDisp disp, string name)
        {
            this.Number = number;
            this.Disp = disp;
            this.Name = name;
        }
    }
}
