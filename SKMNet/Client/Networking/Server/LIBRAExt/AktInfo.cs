﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Server.LIBRAExt
{
    /// <summary>
    /// aktuelle Liste und Register
    /// </summary>
    [Serializable]
    public class AktInfo : SPacket
    {
        public string Register;
        public string Listenanzeige;
        /*
         * Header contains
         * ----------------
         * ushort version (=0)
         * ushort datalen (=16=8+8)
         * ----------------
         * -> useless
         * */

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            //versno
            buffer.ReadUShort();
            //len
            buffer.ReadUShort();
            Register = buffer.ReadString(8);
            Listenanzeige = buffer.ReadString(8);
            return this;
        }

        public override Enums.Response ProcessPacket(LightingConsole console, int type)
        {
            console.AktReg = Register;
            console.AktList = Listenanzeige;
            return Enums.Response.OK;
        }
    }
}
