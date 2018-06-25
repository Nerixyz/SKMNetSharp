using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Networking.Server
{
    abstract class Header
    {
        public abstract Header ParseHeader(byte[] data);
        public abstract int HeaderLength { get; }
    }
}
