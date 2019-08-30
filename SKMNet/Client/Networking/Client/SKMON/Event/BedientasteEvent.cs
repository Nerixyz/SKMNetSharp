﻿ namespace SKMNET.Client.Networking.Client.SKMON.Event
{
    public class BedientasteEvent : Event
    {
        private readonly byte flanke;
        private readonly byte btast;

        // TODO make Enum
        public BedientasteEvent(byte btast, bool tSteigend)
        {
            this.btast = btast;
            flanke = (byte)( tSteigend ? 1 : 0);
        }

        public override int GetEventInteger(LightingConsole console) => 0x04000000 | ((byte)console.BdstNo << 16) | (flanke << 8) | btast;
    }
}
