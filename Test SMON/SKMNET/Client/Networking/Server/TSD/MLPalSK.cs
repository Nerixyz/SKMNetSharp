using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Server.TSD
{
    class MLPalSK : SPacket
    {
        public override int HeaderLength => 10;

        public ushort command;
        public ushort palno;
        public ushort mpaltype;
        public bool last;
        public ushort skcount;
        public ushort[] skTable;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            command = buffer.ReadUShort();
            palno = buffer.ReadUShort();
            mpaltype = buffer.ReadUShort();
            last = buffer.ReadUShort() != 0;
            skcount = buffer.ReadUShort();
            skTable = new ushort[skcount];
            for(int i = 0; i < skcount; i++)
            {
                skTable[i] = buffer.ReadUShort();
            }
            return this;
        }
    }
}
