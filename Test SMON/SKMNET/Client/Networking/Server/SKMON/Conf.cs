using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Server.SKMON
{
    [Serializable]
    class Conf : SPacket
    {
        public override int HeaderLength => 0;

        public List<Enums.OVDisp> Disp { get; } = new List<Enums.OVDisp>();

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            ushort count = buffer.ReadUShort();
            int ptr = 2;
            while(ptr < buffer.Length)
            {
                Disp.Add((Enums.OVDisp)Enum.ToObject(typeof(Enums.OVDisp), buffer.ReadUShort()));
                ptr += 2;
            }
            return this;
        }

        public override Enums.Response ProcessPacket(LightingConsole console, ConnectionHandler handler, int type)
        {
            console.DisplayMode = Disp[0];
            return Enums.Response.OK;
        }
    }
}
