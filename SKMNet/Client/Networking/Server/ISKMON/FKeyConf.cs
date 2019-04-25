﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Server.ISKMON
{
    /// <summary>
    /// Funktionstasten-Konfiguration
    /// </summary>
    [Serializable]
    public class FKeyConf : SPacket
    {

        public ushort count;
        public FKeyConfEntry[] entries;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            count = buffer.ReadUShort();
            entries = new FKeyConfEntry[count];
            for(int i = 0; i < count; i++)
            {
                entries[i] = new FKeyConfEntry(buffer.ReadUShort(), buffer.ReadString(22));
            }
            return this;
        }

        public override Enums.Response ProcessPacket(LightingConsole console, ConnectionHandler handler, int type)
        {
            //TODO
            return Enums.Response.OK;
        }

        [Serializable]
        public struct FKeyConfEntry
        {
            public ushort fkeynr;
            public string label;

            public FKeyConfEntry(ushort fkeynr, string label)
            {
                this.fkeynr = fkeynr;
                this.label = label;
            }
        }
    }
}
