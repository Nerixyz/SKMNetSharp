using SKMNET.Client.Stromkreise;
using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Networking.Client
{
    class DmxData : Header
    {
        public override short Type => 21;
        private const short BdStNo = 0;
        private const short subCmd = 0;
        private Enums.FixParDst dst;
        private List<SK> SKs;

        public override byte[] GetDataToSend()
        {
            ByteArrayParser parser = new ByteArrayParser().
                Add(BdStNo).
                Add(subCmd).
                Add((short)dst).
                Add((short)(SKs == null ? 0 : 1));
            if(SKs != null)
            {
                SKs.Sort(new Comparison<SK>((SK n1, SK n2) =>
                {
                    if (n1.Number > n2.Number) return -1;
                    else if (n2.Number > n1.Number) return 1;
                    else return 0;
                }));
                // assume they are all on the same line
                parser.Add(1);
                int ptr = 0;
                for(int i = 0; i < 512; i++)
                {
                    if(SKs.Count > ptr && SKs[ptr].Number == i + 1)
                    {
                        parser.Add(SKs[ptr].Intensity);
                        ptr++;
                    }
                    else
                    {
                        parser.Add((byte)0);
                    }
                }
            }
            return parser.GetArray();
        }
        
        public DmxData(List<SK> SKs = null, Enums.FixParDst dst = Enums.FixParDst.Current)
        {
            this.SKs = SKs;
            this.dst = dst;
        }
    }
}
