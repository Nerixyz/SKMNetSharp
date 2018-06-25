using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Networking.Server.T98
{
    [Serializable]
    class SKRegAttr : Header
    {
        // TODO Attrib bits = SKMON_SKATTR
        public override int HeaderLength => 6;
        public ushort start;
        public bool update; /* should display update */
        public ushort count;
        public byte[] data;

        public override Header ParseHeader(byte[] data)
        {
            start = ByteUtils.ToUShort(data, 0);
            update = ByteUtils.ToUShort(data, 2) != 0x0;
            count = ByteUtils.ToUShort(data, 4);
            this.data = new byte[count];
            for (int i = 0; i < count; i++)
            {
                this.data[i] = data[i + 6];
            }
            return this;
        }
    }
}
