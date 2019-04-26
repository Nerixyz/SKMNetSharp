﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SKMNET.Client.Networking.Server.ISKMON
{
    /// <summary>
    /// Meldezeile
    /// </summary>
    public class Meld : SPacket
    {

        public string LineText;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            // LENGTH = { 2, 31, 1} = 34
            LineText = buffer.ReadString(31);
            buffer.ReadByte();

            return this;
        }

        public override Enums.Response ProcessPacket(LightingConsole console, int type)
        {
            console.Meldezeile = LineText;
            return Enums.Response.OK;
        }
    }
}
