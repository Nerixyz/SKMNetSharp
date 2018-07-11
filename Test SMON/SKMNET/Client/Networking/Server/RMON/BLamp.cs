using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Networking.Server.RMON
{
    [Serializable]
    class BLamp : SPacket
    {
        public override int HeaderLength => 0;

        public State[] lampStates;

        public override SPacket ParseHeader(byte[] data)
        {
            lampStates = new State[data.Length];
            for(int i = 0; i < data.Length; i++)
            {
                lampStates[i] = (State)Enum.ToObject(typeof(State), data[i]);
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
