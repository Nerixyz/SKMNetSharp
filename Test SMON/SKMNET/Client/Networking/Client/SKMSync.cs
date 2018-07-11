using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Networking.Client
{
    class SKMSync : CPacket
    {
        public override short Type => 13;

        private Action action;
        private int flags;

        public override byte[] GetDataToSend()
        {
            byte[] steckbrief = new byte[10];
            for(int i = 0; i < 10; i++)
            {
                steckbrief[i] = (flags & (1 << i)) != 0 ? (byte)1 : (byte)0;
            }
            steckbrief[0] = 2;
            if(action == Action.BEGIN)
                return new ByteArrayParser().Add((short)action).Add((short)0).Add((short)10).Add(steckbrief).GetArray();
            else
                return new ByteArrayParser().Add((short)action).Add((short)0).Add((short)0).GetArray();
        }

        public SKMSync(Action action)
        {
            this.action = action;
            this.flags = 0;
        }

        public SKMSync(int flags)
        {
            this.action = Action.BEGIN;
            this.flags = flags;
        }

        public enum Action
        {
            BEGIN = 1,
            END,
            PING
        }

        public enum Flags
        {
            Bedientasten = (1 << 1),
            BefMeldZeile = (1 << 2),
            FuncKeys = (1 << 3),
            LKI = (1 << 4),
            BlockInfo = (1 << 5),
            AZ_Zeilen = (1 << 6),
            ExtKeys = (1 << 7),
            AktInfo = (1 << 8),
            Steller = (1 << 9),
        }
    }
}
