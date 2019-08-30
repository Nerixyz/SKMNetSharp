namespace SKMNET.Util
{
    public static class MlUtil
    {
        public static bool GetFlag(MlPalFlag flag, ushort paltype) => (paltype & (ushort)flag) != 0;

        public static MlPalFlag GetPalType(ushort value)
        {
            return GetFlag(MlPalFlag.I, value)   ? MlPalFlag.I   :
                   GetFlag(MlPalFlag.F, value)   ? MlPalFlag.F   :
                   GetFlag(MlPalFlag.C, value)   ? MlPalFlag.C   :
                   GetFlag(MlPalFlag.B, value)   ? MlPalFlag.B   :
                   GetFlag(MlPalFlag.SKG, value) ? MlPalFlag.SKG :
                   GetFlag(MlPalFlag.BLK, value) ? MlPalFlag.BLK :
                   GetFlag(MlPalFlag.DYN, value) ? MlPalFlag.DYN : MlPalFlag.CUR_SEL;
        }

        public enum MlPalFlag
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
