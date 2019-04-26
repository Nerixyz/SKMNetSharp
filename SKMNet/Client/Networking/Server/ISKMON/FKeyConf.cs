﻿using System;

namespace SKMNET.Client.Networking.Server.ISKMON
{
    /// <summary>
    /// Funktionstasten-Konfiguration
    /// </summary>
    [Serializable]
    public class FKeyConf : SPacket
    {

        public ushort Count;
        public FKeyConfEntry[] Entries;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            Count = buffer.ReadUShort();
            Entries = new FKeyConfEntry[Count];
            for(int i = 0; i < Count; i++)
            {
                Entries[i] = new FKeyConfEntry(buffer.ReadUShort(), buffer.ReadString(22));
            }
            return this;
        }

        public override Enums.Response ProcessPacket(LightingConsole console, int type)
        {
            //TODO impl
            return Enums.Response.OK;
        }

        [Serializable]
        public struct FKeyConfEntry
        {
            public ushort FKeyNo;
            public string Label;

            public FKeyConfEntry(ushort fKeyNo, string label)
            {
                FKeyNo = fKeyNo;
                Label = label;
            }
        }
    }
}
