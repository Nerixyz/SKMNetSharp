﻿using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Client
{
    /// <summary>
    /// SKG-Anwahl-Telegramm
    /// </summary>
    public class SKGAnwahl : CPacket
    {
        public override short Type => 16;

        private readonly AWType action;
        private readonly short[] SKGs;

        public override byte[] GetDataToSend(LightingConsole console)
        {
            return new ByteBuffer().WriteShort(console.BdstNo).WriteShort((short)action).WriteShort((short)SKGs.Length).Write(SKGs).ToArray();
        }

        public SKGAnwahl(AWType type, short[] SKGs)
        {
            this.action = type;
            this.SKGs = SKGs;
        }


        public enum AWType
        {
            Abs,
            Add,
            Substract
        }
    }
}
