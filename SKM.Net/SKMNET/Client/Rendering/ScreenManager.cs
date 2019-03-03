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
        public List<VideoFarbe> Palletten { get; }
        public Screen[] Screens { get; }

        [NonSerialized]
        private readonly LightingConsole console;

        public ScreenManager(LightingConsole console)
        {
            this.console = console;
            Screens = new Screen[4];
            this.Palletten = new List<VideoFarbe>();
        }

        public void HandleData(ScreenData packet)
        {
            Screens[0].HandleData(packet.data, packet.start, packet.count);
        }

        public void HandleData(MScreenData packet)
        {
            (Screens[(packet.monitor & MScreenData.MON_MASK) - 1] ?? (Screens[(packet.monitor & MScreenData.MON_MASK) -1] = new Screen(this, (byte)(packet.monitor & MScreenData.MON_MASK)))).HandleData(packet.data, packet.start, packet.count);
        }

        public void HandleData(MPalData packet)
        {
            foreach(VideoFarbe farbe in packet.farbeintrag)
            {
                VideoFarbe ptr = GetFarbeByNumber(farbe.farbno);
                if(!(ptr is null))
                {
                    Palletten.Remove(ptr);
                }
                Palletten.Add(farbe);
            }
        }

        public void HandleData(PalData packet)
        {
            foreach (VideoFarbe farbe in packet.farbeintrag)
            {
                VideoFarbe ptr = GetFarbeByNumber(farbe.farbno);
                if (!(ptr is null))
                {
                    Palletten.Remove(ptr);
                }
                Palletten.Add(farbe);
            }
        }


        public VideoFarbe GetFarbeByNumber(short number)
        {
            return Palletten.Find((x) => x!= null && x.farbno == number);
        }
    }
}
