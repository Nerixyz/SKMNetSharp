using SKMNET.Client.Stromkreise;
using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Server.T98
{
    /// <summary>
    /// Stromkreisregister-Aufbau
    /// </summary>
    [Serializable]
    public class SKRegConf : SPacket
    {
        public ushort start;
        public bool clear; /* should clear */
        public ushort count;
        public ushort[] data;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            start = buffer.ReadUShort();
            clear = buffer.ReadUShort() != 0x0;
            count = buffer.ReadUShort();
            this.data = new ushort[count];
            for(int i = 0; i < count; i++)
            {
                this.data[i] = buffer.ReadUShort();
            }
            return this;
        }

        public override Enums.Response ProcessPacket(LightingConsole console, ConnectionHandler handler, int type)
        {
            if (clear)
            {
                console.Stromkreise = new SK[console.SKSize];
            }
            for (ushort i = 0; i < count ; i++)
            {
                ushort num = data[i];
                if(num < console.Stromkreise.Length)
                    console.Stromkreise[num] = new SK(num);
            }
            return Enums.Response.OK;
        }
    }
}
