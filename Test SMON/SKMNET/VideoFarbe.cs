using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET
{
    class VideoFarbe
    {
            public short farbno;

            public byte vg_red;
            public byte vg_green;
            public byte vg_blue;

            public byte hg_red;
            public byte hg_green;
            public byte hg_blue;

            public VideoFarbe(short farbno, byte vg_red, byte vg_green, byte vg_blue, byte hg_red, byte hg_green, byte hg_blue)
            {
                this.farbno = farbno;
                this.vg_red = vg_red;
                this.vg_green = vg_green;
                this.vg_blue = vg_blue;
                this.hg_red = hg_red;
                this.hg_green = hg_green;
                this.hg_blue = hg_blue;
            }
    }
}
