﻿using SKMNET.Client.Stromkreise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Server.SKMON
{
    /// <summary>
    /// Stromkreiswerte (1..999)
    /// </summary>
    public class SkData : SPacket
    {
        public ushort Start;
        public ushort Count;
        public byte[] Data;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            Start = buffer.ReadUShort();
            Count = buffer.ReadUShort();
            Data = new byte[Count];
            for(int i = 0; i < Count; i++)
            {
                Data[i] = buffer.ReadByte();
            }
            return this;
        }

        public override Enums.Response ProcessPacket(LightingConsole console, int type)
        {
            for (int i = Start; i < Start + Count; i++) console.Stromkreise[i]?.SetDimmer(Data[i - Start]);
            return Enums.Response.OK;
        }
    }
}
