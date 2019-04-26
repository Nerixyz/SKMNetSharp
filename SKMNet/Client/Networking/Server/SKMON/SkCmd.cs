﻿using System;

namespace SKMNET.Client.Networking.Server.SKMON
{
    /// <summary>
    /// Kommando, s.u.
    /// </summary>
    public class SkCmd : SPacket
    {
        public Enums.SKCmd Cmd;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            Cmd = (Enums.SKCmd)Enum.ToObject(typeof(Enums.SKCmd), buffer.ReadByte());
            return this;
        }

        public override Enums.Response ProcessPacket(LightingConsole console, int type)
        {
            //TODO we don't know PepeLaugh
            return Enums.Response.OK;
        }
    }
}
