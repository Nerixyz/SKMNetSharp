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
        public uint vstNum;
        /// <summary>
        /// 0 = normal; 'detect' changed <cref="vstNum"/>
        /// 1 = force; reload MLCConfig
        /// </summary>
        public uint modus;
        public uint par3;
        public ushort res1;
        public ushort count;
        public string buf;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            job = buffer.ReadUShort();
            vstNum = buffer.ReadUInt();
            modus = buffer.ReadUInt();
            par3 = buffer.ReadUInt();
            res1 = buffer.ReadUShort();
            count = buffer.ReadUShort();
            buf = buffer.ReadString(count);
            return this;
        }

        public bool Load { get => job == 1; }
        public bool Save { get => job == 2; }

        public override Enums.Response ProcessPacket(LightingConsole console, ConnectionHandler handler, int type)
        {
            Vorstellung vst = console.Vorstellungen.Find((x) => vstNum == x.Number);
            if (vst is null)
            {
                vst = new Vorstellung((ushort)vstNum);
                console.Vorstellungen.Add(vst);
            }
            return Enums.Response.OK;
        }
    }
}
