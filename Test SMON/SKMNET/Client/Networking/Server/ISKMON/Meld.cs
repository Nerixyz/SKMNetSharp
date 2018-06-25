using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Networking.Server.ISKMON
{
    class Meld : Header
    {
        public override int HeaderLength => 0;

        public ushort cmd;
        public string linetext;

        public override Header ParseHeader(byte[] data)
        {
            // LENGTH = { 2, 31, 1} = 34
            // last byte is unused
            cmd = ByteUtils.ToUShort(data, 0);
            linetext = ByteUtils.ToString(data, 2, 31);

            return this;
        }
    }
}
