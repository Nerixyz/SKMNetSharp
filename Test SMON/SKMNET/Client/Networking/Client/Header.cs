using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Networking.Client
{
    abstract class Header : ISendable
    {
        public abstract short Type { get; }

        public abstract byte[] GetDataToSend();
    }
}
