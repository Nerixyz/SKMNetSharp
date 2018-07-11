using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Networking.Server.ISKMON
{
    [Serializable]
    class Bed : SPacket
    {
        public override int HeaderLength => 0;
        
        public string linetext;

        public override SPacket ParseHeader(ByteBuffer buffer)
        {
            // LENGTH = { 2, 31, 1} = 34
            // last byte is unused
            linetext = buffer.ReadString(31);

            return this;
        }
    }
}
