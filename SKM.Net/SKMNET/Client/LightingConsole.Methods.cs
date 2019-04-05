using SKMNET.Client.Networking.Client;
using SKMNET.Client.Stromkreise;
using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client
{
    public partial class LightingConsole
    {
        [Obsolete("Use SendPacketAsync instead", false)]
        public void Query(byte[] data, short type) => Connection.SendPacket(data, type);

        [Obsolete("Use SendPacketAsync instead", false)]
        /// <summary>
        /// Send CPacket
        /// </summary>
        /// <param name="packet">Packet to send</param>
        /// <param name="callback">ErrorCode</param>
        public void Query(CPacket packet) => Connection.SendPacket(packet);

        [Obsolete("Use SendPacketAsync instead", false)]
        public void Query(SplittableHeader packet) => Connection.SendPacket(packet);

        public async Task<Enums.FehlerT> QueryAsync(CPacket packet) => await Connection.SendPacketAsync(packet);

        public async Task<Enums.FehlerT> QueryAsync(SplittableHeader header) => await Connection.SendPacketAsync(header);

        public async Task<Enums.FehlerT> QueryAsync(byte[] data, short type) => await Connection.SendPacketAsync(data, type);

        public void SendRawData(byte[] arr) => Connection.SendRawData(arr);

        public SK GetSKByNumber(short num, bool entireSet = false)
        {
            return entireSet ? Stromkreise.Find((x) => x.Number == num) : ActiveSK.Find((x) => x.Number == num);
        }
    }
}
