﻿namespace SKMNET.Client.Networking.Client
{
    /// <summary>
    /// Leitungsauswahl für DMX-Daten
    /// </summary>
    public class DMXSelect : CPacket
    {
        public override short Type => 23;

        private readonly short subCmd;
        private readonly bool[] config;

        public DMXSelect(bool[] data, bool  mlc)
        {
            config = data;
            subCmd = (short) (mlc ? 1 : 0);
        }

        public override byte[] GetDataToSend(LightingConsole console)
        {
            ByteBuffer buffer = new ByteBuffer().WriteShort(console.BdstNo).WriteShort(subCmd).WriteShort((short)(config?.Length ?? 0));
            if(config != null)
            {
                foreach(bool b in config)
                {
                    buffer.WriteShort((short)(b ? 1 : 0));
                }
            }
            return buffer.ToArray();
        }
    }
}
