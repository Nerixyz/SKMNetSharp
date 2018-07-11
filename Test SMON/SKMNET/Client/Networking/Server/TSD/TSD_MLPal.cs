using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SKMNET.Util;

namespace SKMNET.Client.Networking.Server.TSD
{
    [Serializable]
    class TSD_MLPal : SPacket
    {
        public override int HeaderLength => 6;

        public MLPal_Prefab[] pallets;
        public bool last;
        
        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            ushort type = buffer.ReadUShort();
            ushort count = buffer.ReadUShort();
            last = buffer.ReadUShort() != 0;
            pallets = new MLPal_Prefab[count];
            for (int i = 0; i < count; i++)
            {
                pallets[i] = new MLPal_Prefab(type, buffer.ReadShort(), buffer.ReadString(8));
            }
            return this;
        }
    }
}
