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

        public MLPal_Prefab[] pallets;
        public bool last;
        public ushort type;
        
        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            type = buffer.ReadUShort();
            ushort count = buffer.ReadUShort();
            last = buffer.ReadUShort() != 0;
            pallets = new MLPal_Prefab[count];
            for (int i = 0; i < count; i++)
            {
                pallets[i] = new MLPal_Prefab(type, buffer.ReadShort(), buffer.ReadString(8));
            }
            return this;
        }

        public override Enums.Response ProcessPacket(LightingConsole console, ConnectionHandler handler, int type)
        {

            if (!console.Paletten.TryGetValue(Enums.GetEnum<MLPal.MLPalFlag>(type), out List<MLPal> list))
                return Enums.Response.BadCmd;

            foreach (MLPal_Prefab pre in pallets)
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
    }
}
