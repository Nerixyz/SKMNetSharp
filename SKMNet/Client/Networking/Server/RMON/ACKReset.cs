namespace SKMNET.Client.Networking.Server.RMON
{
    /// <summary>
    /// Auf das RESET wurde ein vollständiges Update gesendet
    /// </summary>
    public class AckReset : SPacket
    {

        public override SPacket ParsePacket(ByteBuffer buffer) => this;

        public override Enums.Response ProcessPacket(LightingConsole console, int type) =>
            //TODO clear data
            Enums.Response.OK;
    }
}
