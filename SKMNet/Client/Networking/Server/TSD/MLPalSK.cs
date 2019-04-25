﻿using SKMNET.Client.Stromkreise;
using SKMNET.Client.Stromkreise.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SKMNET.Util.MLUtil;

namespace SKMNET.Client.Networking.Server.TSD
{
    /// <summary>
    /// SK-Beteiligung an Paletten
    /// </summary>
    public class MLPalSK : SPacket
    {
        public ushort palno;
        public ushort mlpaltype;
        public bool last;
        public ushort skcount;
        public ushort[] skTable;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            palno = buffer.ReadUShort();
            mlpaltype = buffer.ReadUShort();
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
            if (!console.Paletten.TryGetValue(Enums.GetEnum<MLPal.Flag>(mlpaltype), out List<MLPal> list))
                return Enums.Response.BadCmd;

            Handle(list, console);

            return Enums.Response.OK;
        }

        private void Handle(List<MLPal> pals, LightingConsole console)
        {
            MLPal pal = pals.Find((x) => x.Number == palno);
            if (pal == null)
            {
                pal = new MLPal((MLPal.Flag)GetPalType(mlpaltype), string.Empty, (short)palno);
                pals.Add(pal);
            }
            else
            {
                pal.BetSK.Clear();
            }
            foreach (ushort item in skTable)
            {
                SK sk = console.Stromkreise[item];
                if (sk is null)
                    continue;
                pal.BetSK.Add(sk);
            }
        }
    }
}
