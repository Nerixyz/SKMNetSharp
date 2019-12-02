using System.Collections.Generic;

namespace SKMNET.Client.Networking.Client.Ext
{
    public class FixParRaw : SplittableHeader
    {
        private readonly short subCmd;
        private readonly Enums.FixParDst dstReg;
        private FixParEntry[] entries;
        public override short Type => 20;

        public override IEnumerable<byte[]> GetData(LightingConsole console) =>
            Make(
                entries,
                200,
                CountShort,
                (buf, _) => buf.Write(console.BdstNo).Write(subCmd).Write((short)dstReg),
                (e, buf) => buf.Write(e.FixtureId).WriteShort(e.ParId).Write((short)(e.Value << 8))
            );
        
        public FixParRaw( Enums.FixParDst reg = Enums.FixParDst.Current, params FixParEntry[] entries)
        {
            this.entries = entries;
            subCmd = 0; //ABS
            dstReg = reg;
        }
    }

    public struct FixParEntry
    {
        public short FixtureId;
        public short ParId;
        public byte Value;
    }
}