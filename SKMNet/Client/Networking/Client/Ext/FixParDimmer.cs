using System.Collections.Generic;
using SKMNET.Client.Stromkreise;

namespace SKMNET.Client.Networking.Client.Ext
{
    public class FixParDimmer : SplittableHeader
    {
        private readonly short subCmd;
        private readonly Enums.FixParDst dstReg;
        private readonly SK[] sks;
        public override short Type => 20;

        public override IEnumerable<byte[]> GetData(LightingConsole console) =>
            Make(
                sks,
                200,
                CountShort,
                (buf, _) => buf.Write(console.BdstNo).Write(subCmd).Write((short)dstReg),
                (par, buf) => buf.Write((short)par.Number).WriteShort(0).Write((short)(par.Intensity << 8))
            );

        public FixParDimmer( Enums.FixParDst reg = Enums.FixParDst.Current, params SK[] sks)
        {
            this.sks = sks;
            subCmd = 0; //ABS
            dstReg = reg;
        }
    }
}
