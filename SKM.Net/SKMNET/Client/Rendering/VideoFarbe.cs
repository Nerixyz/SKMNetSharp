using System;

namespace SKMNET.Client.Rendering
{
    [Serializable]
    public class VideoFarbe
    {
        public readonly Color vgColor;
        public readonly Color hgColor;
        public readonly short farbno;

        public VideoFarbe(Color vg, Color hg, short farbno)
        {
            this.vgColor = vg;
            this.hgColor = hg;
            this.farbno = farbno;
        }

        public VideoFarbe(short farbno, byte vg1, byte vg2, byte vg3, byte hg1, byte hg2, byte hg3)
        {
            vgColor = new Color(vg1, vg2, vg3);
            hgColor = new Color(hg1, hg2, hg3);
            this.farbno = farbno;
        }
    }
}
