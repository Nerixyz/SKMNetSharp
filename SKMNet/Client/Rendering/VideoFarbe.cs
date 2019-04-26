using System;

namespace SKMNET.Client.Rendering
{
    [Serializable]
    public class VideoFarbe
    {
        public readonly NTColor vgColor;
        public readonly NTColor hgColor;
        public readonly short farbno;

        public VideoFarbe(NTColor vg, NTColor hg, short farbno)
        {
            vgColor = vg;
            hgColor = hg;
            this.farbno = farbno;
        }

        public VideoFarbe(short farbno, byte vg1, byte vg2, byte vg3, byte hg1, byte hg2, byte hg3)
        {
            vgColor = new NTColor(vg1, vg2, vg3);
            hgColor = new NTColor(hg1, hg2, hg3);
            this.farbno = farbno;
        }
    }
}
