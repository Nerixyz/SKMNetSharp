using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Client
{
    class PalSelect : CPacket
    {
        public override short Type => 24;
        private readonly short PalMask;
        private const short BsStNo = 0;
        private const short SubCmd = 0;

        public override byte[] GetDataToSend()
        {
            return new ByteArrayParser().Add(BsStNo).Add(SubCmd).Add(PalMask).GetArray();
        }

        public PalSelect(short PalMask)
        {
            this.PalMask = PalMask;
        }
    }
}
