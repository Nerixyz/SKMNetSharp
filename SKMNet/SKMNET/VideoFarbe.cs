using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET
{
    [Serializable]
    class VideoFarbe
    {
            public short Farbno { get; set; }

            public byte Vg_red { get; set; }
            public byte Vg_green { get; set; }
            public byte Vg_blue { get; set; }

            public byte Hg_red { get; set; }
            public byte Hg_green { get; set; }
            public byte Hg_blue { get; set; }

            public VideoFarbe(short farbno, byte vg_red, byte vg_green, byte vg_blue, byte hg_red, byte hg_green, byte hg_blue)
            {
                this.Farbno = farbno;
                this.Vg_red = vg_red;
                this.Vg_green = vg_green;
                this.Vg_blue = vg_blue;
                this.Hg_red = hg_red;
                this.Hg_green = hg_green;
                this.Hg_blue = hg_blue;
            }
    }
}
