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

        public override byte[] GetDataToSend(LightingConsole console)
        {
            //no BdstNo ?!
            return new ByteBuffer()
                .Write((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds) // Time
                .Write((short)1) // Count
                .Write((short)1) // Flags
                .Write(GetEventInteger()) // Event Int
                .ToArray();
        }

        public abstract int GetEventInteger();
    }
}
