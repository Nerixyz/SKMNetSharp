﻿using SKMNET.Client.Stromkreise;
using System.Linq;
using static SKMNET.Enums;

namespace SKMNET.Client.Networking.Client
{
    /// <summary>
    /// SKG-Anwahl-Telegramm
    /// </summary>
    public class SKGAnwahl : CPacket
    {
        public override short Type => 16;

        private readonly AWType action;
        private readonly short[] skgs;

        public override byte[] GetDataToSend(LightingConsole console)
        {
            return new ByteBuffer().WriteShort(console.BdstNo).WriteShort((short)action).WriteShort((short)skgs.Length).Write(skgs).ToArray();
        }

        public SKGAnwahl(AWType type, params short[] skgs)
        {
            action = type;
            this.skgs = skgs;
        }

        public SKGAnwahl(AWType type, params SKG[] SKGs)
        {
            action = type;
            skgs = SKGs.Select(skg => (short)skg.Number).ToArray();
        }
    }
}
