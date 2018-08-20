using SKMNET.Client.Networking.Client;
using SKMNET.Client.Stromkreise;
using SKMNET.Client.Stromkreise.ML;
using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Client
{
    public class FixPar : SplittableHeader
    {
        private const short BStdNo = 0; /* Bedienstelle */
        private readonly short subCmd;
        private readonly Enums.FixParDst dstReg;
        private readonly List<MLParameter> list;
        public override short Type => 20;

        public override List<byte[]> GetData()
        {
            return Make(list, 200, CountShort, new Action<ByteBuffer, int>((buf, total) => 
            {
                buf.Write(BStdNo).Write(subCmd).Write((short)dstReg);
            }), new Action<MLParameter, ByteBuffer>((par, buf) =>
            {
                if (par.SK == null)
                    throw new NullReferenceException("MLParameter.SK not set");
                buf.Write((short)par.SK.Number).Write(par.ParNo).Write((short)((int)par.Value << 8));
            }));
        }

        public FixPar(List<MLParameter> list, Enums.FixParDst reg = Enums.FixParDst.Current, short subCmd = 0)
        {
            this.list = list;
            this.subCmd = subCmd;
            this.dstReg = reg;
        }

        public FixPar(MLParameter par, Enums.FixParDst reg = Enums.FixParDst.Current, short subCmd = 0)
        {
            this.list = new List<MLParameter>
            {
                par
            };
            this.subCmd = subCmd;
            this.dstReg = reg;
        }

        public FixPar(List<SK> list, Enums.FixParDst reg = Enums.FixParDst.Current, short subCmd = 0)
        {
            this.list = new List<MLParameter>();
            foreach (SK sk in list)
            {
                this.list.AddRange(sk.Parameters);
            }
            this.subCmd = subCmd;
            this.dstReg = reg;
        }

        public FixPar(SK sk, Enums.FixParDst reg = Enums.FixParDst.Current, short subCmd = 0)
        {
            this.list = new List<MLParameter>(sk.Parameters);
            this.subCmd = subCmd;
            this.dstReg = reg;
        }
    }
}
