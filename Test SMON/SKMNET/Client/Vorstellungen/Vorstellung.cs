using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Vorstellungen
{
    [Serializable]
    public class Vorstellung
    {
        public ushort Number;

        public Vorstellung(ushort number)
        {
            this.Number = number;
        }
    }
}
