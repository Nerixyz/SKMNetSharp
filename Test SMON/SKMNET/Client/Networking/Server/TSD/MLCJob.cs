using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Networking.Server.TSD
{
    [Serializable]
    class MLCJob : SPacket
    {
        public override int HeaderLength => 18;
        
        public ushort job;
        public uint par1;
        public uint par2;
        public uint par3;
        public ushort res1;
        public ushort count;
        public string buf;

        public override SPacket ParseHeader(byte[] data)
        {
            job = ByteUtils.ToUShort(data, 0);
            par1 = ByteUtils.ToUInt(data, 2);
            par2 = ByteUtils.ToUInt(data, 6);
            par3 = ByteUtils.ToUInt(data, 10);
            res1 = ByteUtils.ToUShort(data, 14);
            count = ByteUtils.ToUShort(data, 16);
            buf = ByteUtils.ToString(data, 18, count);
            return this;
        }

        public bool IsLoad()
        {
            return job == 1;
        }
        public bool IsSave()
        {
            return job == 2;
        }
    }
}
