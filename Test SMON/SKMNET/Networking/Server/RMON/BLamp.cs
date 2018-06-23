using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Networking.Server.RMON
{
    class BLamp : Header
    {
        public override int HeaderLength => 0;

        public byte[] lampStates;

        public override Header ParseHeader(byte[] data)
        {
            lampStates = (byte[])data.Clone();
            return this;
        }

        public enum State
        {
            Aus,
            An,
            Blinken,
            ABlinken
        }
    }
}
