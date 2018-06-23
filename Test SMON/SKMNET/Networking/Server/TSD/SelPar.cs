using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Networking.Server.TSD
{
    class SelPar : Header
    {
        public override int HeaderLength => 16;

        public ushort command;
        public ushort fixture;
        public string fixtureName;
        public bool last;
        public ushort count;
        public SelParData[] parameters;

        public override Header ParseHeader(byte[] data)
        {
            command = BitConverter.ToUInt16(data, 0);
            fixture = BitConverter.ToUInt16(data, 2);
            fixtureName = Encoding.ASCII.GetString(data, 4, 8);
            last = BitConverter.ToUInt16(data, 12) != 0;
            count = BitConverter.ToUInt16(data, 14);
            parameters = new SelParData[count];
            const byte DATA_ENTRY_LENGTH = 28;
            for(int i = 0; i < count; i++)
            {
                parameters[i] = new SelParData(
                    BitConverter.ToInt16(data, i * DATA_ENTRY_LENGTH + HeaderLength),
                    BitConverter.ToUInt16(data, i * DATA_ENTRY_LENGTH + HeaderLength + 2),
                    Encoding.ASCII.GetString(data, i * DATA_ENTRY_LENGTH + HeaderLength + 4, 8),
                    Encoding.ASCII.GetString(data, i * DATA_ENTRY_LENGTH + HeaderLength + 12, 8),
                    Encoding.ASCII.GetString(data, i * DATA_ENTRY_LENGTH + HeaderLength + 20, 8));

            }
            return this;
        }

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
