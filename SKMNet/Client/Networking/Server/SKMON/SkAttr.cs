﻿using SKMNET.Client.Stromkreise;
 
namespace SKMNET.Client.Networking.Server.SKMON
{
    /// <summary>
    /// Stromkreis-Attribute (1..999)
    /// </summary>
    public class SkAttr : SPacket
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
            if (Start >= console.SKSize) //ignore sks
                return Enums.Response.OK;
            
            for (int i = Start; i < Start + Count; i++)
            {
                SK sk = console.Stromkreise[i];
                
                if (sk != null) sk.Attrib = Data[i - Start];
            }
            return Enums.Response.OK;
        }
    }
}
