using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Client
{
    /// <summary>
    /// Monitore selektieren
    /// </summary>
    public class MonitorSelect : CPacket
    {
        public override short Type => 28;

        private readonly ushort monitor;

        public MonitorSelect(ushort monitor)
        {
            this.monitor = monitor;
        }

        public override byte[] GetDataToSend()
        {
            return new ByteBuffer().Write((short)0).Write((short)0).Write(monitor).ToArray();
        }
    }
}
