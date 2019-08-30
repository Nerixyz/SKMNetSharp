using System.Linq;
using SKMNET.Client.Stromkreise;
using static SKMNET.Enums;

namespace SKMNET.Client.Networking.Client.SKMON
{
    /// <summary>
    /// SKG-Anwahl-Telegramm
    /// </summary>
    public class SKGAnwahl : CPacket
    {
        public override short Type => 16;

        private readonly AWType action;
        private readonly short[] skgs;

        public override byte[] GetDataToSend(LightingConsole console) => new ByteBuffer().WriteShort(console.BdstNo).WriteShort((short)action).WriteShort((short)skgs.Length).Write(skgs).ToArray();

        public SKGAnwahl(AWType type, params short[] skgs)
        {
            action = type;
            this.skgs = skgs;
        }

        public SKGAnwahl(AWType type, params SKG[] skgs)
        {
            action = type;
            this.skgs = skgs.Select(skg => (short)skg.Number).ToArray();
        }
    }
}
