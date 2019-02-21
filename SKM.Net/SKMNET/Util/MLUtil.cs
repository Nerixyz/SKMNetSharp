using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Util
{
    public static class MLUtil
    {
        public static bool GetFlag(MLPalFlag flag, ushort paltype)
        {
            return (paltype & ((ushort)(flag))) != 0;
        }

        public static MLPalFlag GetPalType(ushort value)
        {
            return GetFlag(MLPalFlag.I, value)   ? MLPalFlag.I   :
                   GetFlag(MLPalFlag.F, value)   ? MLPalFlag.F   :
                   GetFlag(MLPalFlag.C, value)   ? MLPalFlag.C   :
                   GetFlag(MLPalFlag.B, value)   ? MLPalFlag.B   :
                   GetFlag(MLPalFlag.SKG, value) ? MLPalFlag.SKG :
                   GetFlag(MLPalFlag.BLK, value) ? MLPalFlag.BLK :
                   GetFlag(MLPalFlag.DYN, value) ? MLPalFlag.DYN : MLPalFlag.CUR_SEL;
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
