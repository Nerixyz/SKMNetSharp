using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Networking.Server.TSD
{
    class MLPalSK : Header
    {
        public override int HeaderLength => 10;

        public ushort command;
        public ushort palno;
        public ushort mpaltype;
        public bool last;
        public ushort skcount;
        public ushort[] skTable;

        public override Header ParseHeader(byte[] data)
        {
            command = ByteUtils.ToUShort(data, 0);
            palno = ByteUtils.ToUShort(data, 2);
            mpaltype = ByteUtils.ToUShort(data, 4);
            last = ByteUtils.ToUShort(data, 6) != 0;
            skcount = ByteUtils.ToUShort(data, 8);
            skTable = new ushort[skcount];
            for(int i = 0; i < skcount; i++)
            {
                skTable[i] = ByteUtils.ToUShort(data, i * 2 + HeaderLength);
            }
            return this;
        }
    }
}
