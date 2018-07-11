using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Networking.Server.SKMON
{
    class Headline : SPacket
    {
        public override int HeaderLength => 0;

        public ushort farbno;
        public ushort count;
        /// <summary>
        /// Kopfzeilen-String
        /// </summary>
        public string data;

        public override SPacket ParseHeader(byte[] data)
        {
            farbno = ByteUtils.ToUShort(data, 0);
            count = ByteUtils.ToUShort(data, 2);
            this.data = ByteUtils.ToString(data, 4, count);
            return this;
        }
    }
}
