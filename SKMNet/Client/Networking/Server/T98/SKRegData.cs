﻿using SKMNET.Client.Stromkreise;
using System;

namespace SKMNET.Client.Networking.Server.T98
{
    /// <summary>
    /// SK-Werte in Stromkreisregister-Order
    /// </summary>
    [Serializable]
    public class SKRegData : SPacket
    {
        public ushort Start;
        public bool Update; /* should display update */
        public ushort Count;

        [NonSerialized]
        public byte[] Data;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            Start = buffer.ReadUShort();
            Update = buffer.ReadUShort() != 0x0;
            Count = buffer.ReadUShort();
            Data = new byte[Count];
            for (int i = 0; i < Count; i++)
            {
                Data[i] = buffer.ReadByte();
            }
            return this;
        }

        public override Enums.Response ProcessPacket(LightingConsole console, int type)
        {
            for (int i = Start; i < Start + Count; i++)
            {
                if (i >= console.Stromkreise.Length)
                    break;

                SK reg = console.Stromkreise[i];
                reg?.SetDimmer(Data[i - Start]);
            }
            return Enums.Response.OK;
        }
    }
}
