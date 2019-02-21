using SKMNET.Client.Stromkreise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Server.SKMON
{
    /// <summary>
    /// Stromkreis-Attribute (1..999)
    /// </summary>
    class SkAttr : SPacket
    {

        public ushort start;
        public ushort count;
        public byte[] data;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            start = buffer.ReadUShort();
            count = buffer.ReadUShort();
            this.data = new byte[count];
            for(int i = 0; i < count; i++)
            {
                this.data[i] = buffer.ReadByte();
            }
            return this;
        }

        public override Enums.Response ProcessPacket(LightingConsole console, ConnectionHandler handler, int type)
        {
            console.ActiveSK.Clear();
            for (int i = start; i < start + count; i++)
            {
                SK sk = console.Stromkreise[i];
                if (sk != null)
                {
                    sk.Attrib = data[i - start];
                }
            }
            // TODO: optimize speed
            foreach (SK sk in console.Stromkreise)
            {
                if (sk.Attrib != 0 && sk.Number != 0)
                {
                    console.ActiveSK.Add(sk);
                }
            }
            return Enums.Response.OK;
        }
    }
}
