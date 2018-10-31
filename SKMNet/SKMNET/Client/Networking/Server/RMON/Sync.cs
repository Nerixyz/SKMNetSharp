using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Server.RMON
{
    /// <summary>
    /// Synctelegramm
    /// </summary>
    class Sync : SPacket
    {

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            return this;
        }

        public override Enums.Response ProcessPacket(LightingConsole console, ConnectionHandler handler, int type)
        {
            return Enums.Response.OK;
        }
    }
}
