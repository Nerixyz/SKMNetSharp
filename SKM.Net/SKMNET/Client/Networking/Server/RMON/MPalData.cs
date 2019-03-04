﻿using SKMNET.Client.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Server.RMON
{
    /// <summary>
    /// Video-Multiscreen Palettendaten
    /// </summary>
    public class MPalData : SPacket
    {

        public VideoFarbe[] farbeintrag;
        public ushort monitor;
        // siehe MScreenData.cs

        private const int N_HW_PALETTE = 64;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            monitor = buffer.ReadUShort();
            for (int i = 0; i < N_HW_PALETTE; i++)
            {
                VideoFarbe eintrag = farbeintrag[i];
                eintrag = new VideoFarbe(buffer.ReadShort(), buffer.ReadByte(), buffer.ReadByte(), buffer.ReadByte(), buffer.ReadByte(), buffer.ReadByte(), buffer.ReadByte());
            }
            return this;
        }

        public override Enums.Response ProcessPacket(LightingConsole console, ConnectionHandler handler, int type)
        {
            //TODO MonitorHandler
            return Enums.Response.OK;
        }

        public MPalData()
        {
            farbeintrag = new VideoFarbe[64];
        }
    }
}