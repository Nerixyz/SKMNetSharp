using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Server.TSD
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

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            job = buffer.ReadUShort();
            par1 = buffer.ReadUInt();
            par2 = buffer.ReadUInt();
            par3 = buffer.ReadUInt();
            res1 = buffer.ReadUShort();
            count = buffer.ReadUShort();
            buf = buffer.ReadString(count);
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
