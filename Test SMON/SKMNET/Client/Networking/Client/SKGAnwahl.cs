using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Networking.Client
{
    class SKGAnwahl : Header
    {
        public override short Type => 16;

        private AWType action;
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
