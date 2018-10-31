using SKMNET.Client.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Server
{
    /// <summary>
    /// Video-Palettendaten (Komplett-Telegramm)
    /// </summary>
    public class PalData : SPacket
    {

        public VideoFarbe[] farbeintrag;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            farbeintrag = new VideoFarbe[64];
            const int farbSize = 8;
            for (int i = 0; i < buffer.Length; i += farbSize)
            {
                try {
                    VideoFarbe eintrag = farbeintrag[i / farbSize];
                    eintrag = new VideoFarbe(buffer.ReadShort(), buffer.ReadByte(), buffer.ReadByte(), buffer.ReadByte(), buffer.ReadByte(), buffer.ReadByte(), buffer.ReadByte());

                }catch(IndexOutOfRangeException ignored) { }
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
