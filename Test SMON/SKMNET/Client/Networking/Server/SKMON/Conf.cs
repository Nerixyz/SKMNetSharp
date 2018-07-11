using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Networking.Server.SKMON
{
    [Serializable]
    class Conf : SPacket
    {
        public override int HeaderLength => 0;

        public List<Enums.OVDisp> Disp { get; } = new List<Enums.OVDisp>();

        public override SPacket ParseHeader(byte[] data)
        {
            ushort count = ByteUtils.ToUShort(data, 0);
            int ptr = 2;
            while(ptr < data.Length)
            {
                Disp.Add((Enums.OVDisp)Enum.ToObject(typeof(Enums.OVDisp), ByteUtils.ToUShort(data, ptr)));
                ptr += 2;
            }
            return this;
        }
    }
}
