﻿using System;

namespace SKMNET.Client.Networking.Server.SKMON
{
    /// <summary>
    /// Konfigurationsdaten
    /// </summary>
    [Serializable]
    public class Conf : SPacket
    {

        public Enums.OVDisp[] Disp { get; private set; }

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            ushort count = buffer.ReadUShort();
            Disp = new Enums.OVDisp[count];
            for(int i = 0; i < count; i++)
            {
                Disp[i] = Enums.GetEnum<Enums.OVDisp>(buffer.ReadUShort());
            }
            return this;
        }

        public override Enums.Response ProcessPacket(LightingConsole console, int type)
        {
            console.DisplayMode = Disp[0];
            return Enums.Response.OK;
        }
    }
}
