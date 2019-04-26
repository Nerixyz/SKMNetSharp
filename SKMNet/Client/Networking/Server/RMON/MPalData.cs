﻿using SKMNET.Client.Rendering;
using System.Collections.Generic;
 
namespace SKMNET.Client.Networking.Server.RMON
{
    /// <summary>
    /// Video-Multiscreen Palettendaten
    /// </summary>
    public class MPalData : SPacket
    {

        public readonly List<VideoFarbe> farbeintrag;
        public ushort Monitor;
        // siehe MScreenData.cs

        private const int N_HW_PALETTE = 64;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            Monitor = buffer.ReadUShort();
            for (int i = 0; i < N_HW_PALETTE; i++)
            {
                farbeintrag.Add(
                    new VideoFarbe(
                        buffer.ReadShort(),
                        buffer.ReadByte(),
                        buffer.ReadByte(),
                        buffer.ReadByte(),
                        buffer.ReadByte(),
                        buffer.ReadByte(),
                        buffer.ReadByte()
                        )
                    );
            }
            return this;
        }

        public override Enums.Response ProcessPacket(LightingConsole console, int type)
        {
            //TODO MonitorHandler
            console.ScreenManager.HandleData(this);
            return Enums.Response.OK;
        }

        public MPalData() => farbeintrag = new List<VideoFarbe>(64);
    }
}
