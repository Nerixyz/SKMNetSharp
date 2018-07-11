using SKMNET.Util;

namespace SKMNET.Networking.Server
{
    public abstract class SPacket
    {
        public abstract SPacket ParseHeader(ByteBuffer buffer);
        public abstract int HeaderLength { get; }
    }
}
