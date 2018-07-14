using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Client
{
    public abstract class Event : CPacket
    {
        public override short Type => 14;

        public override byte[] GetDataToSend()
        {
            return new ByteArrayParser().Add((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).Add((short)1).Add((short)0).Add(GetEventInteger()).GetArray();
        }

        public abstract int GetEventInteger();
    }
}
