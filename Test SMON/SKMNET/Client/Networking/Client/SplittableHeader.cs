using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Client
{
    public abstract class SplittableHeader : ISplittable
    {
        public abstract short Type { get; }

        public abstract List<byte[]> GetData();
    }
}
