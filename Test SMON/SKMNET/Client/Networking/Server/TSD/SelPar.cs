using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Networking.Server.TSD
{
    [Serializable]
    class SelPar : SPacket
    {
        public override int HeaderLength => 14;
        
        public ushort fixture;
        public string fixtureName;
        public bool last;
        public ushort count;
        public SelParData[] parameters;

        public override SPacket ParseHeader(byte[] data)
        {
            fixture = ByteUtils.ToUShort(data, 0);
            fixtureName = ByteUtils.ToString(data, 2, 8);
            last = ByteUtils.ToUShort(data, 10) != 0;
            count = ByteUtils.ToUShort(data, 12);
            parameters = new SelParData[count];
            const byte DATA_ENTRY_LENGTH = 28;
            for(int i = 0; i < count; i++)
            {
                parameters[i] = new SelParData(
                    ByteUtils.ToShort(data, i * DATA_ENTRY_LENGTH + HeaderLength),
                    ByteUtils.ToUShort(data, i * DATA_ENTRY_LENGTH + HeaderLength + 2),
                    ByteUtils.ToString(data, i * DATA_ENTRY_LENGTH + HeaderLength + 4, 8),
                    ByteUtils.ToString(data, i * DATA_ENTRY_LENGTH + HeaderLength + 12, 8),
                    ByteUtils.ToString(data, i * DATA_ENTRY_LENGTH + HeaderLength + 20, 8));

            }
            return this;
        }

        [Serializable]
        public class SelParData
        {
            /// <summary>
            /// Parameternummer (0-199)
            /// </summary>
            public short parno;
            /// <summary>
            /// Parameterwert
            /// </summary>
            public ushort val16;
            /// <summary>
            /// Parametername
            /// </summary>
            public string parname;
            /// <summary>
            /// Parameterwert als String
            /// </summary>
            public string parval;
            /// <summary>
            /// Palettenname als String
            /// </summary>
            public string palname;

            public SelParData(short parno, ushort val16, string parname, string parval, string palname)
            {
                this.parno = parno;
                this.val16 = val16;
                this.parname = parname;
                this.parval = parval;
                this.palname = palname;
            }
        }
    }
}
