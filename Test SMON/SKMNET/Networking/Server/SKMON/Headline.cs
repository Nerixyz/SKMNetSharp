using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Networking.Server.SKMON
{
    class Headline : Header
    {
        public override int HeaderLength => 0;

        public ushort farbno;
        public ushort count;
        /// <summary>
        /// Kopfzeilen-String
        /// </summary>
        public string data;

        public override Header ParseHeader(byte[] data)
        {
            farbno = BitConverter.ToUInt16(data, 0);
            count = BitConverter.ToUInt16(data, 2);
            this.data = Encoding.ASCII.GetString(data, 4, count);
            return this;
        }
    }
}
