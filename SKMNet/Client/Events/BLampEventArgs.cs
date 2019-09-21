using SKMNET.Client.Networking.Server.RMON;
using SKMNET.Client.Tasten;

namespace SKMNET.Client.Events
{
    public class BLampEventArgs : SkmEventArgs<BLamp>
    {
        public BLampEventArgs(BLamp packet) : base(packet)
        {
        }

        public Taste.LampState GetState(Taste taste) => Packet.LampStates[taste.TastNr];

        public Taste.LampState GetState(TastenManager manager, string name) => GetState(manager.FindByName(name));
    }
}