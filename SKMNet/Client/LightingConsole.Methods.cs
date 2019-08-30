using SKMNET.Client.Networking.Client;
using SKMNET.Client.Stromkreise;
using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client
{
    public sealed partial class LightingConsole
    {
        public void Query(byte[] data, short type) => Connection.SendPacket(data, type);

        /// <summary>
        /// Send CPacket
        /// </summary>
        /// <param name="packet">Packet to send</param>
        public void Query(CPacket packet) => Connection.SendPacket(packet);

        /// <summary>
        /// Send packet to console
        /// </summary>
        /// <param name="packet"></param>
        public void Query(SplittableHeader packet) => Connection.SendPacket(packet);

        public async Task<Enums.FehlerT> QueryAsync(CPacket packet) => await Connection.SendPacketAsync(packet).ConfigureAwait(false);

        public async Task<Enums.FehlerT> QueryAsync(SplittableHeader header) => await Connection.SendPacketAsync(header).ConfigureAwait(false);

        public async Task<Enums.FehlerT> QueryAsync(byte[] data, short type) => await Connection.SendPacketAsync(data, type).ConfigureAwait(false);

        public void SendRawData(byte[] arr) => Connection.SendRawData(arr);

        public SK GetSKByNumber(short num)
        {
            return Stromkreise[num];
        }
    }
}
