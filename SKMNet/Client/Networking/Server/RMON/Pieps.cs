namespace SKMNET.Client.Networking.Server.RMON
{
    /// <summary>
    /// Piepsen
    /// </summary>
    public class Pieps : SPacket
    {

        public override SPacket ParsePacket(ByteBuffer buffer) => this;

        public override Enums.Response ProcessPacket(LightingConsole console, int type)
        {
            console.OnPieps(this);
            return Enums.Response.OK;
        }
    }
}
