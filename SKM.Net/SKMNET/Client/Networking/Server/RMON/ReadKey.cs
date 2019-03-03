using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Server.RMON
{
    /// <summary>
    /// Keyboard Eingabe abholen
    /// </summary>
    public class ReadKey : SPacket
    {

        public override SPacket ParsePacket(ByteBuffer buffer) => this;

        public override Enums.Response ProcessPacket(LightingConsole console, ConnectionHandler handler, int type) =>
            //TODO idk
            Enums.Response.OK;
    }
}
