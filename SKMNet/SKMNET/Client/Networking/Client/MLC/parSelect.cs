using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Client
{
    public class ParSelect : CPacket
    {
        public override short Type => 27;

        private readonly short parno;
        private readonly short subcmd;
        private readonly short bdstno;

        public ParSelect(short parNo, short subcmd = 0, short bdstno = 0)
        {
            this.parno = parNo;
            this.subcmd = subcmd;
            this.bdstno = bdstno;
        }


        public override byte[] GetDataToSend()
        {
            return new ByteBuffer().Write(parno).Write(subcmd).Write(bdstno).ToArray();
        }
    }
}
