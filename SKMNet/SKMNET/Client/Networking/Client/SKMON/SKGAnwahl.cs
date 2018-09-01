using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Client
{
    public class SKGAnwahl : CPacket
    {
        public override short Type => 16;

        private readonly AWType action;
        private short[] SKGs;

        public override byte[] GetDataToSend()
        {
            return new ByteBuffer().WriteShort(0).WriteShort((short)action).WriteShort((short)SKGs.Length).Write(SKGs).ToArray();
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
