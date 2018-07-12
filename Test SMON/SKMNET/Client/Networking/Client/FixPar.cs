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
        private readonly List<SK> list;
        public override short Type => 20;

        public override List<byte[]> GetData()
        {
            List<SK> split = new List<SK>();
            List<byte[]> arr = new List<byte[]>();
            List<short> countz = new List<short>();
            restart:
            short count = 0;
            foreach (SK sk in list)
            {
                if(count + sk.Parameters.Count > 200)
                {
                    split.Add(sk);
                    countz.Add(count);
                    goto restart;
                }
                count += (short)sk.Parameters.Count;

            }
            countz.Add(count);
            bool rebuild = true;
            int i = 0;
            ByteArrayParser parser = new ByteArrayParser();
            foreach(SK sk in list)
            {
                if (rebuild)
                {
                    parser.Add(BStdNo).Add(subCmd).Add((short)dstReg).Add(countz[i]);
                }
                if(split.Count != 0 && split[i] == sk)
                {
                    rebuild = true;
                    arr.Add(parser.GetArray());
                    parser.List().Clear();
                    i++;
                    continue;
                }
                foreach(MLParameter par in sk.Parameters)
                {
                    parser.Add((short)sk.Number).Add(par.ParNo).Add((short)((int)par.Value << 8));
                }
            }
            arr.Add(parser.GetArray());

            return arr;
        }

        public FixPar(List<SK> list, Enums.FixParDst reg = Enums.FixParDst.Current, short subCmd = 0)
        {
            this.list = list;
            this.subCmd = subCmd;
            this.dstReg = reg;
        }
    }
}
