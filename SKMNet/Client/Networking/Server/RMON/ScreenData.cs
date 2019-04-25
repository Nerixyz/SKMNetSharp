﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Server
{
    /// <summary>
    /// Bildschirmdaten
    /// </summary>
    public class ScreenData : SPacket
    {

        /// <summary>
        /// Maximale Anzahl von Bildschirmdaten (legacy)
        /// </summary>
        public static readonly int MAX_DATA = 733;
        /// <summary>
        /// Position im Bildschirm. Links oben entspricht Position 0.
        /// </summary>
        public ushort start;
        /// <summary>
        /// Anzahl der folgenden Bildschirm Daten length. (legacy)
        /// </summary>
        public ushort count;
        /// <summary>
        /// Bildschirmdaten (Bit 15..8 Attribut, Bit 7..0 Zeichen.)
        /// </summary>
        public ushort[] data;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            this.start = buffer.ReadUShort();
            this.count = buffer.ReadUShort();
            this.data = new ushort[count];
            for (int i = 0; i < count; i++)
            {
                this.data[i] = buffer.ReadUShort();
            }
            return this;
        }

        public override Enums.Response ProcessPacket(LightingConsole console, ConnectionHandler handler, int type)
        {
            console.ScreenManager.HandleData(this);
            return Enums.Response.OK;
        }
    }
}
