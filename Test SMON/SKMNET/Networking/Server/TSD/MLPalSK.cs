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
            command = BitConverter.ToUInt16(data, 0);
            palno = BitConverter.ToUInt16(data, 2);
            mpaltype = BitConverter.ToUInt16(data, 4);
            last = BitConverter.ToUInt16(data, 6) != 0;
            skcount = BitConverter.ToUInt16(data, 8);
            skTable = new ushort[skcount];
            for(int i = 0; i < skcount; i++)
            {
                skTable[i] = BitConverter.ToUInt16(data, i * 2 + HeaderLength);
            }
            return this;
        }
    }
}
