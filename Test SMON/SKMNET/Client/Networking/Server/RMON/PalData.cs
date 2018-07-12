using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Server
{
    class PalData : SPacket
    {
        public override int HeaderLength => 0;

        public VideoFarbe[] farbeintrag;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            farbeintrag = new VideoFarbe[64];
            const int farbSize = 8;
            for(int i = 0; i < buffer.Length; i += farbSize)
            {
                VideoFarbe eintrag = farbeintrag[i / farbSize];
                eintrag = new VideoFarbe(buffer.ReadShort(), buffer.ReadByte(), buffer.ReadByte(), buffer.ReadByte(), buffer.ReadByte(), buffer.ReadByte(), buffer.ReadByte());
            }
            return this;
        }

        public override Enums.Response ProcessPacket(LightingConsole console, ConnectionHandler handler, int type)
        {
            //TODO MonitorHandler
            return Enums.Response.OK;
        }

        public PalData()
        {
            farbeintrag = new VideoFarbe[64];
        }
    }
}
