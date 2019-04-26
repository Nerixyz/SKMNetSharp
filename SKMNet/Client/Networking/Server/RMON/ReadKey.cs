namespace SKMNET.Client.Networking.Server.RMON
{
    /// <summary>
    /// Keyboard Eingabe abholen
    /// </summary>
    public class ReadKey : SPacket
    {

        public override SPacket ParsePacket(ByteBuffer buffer) => this;

        public override Enums.Response ProcessPacket(LightingConsole console, int type) =>
            //TODO idk
            Enums.Response.OK;
    }
}
