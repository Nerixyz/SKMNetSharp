﻿using System;
using System.Collections.Generic;
using SKMNET.Client.Stromkreise.ML;

namespace SKMNET.Client.Networking.Server.TSD
{
    /// <summary>
    /// ML-Palettendaten
    /// </summary>
    [Serializable]
    public class TSD_MLPal : SPacket
    {

        public MLPalPrefab[] Pallets;
        public bool Last;
        public ushort Type;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            Type = buffer.ReadUShort();
            ushort count = buffer.ReadUShort();
            Last = buffer.ReadUShort() != 0;
            Pallets = new MLPalPrefab[count];
            for (int i = 0; i < count; i++)
            {
                Pallets[i] = new MLPalPrefab(Type, buffer.ReadShort(), buffer.ReadString(8));
            }
            return this;
        }

        public override Enums.Response ProcessPacket(LightingConsole console, int type)
        {

            if (!console.Paletten.TryGetValue(Enums.GetEnum<MLPal.Flag>(Type), out List<MLPal> list))
                return Enums.Response.BadCmd;

            foreach (MLPalPrefab pre in Pallets)
            {
                MLPal pal = list.Find(x => x.Number == pre.palno / 10.0);
                if(pal is null)
                {
                    pal = new MLPal(MLPal.GetFlag(pre.paltype), pre.Name, pre.palno);
                    list.Add(pal);
                }
                else
                {
                    pal.Name = pre.Name;
                    pal.Number = pre.palno / 10.0;
                }
            }
            return Enums.Response.OK;
        }

        //TODO: rename; replace?
        [Serializable]
        public struct MLPalPrefab
        {
            public readonly ushort paltype;
            public readonly short palno;
            public string Name;

            public MLPalPrefab(ushort paltype, short palno, string name)
            {
                this.paltype = paltype;
                this.palno = palno;
                Name = name;
            }
            
            //TODO remove?
            public bool GetFlag(MLPalFlag flag) => (paltype & (ushort)flag) != 0;

            public static bool GetFlag(MLPalFlag flag, ushort paltype) => (paltype & (ushort)flag) != 0;

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
