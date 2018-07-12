using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Client
{
    class MouseEvent : Event
    {
        readonly byte moveX;
        readonly byte moveY;
        readonly byte buttons;
        public override int GetEventInteger()
        {
            return 0x02000000 | (moveX << (4 * 8)) | (moveY << (2 * 8)) | (buttons);
        }
        public MouseEvent(byte mouseX, byte mouseY, byte buttons)
        {
            this.moveX = mouseX;
            this.moveY = mouseY;
            this.buttons = buttons;
        }
    }
}
