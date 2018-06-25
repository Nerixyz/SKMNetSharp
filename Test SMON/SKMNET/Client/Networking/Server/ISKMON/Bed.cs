using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Networking.Server.ISKMON
{
    [Serializable]
    class Bed : Header
    {
        public override int HeaderLength => 0;
        
        public string linetext;

        public override Header ParseHeader(byte[] data)
        {
            // LENGTH = { 2, 31, 1} = 34
            // last byte is unused
            linetext = ByteUtils.ToString(data, 0, 31);

            return this;
        }
    }
}
