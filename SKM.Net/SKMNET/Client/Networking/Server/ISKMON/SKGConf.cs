using SKMNET.Client.Stromkreise;
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
        public ushort count;
        public SKGConfEntry[] entries;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            count = buffer.ReadUShort();
            entries = new SKGConfEntry[count];
            for(int i = 0; i < count; i++)
            {
                entries[i] = new SKGConfEntry(buffer.ReadUShort(), buffer.ReadString(8));
            }
            return this;
        }

        public override Enums.Response ProcessPacket(LightingConsole console, ConnectionHandler handler, int type)
        {
            foreach(SKGConfEntry entry in entries)
            {
                SKG skg = console.Stromkreisgruppen.Find((x) => x.Number == entry.skgnum);
                if(skg is null)
                {
                    skg = new SKG(entry.skgnum, entry.skgname);
                    console.Stromkreisgruppen.Add(skg);
                    continue;
                }
                else
                {
                    skg.Number = entry.skgnum;
                    skg.Name = entry.skgname;
                }
            }
            return Enums.Response.OK;
        }

        [Serializable]
        public struct SKGConfEntry
        {
            public ushort skgnum;
            public string skgname;

            public SKGConfEntry(ushort skgnum, string skgname)
            {
                this.skgnum = skgnum;
                this.skgname = skgname;
            }
        }
    }
}
