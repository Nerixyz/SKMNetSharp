using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SKMNET.Util;

namespace SKMNET.Client.Stromkreise.ML
{
    [Serializable]
    public class MlPal
    {
        public Flag Type { get; }
        public string Name { get; set; }
        public double Number { get; set; }
        public List<SK> BetSk { get; }
        public short PalNo { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pal">PalType</param>
        /// <param name="name">PalName</param>
        /// <param name="num">PalNo is converted to double</param>
        public MlPal(Flag pal, string name, short num)
        {
            Type = pal;
            Name = name;
            Number = num / 10d;
            BetSk = new List<SK>();
            PalNo = num;
        }

        public enum Flag
        {
            /// <summary>
            /// I-Palette
            /// </summary>
            I = 0x0001,
            /// <summary>
            /// F-Palette
            /// </summary>
            F = 0x0002,
            /// <summary>
            /// C-Palette
            /// </summary>
            C = 0x0004,
            /// <summary>
            /// B-Palette
            /// </summary>
            B = 0x0008,

            /// <summary>
            /// Stromkreisgruppe
            /// </summary>
            SKG = 0x0010,
            /// <summary>
            /// Stimmung
            /// </summary>
            BLK = 0x0020,
            /// <summary>
            /// Dynamics
            /// </summary>
            DYN = 0x0040,
            /// <summary>
            /// Aktuelle Selektion
            /// </summary>
            CUR_SEL = 0x0080
        }

        public static Flag GetFlag(ushort paltype)
        {
            paltype &= 0x0070;
            return 
                (paltype & 0x0001) != 0 ? Flag.I :
                (paltype & 0x0002) != 0 ? Flag.F :
                (paltype & 0x0004) != 0 ? Flag.C :
                (paltype & 0x0008) != 0 ? Flag.B :
                (paltype & 0x0010) != 0 ? Flag.SKG :
                (paltype & 0x0020) != 0 ? Flag.BLK :
                (paltype & 0x0040) != 0 ? Flag.DYN :
                                          Flag.CUR_SEL;
        }
    }
}
