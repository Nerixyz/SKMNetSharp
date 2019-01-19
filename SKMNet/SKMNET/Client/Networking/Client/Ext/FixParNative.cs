using SKMNET;
using SKMNET.Client.Networking.Client;
using SKMNET.Client.Stromkreise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Client
{
    class FixParNative : SplittableHeader
    {
        private readonly short subCmd;
        private readonly Enums.FixParDst dstReg;
        private readonly List<SK> list;
        public override short Type => 20;

        public override List<byte[]> GetData(LightingConsole console)
        {
            return Make(list, 200, CountShort, new Action<ByteBuffer, int>((buf, total) =>
            {
                buf.Write(console.BdstNo).Write(subCmd).Write((short)dstReg);
            }), new Action<SK, ByteBuffer>((par, buf) =>
            {
                buf.Write((short)par.Number).WriteShort(0).Write((short)((int)par.Intensity << 8));
            }));
        }

        public FixParNative(List<SK> list, Enums.FixParDst reg = Enums.FixParDst.Current, short subCmd = 0)
        {
            this.list = list;
            this.subCmd = subCmd;
            this.dstReg = reg;
        }
    }
}
