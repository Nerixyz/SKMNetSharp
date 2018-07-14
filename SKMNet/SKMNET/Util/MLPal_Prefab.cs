using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET
{
    [Serializable]
    class MLPal_Prefab
    {
        public ushort paltype;
        public short palno;
        public string name;

        public MLPal_Prefab(ushort paltype, short palno, string name)
        {
            this.paltype = paltype;
            this.palno = palno;
            this.name = name;
        }

        public bool GetFlag(MLPalFlag flag)
        {
            return (paltype & ((ushort)(flag))) != 0;
        }

        public static bool GetFlag(MLPalFlag flag, ushort paltype)
        {
            return (paltype & ((ushort)(flag))) != 0;
        }

        public enum MLPalFlag
        {
            I = 0x0001      ,   /* I-Palette */
            F = 0x0002      ,   /* F-Palette */
            C = 0x0004      ,   /* C-Palette */
            B = 0x0008      ,   /* B-Palette */
            SKG = 0x0010    ,   /* Stromkreisgruppe */
            BLK = 0x0020    ,   /* Stimmung */
            DYN = 0x0040    ,   /* Dynamics */
            CUR_SEL = 0x0080,   /* Aktuelle Selektion */
        }
    }
}
