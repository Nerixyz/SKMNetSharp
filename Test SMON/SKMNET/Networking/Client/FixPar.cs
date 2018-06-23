using SKMNET.Client.Stromkreise;
using SKMNET.Client.Stromkreise.ML;
using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Networking.Client
{
    class FixPar : Header
    {
        private const short BStdNo = 0; /* Bedienstelle */
        private short subCmd;
        private Enums.FixParDst dstReg;
        private Dictionary<SK, MLParameter> dictCC;
        public override short Type => 20;

        public override byte[] GetDataToSend()
        {
            ByteArrayParser parser = new ByteArrayParser().
                Add(BStdNo).
                Add(subCmd).
                Add((short)dstReg).
                Add((short)dictCC.Count);
            foreach(KeyValuePair<SK, MLParameter> par in dictCC)
            {
                parser.Add(par.Key.Number).Add(par.Value.ParNo).Add(par.Value.Value);
            }
            return parser.GetArray();
        }

        public FixPar(Dictionary<SK, MLParameter> dict, Enums.FixParDst reg = Enums.FixParDst.Current, short subCmd = 0)
        {
            this.dictCC = dict;
            this.subCmd = subCmd;
            this.dstReg = reg;
        }
    }
}
