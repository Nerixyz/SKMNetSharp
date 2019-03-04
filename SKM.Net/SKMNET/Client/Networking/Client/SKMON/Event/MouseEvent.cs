﻿namespace SKMNET.Client.Networking.Client
{
    public class MouseEvent : Event
    {
        private readonly byte moveX;
        private readonly byte moveY;
        private readonly byte buttons;
        public override int GetEventInteger()
        {
            return 0x02000000 | (moveX << 16) | (moveY << 8) | buttons;
        }
        public MouseEvent(byte mouseX, byte mouseY, byte buttons)
        {
            this.moveX = mouseX;
            this.moveY = mouseY;
            this.buttons = buttons;
        }
    }
}
