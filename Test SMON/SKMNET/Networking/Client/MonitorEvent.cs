using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Networking.Client
{
    class MonitorEvent : Event
    {
        byte monitor;
        byte param;
        byte cmd;

        public override int GetEventInteger()
        {
            return 0x0d000000 | (monitor << (3 * 8)) | (param << (2 * 8)) | cmd;
        }

        public MonitorEvent(byte monitor, byte cmd)
        {
            this.monitor = monitor;
            this.cmd = cmd;
        }
    }
}
