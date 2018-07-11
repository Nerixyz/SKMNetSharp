using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Networking.Client
{
    public abstract class CPacket : ISendable
    {
        public abstract short Type { get; }

        public abstract byte[] GetDataToSend();
    }
}
