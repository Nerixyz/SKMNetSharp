using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Header
{
    abstract class Header
    {
        public abstract Header ParseHeader(byte[] data);
        public abstract byte[] ParseData();
        public abstract int HeaderLength { get; }
    }
}
