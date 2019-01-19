﻿using SKMNET.Util;

namespace SKMNET.Client.Networking.Client
{
    public abstract class CPacket : ISendable
    {
        public abstract short Type { get; }

        public abstract byte[] GetDataToSend(LightingConsole console);
    }
}
