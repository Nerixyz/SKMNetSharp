using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Server.LIBRAExt
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

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            buffer.Forward(4);
            register = buffer.ReadString(8);
            listenanzeige = buffer.ReadString(8);
            return this;
        }
    }
}
