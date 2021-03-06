﻿using System;

namespace SKMNET.Client.Networking.Client.SKMON.Event
{
    public class LastkreistasteEvent : Event
    {
        private readonly short lkno;

        // TODO Enum?
        public LastkreistasteEvent(short lkno)
        {
            this.lkno = lkno;
        }

        public override int GetEventInteger(LightingConsole console)
        {
            byte[] data = BitConverter.GetBytes(lkno);
            data.TransformToBigEndian();
            return 0x05000000 | ((byte)console.BdstNo << (4 * 8)) | (data[0] << 2 * 8) | data[1];
        }
    }
}
