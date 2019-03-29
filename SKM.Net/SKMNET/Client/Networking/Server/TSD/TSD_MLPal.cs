using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SKMNET.Client.Stromkreise.ML;
using SKMNET.Util;

namespace SKMNET.Client.Networking.Server.TSD
{
    /// <summary>
    /// ML-Palettendaten
    /// </summary>
    [Serializable]
    public class TSD_MLPal : SPacket
    {

        public MLPalPrefab[] pallets;
        public bool last;
        public ushort type;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            type = buffer.ReadUShort();
            ushort count = buffer.ReadUShort();
            last = buffer.ReadUShort() != 0;
            pallets = new MLPalPrefab[count];
            for (int i = 0; i < count; i++)
            {
                pallets[i] = new MLPalPrefab(type, buffer.ReadShort(), buffer.ReadString(8));
            }
            return this;
        }

        public override Enums.Response ProcessPacket(LightingConsole console, ConnectionHandler handler, int type)
        {

            if (!console.Paletten.TryGetValue(Enums.GetEnum<MLPal.MLPalFlag>(this.type), out List<MLPal> list))
                return Enums.Response.BadCmd;

            foreach (MLPalPrefab pre in pallets)
            {
                MLPal pal = list.Find((x) => x.Number == pre.palno / 10.0);
                if(pal is null)
                {
                    pal = new MLPal(MLPal.GetFlag(pre.paltype), pre.name, pre.palno);
                    list.Add(pal);
                }
                else
                {
                    pal.Name = pre.name;
                    pal.Number = pre.palno / 10.0;
                }
            }
            return Enums.Response.OK;
        }

        //TODO: rename; replace?
        [Serializable]
        public struct MLPalPrefab
        {
            public ushort paltype;
            public short palno;
            public string name;

            public MLPalPrefab(ushort paltype, short palno, string name)
            {
                this.paltype = paltype;
                this.palno = palno;
                this.name = name;
            }

            public bool GetFlag(MLPalFlag flag) => (paltype & ((ushort)(flag))) != 0;

            public static bool GetFlag(MLPalFlag flag, ushort paltype) => (paltype & ((ushort)(flag))) != 0;

            public enum MLPalFlag
            {
                I = 0x0001,   /* I-Palette */
                F = 0x0002,   /* F-Palette */
                C = 0x0004,   /* C-Palette */
                B = 0x0008,   /* B-Palette */
                SKG = 0x0010,   /* Stromkreisgruppe */
                BLK = 0x0020,   /* Stimmung */
                DYN = 0x0040,   /* Dynamics */
                CUR_SEL = 0x0080,   /* Aktuelle Selektion */
            }
        }
    }
}
