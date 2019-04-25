﻿using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Server.TSD
{
    /// <summary>
    /// DMX-orientierte Kreiswerte
    /// </summary>
    public class DMXData : SPacket
    {

        public ushort count; /* 1 or 2 lines */
        public DMXDataEntry[] dmxLines; /* 1 or 2 line data */

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            count = buffer.ReadUShort();
            dmxLines = new DMXDataEntry[count];
            for(int i = 0; i < count; i++)
            {
                ushort line = buffer.ReadUShort();
                byte[] dmxData = buffer.ReadByteArray(512);
                dmxLines[i] = new DMXDataEntry(line, dmxData);
            }
            return this;
        }

        public override Enums.Response ProcessPacket(LightingConsole console, ConnectionHandler handler, int type)
        {
            //TODO actually idk where each SK is located
            return Enums.Response.OK;
        }

        public struct DMXDataEntry
        {
            public ushort line; /* DMX-Leitungsnummer 1..64 */
            public byte[] dmxData;

            public DMXDataEntry(ushort line, byte[] dmxData)
            {
                this.line = line;
                this.dmxData = dmxData;
            }
        }
    }
}
