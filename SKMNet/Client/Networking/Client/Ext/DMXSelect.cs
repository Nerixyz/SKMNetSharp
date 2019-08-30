namespace SKMNET.Client.Networking.Client.Ext
{
    /// <summary>
    /// Leitungsauswahl für DMX-Daten
    /// </summary>
    public class DmxSelect : CPacket
    {
        public override short Type => 23;

        private readonly short subCmd;
        private readonly bool[] config;

        public DmxSelect(bool[] data, bool  mlc)
        {
            config = data;
            subCmd = (short) (mlc ? 1 : 0);
        }

        public override byte[] GetDataToSend(LightingConsole console)
        {
            ByteBuffer buffer = new ByteBuffer().WriteShort(console.BdstNo).WriteShort(subCmd).WriteShort((short)(config?.Length ?? 0));
            if (config == null) return buffer.ToArray();
            foreach(bool b in config)
            {
                buffer.WriteShort((short)(b ? 1 : 0));
            }
            return buffer.ToArray();
        }
    }
}
