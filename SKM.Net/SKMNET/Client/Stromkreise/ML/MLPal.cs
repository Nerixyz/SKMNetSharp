using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SKMNET.Util;

namespace SKMNET.Client.Stromkreise.ML
{
    [Serializable]
    public class MLPal
    {

        public Flag Type { get; set; }
        public string Name { get; set; }
        public double Number { get; set; }
        public List<SK> BetSK { get; set; }
        public short PalNO { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pal">PalType</param>
        /// <param name="name">PalName</param>
        /// <param name="num">PalNo is converted to double</param>
        public MLPal(Flag pal, string name, short num)
        {
            this.Type = pal;
            this.Name = name;
            this.Number = num / 10d;
            this.BetSK = new List<SK>();
            this.PalNO = num;
        }

        public enum Flag
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

        public static Flag GetFlag(ushort paltype)
        {
            paltype &= 0x0070;
            return (paltype & 0x0001) != 0 ? Flag.I : (paltype & 0x0002) != 0 ? Flag.F : (paltype & 0x0004) != 0 ? Flag.C : (paltype & 0x0008) != 0 ? Flag.B : (paltype & 0x0010) != 0 ? Flag.SKG : (paltype & 0x0020) != 0 ? Flag.BLK : (paltype & 0x0040) != 0 ? Flag.DYN : Flag.CUR_SEL;
        }
    }
}
