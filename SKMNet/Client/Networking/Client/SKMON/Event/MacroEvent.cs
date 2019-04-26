namespace SKMNET.Client.Networking.Client
{
    public class MacroEvent : Event
    {
        private readonly byte macroNoMsb;
        private readonly byte macroNoLsb;

        public MacroEvent(byte lsb, byte msb = 0)
        {
            macroNoLsb = lsb;
            macroNoMsb = msb;
        }

        public override int GetEventInteger(LightingConsole console)
        {
            return 0x06000000 | ((byte)console.BdstNo << 16) | (macroNoMsb << 8) | macroNoLsb;
        }
    }
}
