
namespace SKMNET.Client.Networking.Client
{
    public class MonitorEvent : Event
    {
        readonly byte monitor;
        readonly byte param  = 0;
        readonly byte cmd;

        public override int GetEventInteger()
        {
            return 0x0d000000 | (monitor << (16)) | (param << (8)) | cmd;
        }

        public MonitorEvent(byte monitor, byte cmd)
        {
            this.monitor = monitor;
            this.cmd = cmd;
        }
    }
}
