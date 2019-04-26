﻿namespace SKMNET.Client.Networking.Client
{
    public class MonitorEvent : Event
    {
        private readonly byte monitor;
        private const byte PARAM = 0;
        private readonly byte cmd;

        public override int GetEventInteger(LightingConsole console)
        {
            return 0x0d000000 | (monitor << 16) | (PARAM << 8) | cmd;
        }

        public MonitorEvent(byte monitor, byte cmd)
        {
            this.monitor = monitor;
            this.cmd = cmd;
        }
    }
}
