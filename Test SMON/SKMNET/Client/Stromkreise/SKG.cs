using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Stromkreise
{
    [Serializable]
    public class SKG
    {
        public ushort Number { get; set; }
        public string Name { get; set; }

        public SKG(ushort number, string name)
        {
            this.Number = number;
            this.Name = name;
        }

    }
}
