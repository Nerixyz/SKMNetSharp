﻿using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SKMNET.Client.Networking.Server.ISKMON
{
    /// <summary>
    /// Aktuellzeile
    /// </summary>
    public class Az : SPacket
    {

        public ushort Flags;
        public string LineText;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            Flags = buffer.ReadUShort();
            LineText = buffer.ReadString(48);
            return this;
        }

        public bool Angewaehlt => (Flags & 0x0001) != 0;

        public override Enums.Response ProcessPacket(LightingConsole console, int type)
        {
            switch(Enums.GetEnum<Enums.Type>(type))
            {
                case Enums.Type.AZ_IST: console.RegIST.Text = LineText; console.RegIST.Aw = Angewaehlt; break;
                case Enums.Type.AZ_ZIEL: console.RegZIEL.Text = LineText; console.RegZIEL.Aw = Angewaehlt; break;
                case Enums.Type.AZ_VOR: console.RegVOR.Text = LineText; console.RegVOR.Aw = Angewaehlt; break;
                default: throw new ArgumentException("type != AZ");
            }

            return Enums.Response.OK;
        }
    }
}
