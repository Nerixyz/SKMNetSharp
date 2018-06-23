using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Networking.Server.TSD
{
    class MLCJob : Header
    {
        public override int HeaderLength => throw new NotImplementedException();

        public ushort command; /* = SKMON_MLC_JOB */
        public ushort job;
        public uint par1;
        public uint par2;
        public uint par3;
        public ushort res1;
        public ushort count;
        public string buf;

        public override Header ParseHeader(byte[] data)
        {
            command = BitConverter.ToUInt16(data, 0);
            job = BitConverter.ToUInt16(data, 2);
            par1 = BitConverter.ToUInt32(data, 4);
            par2 = BitConverter.ToUInt32(data, 8);
            par3 = BitConverter.ToUInt32(data, 12);
            res1 = BitConverter.ToUInt16(data, 16);
            count = BitConverter.ToUInt16(data, 18);
            buf = Encoding.ASCII.GetString(data, 20, count);
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
