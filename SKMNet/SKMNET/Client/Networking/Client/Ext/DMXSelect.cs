using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Client
{
    /// <summary>
    /// Leitungsauswahl für DMX-Daten
    /// </summary>
    public class DMXSelect : CPacket
    {
        public override short Type => 23;
        
        readonly short subCmd;
        readonly bool[] config;

        public DMXSelect(bool[] data, bool MLC)
        {
            this.config = data;
            this.subCmd = (short) (MLC ? 1 : 0);
        }

        public override byte[] GetDataToSend(LightingConsole console)
        {
            ByteBuffer buffer = new ByteBuffer().WriteShort(console.BdstNo).WriteShort(subCmd).WriteShort((short)(config == null ? 0 : config.Length));
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
