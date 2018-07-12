using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Server.SKMON
{
    class Headline : SPacket
    {
        public override int HeaderLength => 0;

        public ushort farbno;
        public ushort count;
        /// <summary>
        /// Kopfzeilen-String
        /// </summary>
        public string data;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            farbno = buffer.ReadUShort();
            count = buffer.ReadUShort();
            this.data = buffer.ReadString(count);
            return this;
        }

        public override Enums.Response ProcessPacket(LightingConsole console, ConnectionHandler handler, int type)
        {
            console.Headline = data;
            return Enums.Response.OK;
        }
    }
}
