using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Client
{
    class SKGAnwahl : CPacket
    {
        public override short Type => 16;

        private readonly AWType action;
        private short[] SKGs;

        public override byte[] GetDataToSend()
        {
            return new ByteArrayParser().Add((short)0).Add((short)action).Add((short)SKGs.Length).Add(SKGs).GetArray();
        }

        public SKGAnwahl(AWType type, short[] SKGs)
        {
            this.action = type;
            this.SKGs = SKGs;
        }


        public enum AWType
        {
            Abs,
            Add,
            Substract
        }
    }
}
