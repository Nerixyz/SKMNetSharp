using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SKMNET.Util;

namespace SKMNET.Networking.Server.TSD
{
    class TSD_MLPal : Header
    {
        public override int HeaderLength => 8;

        public ushort command; /* = SKMON_TSD_MLPAL */
        public MLPal[] pallets;
        public bool last;
        
        public override Header ParseHeader(byte[] data)
        {
            ushort type = BitConverter.ToUInt16(data, 2);
            ushort count = BitConverter.ToUInt16(data, 6);
            last = BitConverter.ToUInt16(data, 4) != 0;
            pallets = new MLPal[count];
            for (int i = 0; i < count; i++)
            {
                pallets[i] = new MLPal(type, BitConverter.ToInt16(data, i * 10 + 8), Encoding.ASCII.GetString(data, i * 10 + 10, 8));
            }
            return this;
        }
    }
}
