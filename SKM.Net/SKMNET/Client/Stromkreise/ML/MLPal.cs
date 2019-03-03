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

        public MLPalFlag Type { get; set; }
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
        public MLPal(MLPalFlag pal, string name, short num)
        {
            this.Type = pal;
            this.Name = name;
            this.Number = num / 10d;
            this.BetSK = new List<SK>();
            this.PalNO = num;
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

        public static MLPalFlag GetFlag(ushort paltype)
        {
            paltype &= 0x0070;
            return (paltype & 0x0001) != 0 ? MLPalFlag.I : (paltype & 0x0002) != 0 ? MLPalFlag.F : (paltype & 0x0004) != 0 ? MLPalFlag.C : (paltype & 0x0008) != 0 ? MLPalFlag.B : (paltype & 0x0010) != 0 ? MLPalFlag.SKG : (paltype & 0x0020) != 0 ? MLPalFlag.BLK : (paltype & 0x0040) != 0 ? MLPalFlag.DYN : MLPalFlag.CUR_SEL;
        }
    }
}
