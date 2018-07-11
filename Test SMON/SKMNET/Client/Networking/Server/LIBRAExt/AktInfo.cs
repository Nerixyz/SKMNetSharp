using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Networking.Server.LIBRAExt
{
    [Serializable]
    class AktInfo : SPacket
    {
        public override int HeaderLength => 4;
        public string register;
        public string listenanzeige;
        /*
         * Header contains
         * ----------------
         * ushort version
         * ushort datalen
         * ----------------
         * -> useless
         * */

        public override SPacket ParseHeader(byte[] data)
        {
            register = ByteUtils.ToString(data, 4, 8);
            listenanzeige = ByteUtils.ToString(data, 4 + 8, 8);
            return this;
        }
    }
}
