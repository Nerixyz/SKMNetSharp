using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Networking.Server.SKMON
{
    class Conf : Header
    {
        public override int HeaderLength => 0;

        Enums.OVDisp disp;

        public override Header ParseHeader(byte[] data)
        {
            disp = (Enums.OVDisp)Enum.ToObject(typeof(Enums.OVDisp), data[2]);
            return this;
        }
    }
}
