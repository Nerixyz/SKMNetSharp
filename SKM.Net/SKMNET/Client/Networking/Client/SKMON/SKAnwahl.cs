using SKMNET.Client.Stromkreise;
using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SKMNET.Enums;

namespace SKMNET.Client.Networking.Client
{
    /// <summary>
    /// SK-Anwahl-Telegramm
    /// </summary>
    public class SKAnwahl : CPacket
    {
        public override short Type => 15;
        private readonly AWType type;
        private readonly short[] skNo;

        public override byte[] GetDataToSend(LightingConsole console)
        {
            return new ByteBuffer().WriteShort(console.BdstNo).Write((short)type).Write((short)skNo.Length).Write(skNo).ToArray();
        }

        public SKAnwahl(AWType type, params short[] skNo)
        {
            this.type = type;
            this.skNo = skNo;
        }

        public SKAnwahl(AWType type, params SK[] skNo)
        {
            this.type = type;
            this.skNo = skNo.Select((sk) => (short)sk.Number).ToArray();
        }
    }
}
