﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Networking.Server.ISKMON
{
    class AZ : Header
    {
        public override int HeaderLength => 0;

        public ushort command;
        public ushort flags;
        public string linetext;

        public override Header ParseHeader(byte[] data)
        {
            command = ByteUtils.ToUShort(data, 0);
            flags = ByteUtils.ToUShort(data, 2);
            linetext = ByteUtils.ToString(data, 4, 48);
            return this;
        }

        public bool Angewaehlt()
        {
            return (flags & 0x0001) != 0;
        }

        public bool IST()
        {
            return command ==(ushort) Enums.Type.AZ_IST;
        }
        public bool ZIEL()
        {
            return command == (ushort)Enums.Type.AZ_ZIEL;
        }
        public bool VOR()
        {
            return command == (ushort)Enums.Type.AZ_VOR;
        }
    }
}
