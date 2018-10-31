using SKMNET.Client.Tasten;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Server.RMON
{
    /// <summary>
    /// Lampendaten fuer Bedientasten (Komplett-Telegramm)
    /// </summary>
    [Serializable]
    class BLamp : SPacket
    {

        public Taste.LampState[] lampStates;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            lampStates = new Taste.LampState[256];
            for(int i = 0; i < 256; i++)
            {
                lampStates[i] = (Taste.LampState)Enum.ToObject(typeof(Taste.LampState), buffer.ReadByte());
            }
            return this;
        }

        public override Enums.Response ProcessPacket(LightingConsole console, ConnectionHandler handler, int type)
        {
            for(int i = 0; i < lampStates.Length /* 256 */; i++)
            {
                Taste taste = console.TastenManager.FindByNumber(i);
                if(taste != null)
                    taste.State = lampStates[i];
            }
            return Enums.Response.OK;
        }
    }
}
