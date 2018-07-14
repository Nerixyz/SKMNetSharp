using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Server.ISKMON
{
    class Meld : SPacket
    {
        public override int HeaderLength => 0;
        
        public string linetext;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            // LENGTH = { 2, 31, 1} = 34
            // last byte is unused
            linetext = buffer.ReadString(31);

            return this;
        }

        public override Enums.Response ProcessPacket(LightingConsole console, ConnectionHandler handler, int type)
        {
            console.Meldezeile = linetext;
            return Enums.Response.OK;
        }
    }
}
