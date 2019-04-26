using SKMNET.Client.Stromkreise;
using SKMNET.Client.Stromkreise.ML;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SKMNET.Client.Networking.Client
{
    /// <summary>
    /// Geräte-orientierte Parameterwerte
    /// </summary>
    public class FixPar : SplittableHeader
    {
        private readonly ValueType valueType;
        private readonly Enums.FixParDst dstReg;
        private readonly MLParameter[] parameters;
        public override short Type => 20;

        public override IEnumerable<byte[]> GetData(LightingConsole console)
        {
            return Make(
                parameters,
                200,
                CountShort,
                (buf, _) => buf.Write(console.BdstNo).Write((short)valueType).Write((short)dstReg),
                (par, buf) =>
                {
                    if (par.Sk == null)
                        throw new NullReferenceException("MLParameter.SK not set");
                    buf.Write((short)par.Sk.Number).Write(par.ParNo).Write((short)((int)par.Value << 8));
                }
            );
        }

        public FixPar(ValueType type = ValueType.ABS, Enums.FixParDst reg = Enums.FixParDst.Current, params MLParameter[] parameters)
        {
            this.parameters = parameters;
            valueType = type;
            dstReg = reg;
        }

        public FixPar(ValueType type = ValueType.ABS, Enums.FixParDst reg = Enums.FixParDst.Current, params SK[] sks)
        {
            int estSize = sks.Sum(s => s.Parameters.Count);

            parameters = new MLParameter[estSize];
            for(int i = 0, pointer = 0; i < sks.Length; i++)
            {
                foreach(MLParameter parameter in sks[i].Parameters)
                {
                    parameters[i] = parameter;

                    pointer++;
                }
            }
            valueType = type;
            dstReg = reg;
        }

        public enum ValueType
        {
            ABS,
            REL,
            HOME,
            PLUS,
            MINUS
        }

    }
}
