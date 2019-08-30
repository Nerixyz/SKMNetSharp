namespace SKMNET.Client.Networking.Client.SKMON.Event
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

        public override int GetEventInteger(LightingConsole console) => 0x06000000 | ((byte)console.BdstNo << 16) | (macroNoMsb << 8) | macroNoLsb;
    }
}
