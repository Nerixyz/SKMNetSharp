﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Server.RMON
{
    /// <summary>
    /// Multiscreen Bildschirmdaten
    /// </summary>
    public class MScreenData : SPacket
    {

        public const ushort MON_MASK = 0x000f;
        public const ushort MON_HM_FLAG = 0x8000;
        public const byte MON_MAX = 4;

        public ushort monitor;
        public ushort start;
        public ushort count;
        public ushort[] data;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            monitor = buffer.ReadUShort();
            start = buffer.ReadUShort();
            count = buffer.ReadUShort();

            this.data = new ushort[count];
            for(int i = 0; i < count; i++){
                this.data[i] = buffer.ReadUShort();
            }

            return this;
        }

        public override Enums.Response ProcessPacket(LightingConsole console, ConnectionHandler handler, int type)
        {
            console.ScreenManager.HandleData(this);
            return Enums.Response.OK;
        }
    }
}
