using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Networking.Server.TSD
{
    class DMXData : Header
    {
        public override int HeaderLength => 4;

        public ushort command; /* = SKMON_TSD_DMXDATA */
        public ushort count; /* 1 or 2 lines */
        public DMXDataEntry[] dmxLines; /* 1 or 2 line data */

        public override Header ParseHeader(byte[] data)
        {
            command = BitConverter.ToUInt16(data, 0);
            count = BitConverter.ToUInt16(data, 2);
            dmxLines = new DMXDataEntry[count];
            for(int i = 0; i < count; i++)
            {
                byte[] dmxData = new byte[512];
                Array.Copy(data, i * 514 + 6, data, 0, 512);
                dmxLines[i] = new DMXDataEntry(BitConverter.ToUInt16(data, i * 514 + 4), dmxData);
            }
            return this;
        }

        public class DMXDataEntry
        {
            public ushort line; /* DMX-Leitungsnummer 1..64 */
        public byte[] dmxData;

            public DMXDataEntry(ushort line, byte[] dmxData)
            {
                this.line = line;
                this.dmxData = dmxData;
            }
        }
    }
}
