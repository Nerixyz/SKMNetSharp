using SKMNET.Client.Networking.Server;
using SKMNET.Client.Networking.Server.RMON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Rendering
{
    [Serializable]
    public class ScreenManager
    {
        public VideoFarbe[] Paletten { get; }
        public Screen[] Screens { get; }

        [NonSerialized]
        private readonly LightingConsole console;

        public ScreenManager(LightingConsole console)
        {
            this.console = console;
            Screens = new Screen[4];
            this.Paletten = new VideoFarbe[64];
        }

        public void HandleData(ScreenData packet)
        {
            Screens[0].HandleData(packet.Data, packet.Start, packet.Count);
        }

        public void HandleData(MScreenData packet)
        {
            (Screens[(packet.Monitor & MScreenData.MON_MASK) - 1] ?? (Screens[(packet.Monitor & MScreenData.MON_MASK) -1] = new Screen(this, (byte)(packet.Monitor & MScreenData.MON_MASK)))).HandleData(packet.Data, packet.Start, packet.Count);
        }

        public void HandleData(MPalData packet)
        {
            foreach(VideoFarbe farbe in packet.farbeintrag)
            {
                Paletten[farbe.farbno] = farbe;
            }
        }

        public void HandleData(PalData packet)
        {
            foreach (VideoFarbe farbe in packet.farbeintrag)
            {
                Paletten[farbe.farbno] = farbe;
            }
        }
    }
}
