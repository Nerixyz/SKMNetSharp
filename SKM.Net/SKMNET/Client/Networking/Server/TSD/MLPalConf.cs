using SKMNET.Client.Stromkreise.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Server.TSD
{
    /// <summary>
    /// Palettenkonfiguration mit langen Namen
    /// </summary>
    [Serializable]
    public class MLPalConf : SPacket
    {

        public bool absolute; /* Should Update the whole configuration */
        public ushort MLPalType { get; set; }/* MLPalFlag */
        public bool last;

        public List<ConfEntry> Entries { get; } = new List<ConfEntry>();

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            absolute = buffer.ReadUShort() == 0;
            MLPalType = buffer.ReadUShort();
            last = buffer.ReadUShort() != 0;
            while(true)
            {
                short palno = buffer.ReadShort();
                short length = buffer.ReadShort();

                if (palno == 0 && length == 0)
                    break;

                string text = buffer.ReadString(length);
                Entries.Add(new ConfEntry(palno, text));
            }
            return this;
        }

        public override Enums.Response ProcessPacket(LightingConsole console, ConnectionHandler handler, int type)
        {
            MLPal.MLPalFlag mlType = MLPal.GetFlag(MLPalType);
            if (!console.Paletten.TryGetValue(mlType, out List<MLPal> list))
                return Enums.Response.BadCmd;

            if (absolute)
                list.Clear();

            foreach(ConfEntry entry in Entries) list.Add(new MLPal(mlType, entry.Text, entry.Palno));

            return Enums.Response.OK;
        }

        [Serializable]
        public struct ConfEntry
        {
            public short Palno { get; set; }
            public string Text { get; set; }

            public ConfEntry(short palno, string text)
            {
                this.Palno = palno;
                this.Text = text;
            }
        }
    }
}
