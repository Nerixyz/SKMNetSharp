using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Stromkreise.ML
{
    class MLPal
    {
        public Enums.Pal Type { get; set; }
        public string Name { get; set; }
        public double Number { get; set; }
        
        public MLPal(Enums.Pal pal, string name, ushort num)
        {
            if (((ushort)pal & 0x0070) != 0)
                throw new Exception("MLPal is only for IFCB pal");
            this.Type = pal;
            this.Name = name;
            this.Number = num / 10d;
        }

        public enum MLPalFlag
        {
            I = 0x0001,   /* I-Palette */
            F = 0x0002,   /* F-Palette */
            C = 0x0004,   /* C-Palette */
            B = 0x0008,   /* B-Palette */

            SKG = 0x0010,   /* Stromkreisgruppe */
            BLK = 0x0020,   /* Stimmung */
            DYN = 0x0040,   /* Dynamics */
            CUR_SEL = 0x0080,   /* Aktuelle Selektion */
        }
    }
}
