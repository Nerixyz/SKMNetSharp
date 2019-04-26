namespace SKMNET.Client.Networking.Server
{
    public abstract class SPacket
    {
        public abstract SPacket ParsePacket(ByteBuffer buffer);

        public abstract Enums.Response ProcessPacket(LightingConsole console, int type);
    }
}
