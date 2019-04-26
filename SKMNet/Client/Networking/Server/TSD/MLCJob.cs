﻿using SKMNET.Client.Vorstellungen;
using System;

namespace SKMNET.Client.Networking.Server.TSD
{
    /// <summary>
    /// einfache Jobkommandos an MLC
    /// </summary>
    [Serializable]
    public class MLCJob : SPacket
    {

        public ushort Job;
        public uint VstNum;
        /// <summary>
        /// 0 = normal; 'detect' changed <see cref="VstNum"/>
        /// 1 = force; reload MLCConfig
        /// </summary>
        public uint Modus;
        public uint Par3;
        public ushort Res1;
        public ushort Count;
        public string Buf;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            Job = buffer.ReadUShort();
            VstNum = buffer.ReadUInt();
            Modus = buffer.ReadUInt();
            Par3 = buffer.ReadUInt();
            Res1 = buffer.ReadUShort();
            Count = buffer.ReadUShort();
            Buf = buffer.ReadString(Count);
            return this;
        }

        public bool Load => Job == 1;
        public bool Save => Job == 2;

        public override Enums.Response ProcessPacket(LightingConsole console, int type)
        {
            Vorstellung vst = console.Vorstellungen.Find(x => VstNum == x.Number);
            if (!(vst is null)) return Enums.Response.OK;
            
            vst = new Vorstellung((ushort)VstNum);
            console.Vorstellungen.Add(vst);
            return Enums.Response.OK;
        }
    }
}
