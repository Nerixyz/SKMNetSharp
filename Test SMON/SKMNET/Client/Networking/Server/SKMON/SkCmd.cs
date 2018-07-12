using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Server.SKMON
{
    class SkCmd : SPacket
    {
        public override int HeaderLength => 0;

        Enums.SKCmd cmd;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            cmd = (Enums.SKCmd)Enum.ToObject(typeof(Enums.SKCmd), buffer.ReadByte());
            return this;
        }

        public override Enums.Response ProcessPacket(LightingConsole console, ConnectionHandler handler, int type)
        {
            //TODO
            return Enums.Response.OK;
        }
    }
}
