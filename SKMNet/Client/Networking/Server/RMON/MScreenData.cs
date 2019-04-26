namespace SKMNET.Client.Networking.Server.RMON
{
    /// <summary>
    /// Multiscreen Bildschirmdaten
    /// </summary>
    public class MScreenData : SPacket
    {

        public const ushort MON_MASK = 0x000f;
        public const ushort MON_HM_FLAG = 0x8000;
        public const byte MON_MAX = 4;

        public ushort Monitor;
        public ushort Start;
        public ushort Count;
        public ushort[] Data;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            Monitor = buffer.ReadUShort();
            Start = buffer.ReadUShort();
            Count = buffer.ReadUShort();

            Data = new ushort[Count];
            for(int i = 0; i < Count; i++){
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
