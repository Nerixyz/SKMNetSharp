﻿using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Client
{
    public class SKAnwahl : CPacket
    {
        public override short Type => 15;
        private readonly AWType type;
        private short[] skNo;

        public override byte[] GetDataToSend()
        {
            return new ByteArrayParser().Add((short)0).Add((short)type).Add((short)skNo.Length).Add(skNo).GetArray();
        }

        public SKAnwahl(AWType type, short[] skNo)
        {
            this.type = type;
            this.skNo = skNo;
        }

        public enum AWType
        {
            Abs = 1,
            Add,
            Sub
        }
    }
}
