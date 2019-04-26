﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Server
{
    /// <summary>
    /// Bedientasten-Konfiguration
    /// </summary>
    public class BTastConf : SPacket
    {

        public BTastConfEntry[] Entries;
        public ushort Count;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            Count = buffer.ReadUShort();
            Entries = new BTastConfEntry[Count];
            for(int i = 0; i < Count; i++)
            {
                Entries[i] = new BTastConfEntry(buffer.ReadUShort(), buffer.ReadString(6));
            }
            return this;
        }

        public override Enums.Response ProcessPacket(LightingConsole console, int type)
        {
            foreach(BTastConfEntry entry in Entries)
            {
                console.TastenManager.Add(new Tasten.Taste(entry.Tastnr, entry.Name));
            }
            return Enums.Response.OK;
        }

        [Serializable]
        public struct BTastConfEntry
        {
            public ushort Tastnr { get; }
            public string Name { get; }

            public BTastConfEntry(ushort tastnr, string name)
            {
                Tastnr = tastnr;
                Name = name;
            }

            public ushort GetNumber()
            {
                return (ushort)( Tastnr & 0x3fff);
            }
        }
    }
}
