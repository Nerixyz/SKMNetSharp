﻿using System;

namespace SKMNET.Client.Networking.Client
{
    public class TextTastEvent : Event
    {
        readonly byte state;
        readonly short scancode;

        // TODO make Enum
        public TextTastEvent(short scancode, byte state = 0)
        {
            this.state = state;
            this.scancode = scancode;
        }

        public override int GetEventInteger()
        {
            byte[] data = BitConverter.GetBytes(scancode);
            Array.Reverse(data);
            return 0x07000000 | (state << 16) | (data[0] << 8) | data[1];
        }
    }
}