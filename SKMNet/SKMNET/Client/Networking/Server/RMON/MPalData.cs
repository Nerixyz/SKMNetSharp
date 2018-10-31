using SKMNET.Client.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Server.RMON
{
    /// <summary>
    /// Video-Multiscreen Palettendaten
    /// </summary>
    public class MPalData : SPacket
    {

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

        public override Enums.Response ProcessPacket(LightingConsole console, ConnectionHandler handler, int type)
        {
            //TODO MonitorHandler
            return Enums.Response.OK;
        }

        public MPalData()
        {
            farbeintrag = new VideoFarbe[64];
        }
    }
}
