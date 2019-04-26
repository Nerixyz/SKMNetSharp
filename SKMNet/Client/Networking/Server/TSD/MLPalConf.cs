﻿using SKMNET.Client.Stromkreise.ML;
using System;
using System.Collections.Generic;

namespace SKMNET.Client.Networking.Server.TSD
{
    /// <summary>
    /// Palettenkonfiguration mit langen Namen
    /// </summary>
    [Serializable]
    public class MLPalConf : SPacket
    {

        public bool Absolute; /* Should Update the whole configuration */
        public ushort MlPalType { get; set; }/* MLPalFlag */
        public bool Last;

        public List<ConfEntry> Entries { get; } = new List<ConfEntry>();

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            Absolute = buffer.ReadUShort() == 0;
            MlPalType = buffer.ReadUShort();
            Last = buffer.ReadUShort() != 0;
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

        public override Enums.Response ProcessPacket(LightingConsole console, int type)
        {
            MLPal.Flag mlType = MLPal.GetFlag(MlPalType);
            if (!console.Paletten.TryGetValue(mlType, out List<MLPal> list))
                return Enums.Response.BadCmd;

            if (Absolute)
                list.Clear();

            foreach(ConfEntry entry in Entries) list.Add(new MLPal(mlType, entry.Text, entry.Palno));

            return Enums.Response.OK;
        }

        [Serializable]
        public struct ConfEntry
        {
            public short Palno { get; }
            public string Text { get; }

            public ConfEntry(short palno, string text)
            {
                Palno = palno;
                Text = text;
            }
        }
    }
}
