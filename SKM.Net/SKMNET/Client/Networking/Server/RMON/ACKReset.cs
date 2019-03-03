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
    public class ACKReset : SPacket
    {

        public override SPacket ParsePacket(ByteBuffer buffer) => this;

        public override Enums.Response ProcessPacket(LightingConsole console, ConnectionHandler handler, int type) =>
            //TODO clear data
            Enums.Response.OK;
    }
}
