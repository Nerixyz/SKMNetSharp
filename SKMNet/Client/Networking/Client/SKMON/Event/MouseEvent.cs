﻿ namespace SKMNET.Client.Networking.Client.SKMON.Event
{
    public class MouseEvent : Event
    {
        private readonly byte moveX;
        private readonly byte moveY;
        private readonly byte buttons;
        public override int GetEventInteger(LightingConsole console) => 0x02000000 | (moveX << 16) | (moveY << 8) | buttons;

        public MouseEvent(byte mouseX, byte mouseY, byte buttons)
        {
            moveX = mouseX;
            moveY = mouseY;
            this.buttons = buttons;
        }
    }
}
