namespace SKMNET.Client.Networking.Server.TSD
{
    /// <summary>
    /// DMX-orientierte Kreiswerte
    /// </summary>
    public class DmxData : SPacket
    {

        public ushort Count; /* 1 or 2 lines */
        public DmxDataEntry[] DmxLines; /* 1 or 2 line data */

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            Count = buffer.ReadUShort();
            DmxLines = new DmxDataEntry[Count];
            for(int i = 0; i < Count; i++)
            {
                ushort line = buffer.ReadUShort();
                byte[] dmxData = buffer.ReadByteArray(512);
                DmxLines[i] = new DmxDataEntry(line, dmxData);
            }
            return this;
        }

        public override Enums.Response ProcessPacket(LightingConsole console, int type)
        {
            //TODO actually idk where each SK is located
            return Enums.Response.OK;
        }

        public struct DmxDataEntry
        {
            public ushort Line; /* DMX-Leitungsnummer 1..64 */
            public byte[] DmxData;

            public DmxDataEntry(ushort line, byte[] dmxData)
            {
                Line = line;
                DmxData = dmxData;
            }
        }
    }
}
