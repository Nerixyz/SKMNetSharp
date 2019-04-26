﻿using SKMNET.Client.Stromkreise;
using System;

namespace SKMNET.Client.Networking.Server.T98
{
    /// <summary>
    /// Stromkreisregister-Aufbau
    /// </summary>
    [Serializable]
    public class SKRegConf : SPacket
    {
        public ushort Start;
        public bool Clear; /* should clear */
        public ushort Count;
        public ushort[] Data;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            Start = buffer.ReadUShort();
            Clear = buffer.ReadUShort() != 0x0;
            Count = buffer.ReadUShort();
            Data = new ushort[Count];
            for(int i = 0; i < Count; i++) Data[i] = buffer.ReadUShort();
            
            return this;
        }

        public override Enums.Response ProcessPacket(LightingConsole console, int type)
        {
            if (Clear) console.Stromkreise = new SK[console.SKSize];
            
            for (ushort i = 0; i < Count ; i++)
            {
                ushort num = Data[i];
                if(num < console.Stromkreise.Length)
                    console.Stromkreise[num] = new SK(num);
            }
            return Enums.Response.OK;
        }
    }
}
