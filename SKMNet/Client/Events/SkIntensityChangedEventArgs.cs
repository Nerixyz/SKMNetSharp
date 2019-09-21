using SKMNET.Client.Networking.Server.T98;

namespace SKMNET.Client.Events
{
    public class SkIntensityChangedEventArgs : SkmEventArgs<SKRegData>
    {
        public SkIntensityChangedEventArgs(SKRegData packet) : base(packet)
        {
        }

        /// <summary>
        /// Returns the value for a given SK
        /// </summary>
        /// <param name="i">SK Number</param>
        public byte this[int i] => Packet.Data[Packet.Start + i];

        public bool IsUpdated(int skNum) => Packet.Start <= skNum && Packet.Start + Packet.Count > skNum;
    }
}