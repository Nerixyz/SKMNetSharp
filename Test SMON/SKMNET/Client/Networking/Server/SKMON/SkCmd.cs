using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Networking.Server.SKMON
{
    class SkCmd : SPacket
    {
        public override int HeaderLength => 0;
        Enums.SKCmd cmd;

        public override SPacket ParseHeader(byte[] data)
        {
            cmd = (Enums.SKCmd)Enum.ToObject(typeof(Enums.SKCmd), data[0]);
            return this;
        }
    }
}
