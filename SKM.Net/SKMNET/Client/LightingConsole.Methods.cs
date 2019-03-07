using SKMNET.Client.Networking.Client;
using SKMNET.Client.Stromkreise;
using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace SKMNET.Client
{
    public partial class LightingConsole
    {
        public void Query(byte[] data, short type, Action<Enums.FehlerT> callback = null) => Connection.SendPacket(data, type, callback);

        /// <summary>
        /// Send CPacket
        /// </summary>
        /// <param name="packet">Packet to send</param>
        /// <param name="callback">ErrorCode</param>
        public void Query(CPacket packet, Action<Enums.FehlerT> callback = null) => Connection.SendPacket(packet, callback);

        public void Query(SplittableHeader packet, Action<Enums.FehlerT> callback = null) => Connection.SendPacket(packet, callback);

        public void SendRawData(byte[] arr) => Connection.SendRawData(arr);

        public SK GetSKByNumber(short num, bool entireSet = false)
        {
            return entireSet ? Stromkreise.Find((x) => x.Number == num) : ActiveSK.Find((x) => x.Number == num);
        }
    }
}
