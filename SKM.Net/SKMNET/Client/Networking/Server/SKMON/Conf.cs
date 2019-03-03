using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Server.SKMON
{
    /// <summary>
    /// Konfigurationsdaten
    /// </summary>
    [Serializable]
    public class Conf : SPacket
    {

        public List<Enums.OVDisp> Disp { get; } = new List<Enums.OVDisp>();

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            ushort count = buffer.ReadUShort();
            for(int i = 0; i < count; i++)
            {
                Disp.Add(Enums.GetEnum<Enums.OVDisp>(buffer.ReadUShort()));
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
