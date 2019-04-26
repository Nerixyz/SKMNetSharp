namespace SKMNET.Client.Networking.Client
{
    public class StellermeldungEvent : Event
    {
        private readonly byte value;
        private readonly byte stellno;

        
        public StellermeldungEvent(Enums.Steller steller, byte value)
        {
            stellno = (byte) steller;
            this.value = value;
        }

        public override int GetEventInteger(LightingConsole console)
        {
            return 0x06000000 | ((byte)console.BdstNo << 16) | (value << 8) | stellno;
        }
    }
}
