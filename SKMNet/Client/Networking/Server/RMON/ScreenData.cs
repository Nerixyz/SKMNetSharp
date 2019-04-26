namespace SKMNET.Client.Networking.Server
{
    /// <summary>
    /// Bildschirmdaten
    /// </summary>
    public class ScreenData : SPacket
    {

        /// <summary>
        /// Maximale Anzahl von Bildschirmdaten (legacy)
        /// </summary>
        public static readonly int MAX_DATA = 733;
        /// <summary>
        /// Position im Bildschirm. Links oben entspricht Position 0.
        /// </summary>
        public ushort Start;
        /// <summary>
        /// Anzahl der folgenden Bildschirm Daten length. (legacy)
        /// </summary>
        public ushort Count;
        /// <summary>
        /// Bildschirmdaten (Bit 15..8 Attribut, Bit 7..0 Zeichen.)
        /// </summary>
        public ushort[] Data;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            Start = buffer.ReadUShort();
            Count = buffer.ReadUShort();
            Data = new ushort[Count];
            for (int i = 0; i < Count; i++)
            {
                Data[i] = buffer.ReadUShort();
            }
            return this;
        }

        public override Enums.Response ProcessPacket(LightingConsole console, int type)
        {
            console.ScreenManager.HandleData(this);
            return Enums.Response.OK;
        }
    }
}
