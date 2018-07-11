using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Server.TSD
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

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            fixture = buffer.ReadUShort();
            fixtureName = buffer.ReadString(8);
            last = buffer.ReadUShort() != 0;
            count = buffer.ReadUShort();
            parameters = new SelParData[count];
            for(int i = 0; i < count; i++)
            {
                parameters[i] = new SelParData(
                    buffer.ReadShort(),
                    buffer.ReadUShort(),
                    buffer.ReadString(8),
                    buffer.ReadString(8),
                    buffer.ReadString(8));

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
