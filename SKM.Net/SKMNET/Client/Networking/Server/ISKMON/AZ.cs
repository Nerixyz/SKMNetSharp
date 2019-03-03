using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Server.ISKMON
{
    /// <summary>
    /// Aktuellzeile
    /// </summary>
    public class AZ : SPacket
    {

        public ushort flags;
        public string linetext;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            flags = buffer.ReadUShort();
            linetext = buffer.ReadString(48);
            return this;
        }

        public bool Angewaehlt() => (flags & 0x0001) != 0;

        public override Enums.Response ProcessPacket(LightingConsole console, ConnectionHandler handler, int type)
        {
            switch((Enums.Type)Enum.ToObject(typeof(Enums.Type), type))
            {
                case Enums.Type.AZ_IST: console.RegIST.Text = linetext; console.RegIST.AW = Angewaehlt(); break;
                case Enums.Type.AZ_ZIEL: console.RegZIEL.Text = linetext; console.RegZIEL.AW = Angewaehlt(); break;
                case Enums.Type.AZ_VOR: console.RegVOR.Text = linetext; console.RegVOR.AW = Angewaehlt(); break;
            }

            return Enums.Response.OK;
        }
    }
}
