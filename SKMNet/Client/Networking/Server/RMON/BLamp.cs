﻿using SKMNET.Client.Tasten;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SKMNET.Client.Networking.Server.RMON
{
    /// <summary>
    /// Lampendaten fuer Bedientasten (Komplett-Telegramm)
    /// </summary>
    [Serializable]
    public class BLamp : SPacket
    {

        public Taste.LampState[] LampStates;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            LampStates = new Taste.LampState[256];
            for(int i = 0; i < 256; i++)
            {
                LampStates[i] = Enums.GetEnum<Taste.LampState>(buffer.ReadByte());
            }
            return this;
        }

        public override Enums.Response ProcessPacket(LightingConsole console, int type)
        {
            for(int i = 0; i < LampStates.Length /* =256 */; i++)
            {
                Taste taste = console.TastenManager.FindByNumber(i);
                if(taste != null)
                    taste.State = LampStates[i];
            }
            return Enums.Response.OK;
        }
    }
}
