namespace SKMNET.Client.Networking.Server.RMON
{
    /// <summary>
    /// Synctelegramm
    /// </summary>
    public class Sync : SPacket
    {

        public override SPacket ParsePacket(ByteBuffer buffer) => this;

        public override Enums.Response ProcessPacket(LightingConsole console, int type) => Enums.Response.OK;
    }
}
