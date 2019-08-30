using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using SKMNET;
using SKMNET.Client;
using SKMNET.Client.Networking.Client;
using SKMNET.Client.Networking.Client.Ext;
using SKMNET.Client.Stromkreise.ML;

namespace Test
{
    public class FixParColor : SplittableHeader
    {
            public override short Type => 20;

            private readonly IEnumerable<SkInfo> infos;
            private readonly int count;
            private readonly FixPar.ValueType valueType;
            private readonly Enums.FixParDst dstReg;

            public override IEnumerable<byte[]> GetData(LightingConsole console)
            {
                return Make(
                    infos.ToArray(),
                    200,
                    CountShort,
                    (buf, _) => buf.Write((short)console.Bedienstelle).Write((short)valueType).Write((short)dstReg),
                    (par, buf) =>
                    {
                        buf
                            .Write(par.SkNum)
                            .Write(par.Par)
                            .Write((short) (par.Val << 8));
                    }
                );
            }

            public FixParColor(Dictionary<short, Color> mapping, FixPar.ValueType type = FixPar.ValueType.ABS, Enums.FixParDst reg = Enums.FixParDst.Current)
            {
                infos = mapping.SelectMany(x => new SkInfo[]
                {
                    new SkInfo
                    {
                        SkNum = x.Key,
                        Par = FixParMap.Active.Red,
                        Val = x.Value.R
                    }
                    ,new SkInfo
                    {
                        SkNum = x.Key,
                        Par = FixParMap.Active.Green,
                        Val = x.Value.G
                    }
                    ,new SkInfo
                    {
                        SkNum = x.Key,
                        Par = FixParMap.Active.Blue,
                        Val = x.Value.B
                    }
                });
                count = mapping.Count * 3;
                valueType = type;
                dstReg = reg;
            }
            
            private struct SkInfo
            {
                public short SkNum;
                public short Par;
                public byte Val;
            }
    }
}