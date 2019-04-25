﻿using System;

namespace SKMNET.Client.Networking.Client
{
    public class BedienfehlermeldungEvent : Event
    {
        private readonly byte bdst;
        private readonly short fehlNo;

        // TODO make Enum
        public BedienfehlermeldungEvent(short fehlNo, byte bdst = 0)
        {
            this.bdst = bdst;
            this.fehlNo = fehlNo;
        }

        public override int GetEventInteger(LightingConsole console)
        {
            byte[] data = BitConverter.GetBytes(fehlNo);
            Array.Reverse(data);
            return 0x05000000 | (bdst << 16) | (data[0] << 8) | data[1];
        }
    }
}
