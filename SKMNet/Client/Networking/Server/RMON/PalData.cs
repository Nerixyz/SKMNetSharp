using System.Collections.Generic;
using SKMNET.Client.Rendering;

namespace SKMNET.Client.Networking.Server.RMON
{
    /// <summary>
    /// Video-Palettendaten (Komplett-Telegramm)
    /// </summary>
    public class PalData : SPacket
    {

        public List<VideoFarbe> farbeintrag;

        private const int N_HW_PALETTE = 64;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            for (int i = 0; i < N_HW_PALETTE; i ++)
            {
                 farbeintrag.Add(
                     new VideoFarbe(
                         buffer.ReadShort(),
                         buffer.ReadByte(),
                         buffer.ReadByte(),
                         buffer.ReadByte(),
                         buffer.ReadByte(),
                         buffer.ReadByte(),
                         buffer.ReadByte()
                         )
                     );
            }
            return this;
        }

        public override Enums.Response ProcessPacket(LightingConsole console, int type)
        {
            //TODO MonitorHandler
            console.ScreenManager.HandleData(this);
            return Enums.Response.OK;
        }

        public PalData() => farbeintrag = new List<VideoFarbe>(64);
    }
}
