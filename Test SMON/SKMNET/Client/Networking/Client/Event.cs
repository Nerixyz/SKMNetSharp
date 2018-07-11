using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Networking.Client
{
    abstract class Event : CPacket
    {
        public override short Type => 14;

        public override byte[] GetDataToSend()
        {
            return new ByteArrayParser().Add(DateTime.Now.Millisecond).Add((short)1).Add((short)0).Add(GetEventInteger()).GetArray();
        }

        public abstract int GetEventInteger();
    }
}
