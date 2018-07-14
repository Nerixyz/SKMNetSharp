using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SKMNET.Client.Stromkreise.ML;
using SKMNET.Util;

namespace SKMNET.Client.Networking.Server.TSD
{
    [Serializable]
    class TSD_MLPal : SPacket
    {
        public override int HeaderLength => 6;

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
            List<MLPal> listPtr;
            if (MLPal_Prefab.GetFlag(MLPal_Prefab.MLPalFlag.I, this.type)) listPtr = console.IPal;
            if (MLPal_Prefab.GetFlag(MLPal_Prefab.MLPalFlag.F, this.type)) listPtr = console.FPal;
            if (MLPal_Prefab.GetFlag(MLPal_Prefab.MLPalFlag.C, this.type)) listPtr = console.CPal;
            if (MLPal_Prefab.GetFlag(MLPal_Prefab.MLPalFlag.B, this.type)) listPtr = console.BPal;
            if (MLPal_Prefab.GetFlag(MLPal_Prefab.MLPalFlag.BLK, this.type)) listPtr = console.BLK;
            else return Enums.Response.OK; // TODO inspection
            foreach (MLPal_Prefab pre in pallets)
            {
                MLPal pal = listPtr.Find((x) => x.Number == pre.palno / 10.0);
                if(pal is null)
                {
                    pal = new MLPal(MLPal.GetFlag(pre.paltype), pre.name, pre.palno);
                    listPtr.Add(pal);
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
