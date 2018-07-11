using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Server.ISKMON
{
    class AZ : SPacket
    {
        public override int HeaderLength => 0;

        public ushort command;
        public ushort flags;
        public string linetext;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            command = buffer.ReadUShort();
            flags = buffer.ReadUShort();
            linetext = buffer.ReadString(48);
            return this;
        }

        public bool Angewaehlt()
        {
            return (flags & 0x0001) != 0;
        }

        public bool IST()
        {
            return command ==(ushort) Enums.Type.AZ_IST;
        }
        public bool ZIEL()
        {
            return command == (ushort)Enums.Type.AZ_ZIEL;
        }
        public bool VOR()
        {
            return command == (ushort)Enums.Type.AZ_VOR;
        }
    }
}
