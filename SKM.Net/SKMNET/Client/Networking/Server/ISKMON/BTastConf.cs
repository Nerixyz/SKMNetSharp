using System;
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

        public BTastConfEntry[] entries;
        public ushort count;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            count = buffer.ReadUShort();
            entries = new BTastConfEntry[count];
            for(int i = 0; i < count; i++)
            {
                entries[i] = new BTastConfEntry(buffer.ReadUShort(), buffer.ReadString(6));
            }
            return this;
        }

        public override Enums.Response ProcessPacket(LightingConsole console, ConnectionHandler handler, int type)
        {
            foreach(BTastConfEntry entry in entries)
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
                this.Tastnr = tastnr;
                this.Name = name;
            }

            public ushort GetNumber()
            {
                return (ushort)( Tastnr & 0x3fff);
            }
        }
    }
}
