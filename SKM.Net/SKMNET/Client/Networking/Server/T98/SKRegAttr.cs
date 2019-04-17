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
    /// SK-Attr. in Stromkreisregister-Order
    /// </summary>
    [Serializable]
    public class SKRegAttr : SPacket
    {
        // TODO Attrib bits = SKMON_SKATTR

        public ushort start;
        public bool update; /* should display update */
        public ushort count;
        public byte[] data;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            start = buffer.ReadUShort();
            update = buffer.ReadUShort() != 0x0;
            count = buffer.ReadUShort();
            this.data = new byte[count];
            for (int i = 0; i < count; i++)
            {
                this.data[i] = buffer.ReadByte();
            }
            return this;
        }

        public override Enums.Response ProcessPacket(LightingConsole console, ConnectionHandler handler, int type)
        {
            for (int i = start; i < start + count; i++)
            {
                if (i >= console.Stromkreise.Length)
                    break;

                SK sk = console.Stromkreise[i];
                if (sk != null)
                {
                    sk.Attrib = data[i - start];
                }
            }
            return Enums.Response.OK;
        }
    }
}
