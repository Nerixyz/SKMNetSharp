using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Server.SKMON
{
    /// <summary>
    /// Kommando, s.u.
    /// </summary>
    class SkCmd : SPacket
    {

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
