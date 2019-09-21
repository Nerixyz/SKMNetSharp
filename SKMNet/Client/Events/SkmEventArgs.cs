using SKMNET.Client.Networking.Server;

namespace SKMNET.Client.Events
{
    public abstract class SkmEventArgs<T> where T : SPacket
    {
        public T Packet { get; }
        
        protected SkmEventArgs(T packet)
        {
            Packet = packet;
        }
    }
}