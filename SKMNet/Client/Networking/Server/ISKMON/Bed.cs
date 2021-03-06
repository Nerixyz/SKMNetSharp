﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SKMNET.Client.Networking.Server.ISKMON
{
    /// <summary>
    /// Bedienzeile
    /// </summary>
    [Serializable]
    public class Bed : SPacket
    {

        public string LineText;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            // LENGTH = { 2, 31, 1} = 34
            LineText = buffer.ReadString(31);
            //unused
            buffer.ReadByte();

            return this;
        }

        public override Enums.Response ProcessPacket(LightingConsole console, int type)
        {
            console.Bedienzeile = LineText;
            return Enums.Response.OK;
        }
    }
}
