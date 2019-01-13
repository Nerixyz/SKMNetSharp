using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Client
{
    /// <summary>
    /// Auswahl für Palettendaten
    /// </summary>
    public class PalSelect : CPacket
    {
        public override short Type => 24;
        private readonly short PalMask;
        private const short BsStNo = 0;
        private const short SubCmd = 0;

        public override byte[] GetDataToSend()
        {
            return new ByteBuffer().Write(BsStNo).Write(SubCmd).Write(PalMask).ToArray();
        }

        public PalSelect(MLUtil.MLPalFlag PalMask)
        {
            this.PalMask = (short)PalMask;
        }

        public PalSelect(short mask)
        {
            this.PalMask = mask;
        }
    }
}
