using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Networking.Server
{
    class PalData : Header
    {
        public override int HeaderLength => 0;

        public VideoFarbe[] farbeintrag;

        public override Header ParseHeader(byte[] data)
        {
            const int farbSize = 8;
            for(int i = 0; i < data.Length; i += farbSize)
            {
                VideoFarbe eintrag = farbeintrag[i / farbSize];
                eintrag = new VideoFarbe(ByteUtils.ToShort(data, i), data[i + 2], data[i + 3], data[i + 4], data[i + 5], data[i + 6], data[i + 7]);
            }
            return this;
        }
        
        public PalData()
        {
            farbeintrag = new VideoFarbe[64];
        }
    }
}
