﻿using SKMNET.Client.Stromkreise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Server.ISKMON
{
    /// <summary>
    /// SKG-Konfiguration
    /// </summary>
    [Serializable]
    public class SKGConf : SPacket
    {
        public ushort Count;
        public SKGConfEntry[] Entries;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            Count = buffer.ReadUShort();
            Entries = new SKGConfEntry[Count];
            for(int i = 0; i < Count; i++)
            {
                Entries[i] = new SKGConfEntry(buffer.ReadUShort(), buffer.ReadString(8));
            }
            return this;
        }

        public override Enums.Response ProcessPacket(LightingConsole console, int type)
        {
            foreach(SKGConfEntry entry in Entries)
            {
                SKG skg = console.Stromkreisgruppen.Find(x => x.Number == entry.SKGNum);
                if(skg is null)
                {
                    skg = new SKG(entry.SKGNum, entry.SKGName);
                    console.Stromkreisgruppen.Add(skg);
                    continue;
                }

                skg.Number = entry.SKGNum;
                skg.Name = entry.SKGName;
            }
            return Enums.Response.OK;
        }

        [Serializable]
        public struct SKGConfEntry
        {
            public ushort SKGNum;
            public string SKGName;

            public SKGConfEntry(ushort skgNum, string skgName)
            {
                SKGNum = skgNum;
                SKGName = skgName;
            }
        }
    }
}
