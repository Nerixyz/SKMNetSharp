namespace SKMNET.Client.Networking.Server.SKMON
{
    /// <summary>
    /// Kopfzeile
    /// </summary>
    public class Headline : SPacket
    {

        public ushort Farbno;
        public ushort Count;
        /// <summary>
        /// Kopfzeilen-String
        /// </summary>
        public string Data;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            Farbno = buffer.ReadUShort();
            Count = buffer.ReadUShort();
            Data = buffer.ReadString(Count);
            return this;
        }

        public override Enums.Response ProcessPacket(LightingConsole console, int type)
        {
            console.Headline = Data;
            return Enums.Response.OK;
        }
    }
}
