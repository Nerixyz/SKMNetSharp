using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Server.RMON
{
    /// <summary>
    /// Auf das RESET wurde ein vollständiges Update gesendet
    /// </summary>
    class ACKReset : SPacket
    {

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            return this;
        }

        public override Enums.Response ProcessPacket(LightingConsole console, ConnectionHandler handler, int type)
        {
            //TODO clear data
            return Enums.Response.OK;
        }
    }
}
