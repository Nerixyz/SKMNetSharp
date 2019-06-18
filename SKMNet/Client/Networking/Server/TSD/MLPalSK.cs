﻿using SKMNET.Client.Stromkreise;
using SKMNET.Client.Stromkreise.ML;
using System.Collections.Generic;
using static SKMNET.Util.MLUtil;

namespace SKMNET.Client.Networking.Server.TSD
{
    /// <summary>
    /// SK-Beteiligung an Paletten
    /// </summary>
    public class MLPalSK : SPacket
    {
        public ushort Palno;
        public ushort Mlpaltype;
        public bool Last;
        public ushort SkCount;
        public ushort[] SKTable;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            Palno = buffer.ReadUShort();
            Mlpaltype = buffer.ReadUShort();
            Last = buffer.ReadUShort() != 0;
            SkCount = buffer.ReadUShort();
            SKTable = new ushort[SkCount];
            for(int i = 0; i < SkCount; i++)
            {
                SKTable[i] = buffer.ReadUShort();
            }
            return this;
        }

        public override Enums.Response ProcessPacket(LightingConsole console, int type)
        {
            if (!console.Paletten.TryGetValue(Enums.GetEnum<MLPal.Flag>(Mlpaltype), out List<MLPal> list))
                return Enums.Response.BadCmd;

            Handle(list, console);

            return Enums.Response.OK;
        }

        private void Handle(List<MLPal> pals, LightingConsole console)
        {
            MLPal pal = pals.Find(x => x.PalNo == Palno);
            if (pal == null)
            {
                pal = new MLPal((MLPal.Flag)GetPalType(Mlpaltype), string.Empty, (short)Palno);
                pals.Add(pal);
            }
            else
            {
                pal.BetSk.Clear();
            }
            foreach (ushort item in SKTable)
            {
                SK sk = console.Stromkreise[item];
                if (sk is null)
                    continue;
                pal.BetSk.Add(sk);
            }
        }
    }
}
