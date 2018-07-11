using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SKMNET.Util;

namespace SKMNET.Networking.Server.TSD
{
    [Serializable]
    class TSD_MLPal : SPacket
    {
        public override int HeaderLength => 6;
       
        public MLPal_Prefab[] pallets;
        public bool last;
        
        public override SPacket ParseHeader(byte[] data)
        {
            ushort type = ByteUtils.ToUShort(data, 0);
            ushort count = ByteUtils.ToUShort(data, 4);
            last = ByteUtils.ToUShort(data, 2) != 0;
            pallets = new MLPal_Prefab[count];
            for (int i = 0; i < count; i++)
            {
                pallets[i] = new MLPal_Prefab(type, ByteUtils.ToShort(data, i * 10 + 6), ByteUtils.ToString(data, i * 10 + 8, 8));
            }
            return this;
        }
    }
}
