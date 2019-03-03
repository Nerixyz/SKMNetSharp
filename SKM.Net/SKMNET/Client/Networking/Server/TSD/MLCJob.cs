using SKMNET.Client.Vorstellungen;
using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Server.TSD
{
    /// <summary>
    /// einfache Jobkommandos an MLC
    /// </summary>
    [Serializable]
    public class MLCJob : SPacket
    {

        public ushort job;
        public uint par1;
        public uint par2;
        public uint par3;
        public ushort res1;
        public ushort count;
        public string buf;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            job = buffer.ReadUShort();
            par1 = buffer.ReadUInt();
            par2 = buffer.ReadUInt();
            par3 = buffer.ReadUInt();
            res1 = buffer.ReadUShort();
            count = buffer.ReadUShort();
            buf = buffer.ReadString(count);
            return this;
        }

        public bool IsLoad()
        {
            return job == 1;
        }
        public bool IsSave()
        {
            return job == 2;
        }

        public override Enums.Response ProcessPacket(LightingConsole console, ConnectionHandler handler, int type)
        {
            Vorstellung vst = console.Vorstellungen.Find((x) => par1 == x.Number);
            if(vst is null)
            {
                vst = new Vorstellung((ushort)par1);
                console.Vorstellungen.Add(vst);
            }
            return Enums.Response.OK;
        }
    }
}
