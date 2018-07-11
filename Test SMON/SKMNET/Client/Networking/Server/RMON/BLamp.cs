using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Server.RMON
{
    [Serializable]
    class BLamp : SPacket
    {
        public override int HeaderLength => 0;

        public State[] lampStates;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            lampStates = new State[256];
            for(int i = 0; i < 256; i++)
            {
                lampStates[i] = (State)Enum.ToObject(typeof(State), buffer.ReadByte());
            }
            return this;
        }

        [Serializable]
        public enum State
        {
            Aus,
            An,
            Blinken,
            ABlinken
        }
    }
}
