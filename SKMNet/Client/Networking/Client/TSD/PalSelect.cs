﻿using SKMNET.Util;

 namespace SKMNET.Client.Networking.Client.TSD
{
    /// <summary>
    /// Auswahl für Palettendaten
    /// </summary>
    public class PalSelect : CPacket
    {
        public override short Type => 24;
        private readonly short palMask;
        private const short SubCmd = 0;

        public override byte[] GetDataToSend(LightingConsole console) => new ByteBuffer().Write(console.BdstNo).Write(SubCmd).Write(palMask).ToArray();

        public PalSelect(MlUtil.MlPalFlag PalMask)
        {
            this.palMask = (short)PalMask;
        }

        public PalSelect(short mask)
        {
            palMask = mask;
        }
    }
}
