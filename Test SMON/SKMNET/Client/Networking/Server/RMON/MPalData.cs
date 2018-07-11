using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Server.RMON
{
    class MPalData : SPacket
    {
        public override int HeaderLength => 0;

        public VideoFarbe[] farbeintrag;
        public ushort monitor;
        // siehe MScreenData.cs

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            monitor = buffer.ReadUShort();
            const int farbSize = 8;
            for (int i = 2; i < buffer.Length; i += farbSize)
            {
                VideoFarbe eintrag = farbeintrag[i / farbSize];
                eintrag = new VideoFarbe(buffer.ReadShort(), buffer.ReadByte(), buffer.ReadByte(), buffer.ReadByte(), buffer.ReadByte(), buffer.ReadByte(), buffer.ReadByte());
            }
            return this;
        }

        public MPalData()
        {
            farbeintrag = new VideoFarbe[64];
        }
    }
}
