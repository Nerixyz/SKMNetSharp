using System;
using System.Collections.Generic;
using System.Text;

namespace SKMNET.Exceptions
{
    public class UnknownSKMPacketException : Exception
    {
        public ushort Type { get; }
        public byte[] Packet { get; }
        public byte[] Remaining { get; }
        public long Position { get; }
        public UnknownSKMPacketException(ushort type, byte[] packet, ByteBuffer buffer) : base("Packet not implemented")
        {
            this.Type = type;
            this.Position = buffer.Position;
            this.Packet = packet;
            this.Remaining = new byte[buffer.Length - buffer.Position + 2];
            Array.Copy(ByteUtils.ToByte(type), Remaining, 2);
            Array.Copy(Packet, buffer.Position, Remaining, 2, Remaining.Length - 2);
        }
    }
}
