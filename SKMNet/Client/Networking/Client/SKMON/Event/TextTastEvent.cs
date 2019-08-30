using System;

namespace SKMNET.Client.Networking.Client.SKMON.Event
{
    public class TextTastEvent : Event
    {
        private readonly byte state;
        private readonly short scancode;

        public TextTastEvent(short scancode, byte state = 0)
        {
            this.state = state;
            this.scancode = scancode;
        }

        public override int GetEventInteger(LightingConsole console)
        {
            byte[] data = BitConverter.GetBytes(scancode);
            data.TransformToBigEndian();
            return 0x07000000 | (state << 16) | (data[0] << 8) | data[1];
        }
    }
}
