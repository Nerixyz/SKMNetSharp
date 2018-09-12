using SKMNET.Client.Stromkreise;
using SKMNET.Client.Stromkreise.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SKMNET.Util.MLUtil;

namespace SKMNET.Client.Networking.Server.TSD
{
    class MLPalSK : SPacket
    {
        public override int HeaderLength => 10;
        
        public ushort palno;
        public ushort mpaltype;
        public bool last;
        public ushort skcount;
        public ushort[] skTable;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
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

        public override Enums.Response ProcessPacket(LightingConsole console, ConnectionHandler handler, int type)
        {
            switch (GetPalType(mpaltype))
            {
                case MLPalFlag.I: Handle(console.IPal, console); break;
                case MLPalFlag.F: Handle(console.FPal, console); break;
                case MLPalFlag.C: Handle(console.CPal, console); break;
                case MLPalFlag.B: Handle(console.BPal, console); break;
                default: break;
            }
            return Enums.Response.OK;
        }

        private void Handle(List<MLPal> pals, LightingConsole console)
        {
            MLPal pal = pals.Find((x) => x.Number == palno);
            if (pal == null)
            {
                pal = new MLPal((MLPal.MLPalFlag)GetPalType(mpaltype), string.Empty, (short)palno);
                pals.Add(pal);
            }
            else
            {
                pal.BetSK.Clear();
            }
            foreach (ushort item in skTable)
            {
                SK sk = console.Stromkreise.Find((x) => x.Number == item);
                if (sk is null)
                    continue;
                pal.BetSK.Add(sk);
            }
        }
    }
}
