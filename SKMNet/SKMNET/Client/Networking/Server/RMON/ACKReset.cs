﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Server.RMON
{
    class ACKReset : SPacket
    {
        public override int HeaderLength => throw new NotImplementedException();

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            return this;
        }

        public override Enums.Response ProcessPacket(LightingConsole console, ConnectionHandler handler, int type)
        {
            //TODO
            return Enums.Response.OK;
        }
    }
}