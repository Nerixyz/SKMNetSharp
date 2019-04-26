namespace SKMNET.Client.Networking.Client
{
    /// <summary>
    /// ML-Parameter selektieren
    /// </summary>
    public class ParSelect : CPacket
    {
        public override short Type => 27;

        private readonly short parno;
        private readonly short subcmd;

        public ParSelect(short parNo, short subcmd = 0)
        {
            parno = parNo;
            this.subcmd = subcmd;
        }


        public override byte[] GetDataToSend(LightingConsole console)
        {
            return new ByteBuffer().Write(parno).Write(subcmd).Write(console.BdstNo).ToArray();
        }
    }
}
