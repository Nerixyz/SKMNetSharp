using SKMNET.Client.Networking.Server;
using SKMNET.Client.Networking.Server.RMON;
using SKMNET.Client.Networking.Server.ISKMON;
using SKMNET.Client.Networking.Server.LIBRAExt;
using SKMNET.Client.Networking.Server.SKMON;
using SKMNET.Client.Networking.Server.T98;
using SKMNET.Client.Networking.Server.TSD;
using SKMNET.Networking.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking
{
    class PacketDispatcher
    {
        ConnectionHandler connection;
        private readonly Dictionary<int, Type> serverPacketMap;

        public PacketDispatcher(ConnectionHandler handler)
        {
            this.connection = handler;
            this.serverPacketMap = new Dictionary<int, Type>()
            {
                /* RMON */
                { 0, typeof(Sync) },
                { 1, typeof(ScreenData) },
                { 2, typeof(PalData) },
                { 3, typeof(ReadKey) },
                { 4, typeof(Pieps) },
                { 5, typeof(BLamp) },
                { 6, typeof(ACKReset) },

                { 11, typeof(MScreenData) },
                { 12, typeof(MPalData) },

                /* SKMON */
                { 100, typeof(SkData) },
                { 101, typeof(SkAttr) },
                { 102, typeof(Headline) },
                { 103, typeof(Sync) },
                { 104, typeof(Conf) },

                /* ISKMON */
                { 105, typeof(BTastConf) },
                { 106, typeof(FKeyConf) },
                { 107, typeof(Bed) },
                { 108, typeof(Meld) },
                { 110, typeof(AZ) },
                { 111, typeof(AZ) },
                { 112, typeof(AZ) },
                { 115, typeof(SKGConf) },

                /* T98 */
                { 120, typeof(Sync) }, /* returns SKMON_OK  */
                { 121, typeof(SKRegConf) },
                { 122, typeof(SKRegData) },
                { 123, typeof(SKRegAttr) },

                /* TSD */
                { 130, typeof(Sync) }, /* returns SKMON_OK */
                { 131, typeof(DMXData) },
                { 132, typeof(TSD_MLPal) },
                { 133, typeof(TSD_MLPal) }, /* the same i guess */

                /* MLC */
                { 150, typeof(MLCJob) },
                { 151, typeof(SelPar) },
                { 152, typeof(SelRange) },
                { 153, typeof(ParDef) },
                { 154, typeof(MLPalConf) },
                { 155, typeof(MLPalSK) },

                /* Libra 1.8 */
                { 158, typeof(AktInfo) },
            };
            
        }

        public Enums.Response OnDataIncoming(byte[] data)
        {
            ushort type = ByteUtils.ToUShort(data, 0);
            bool result = serverPacketMap.TryGetValue(type, out Type pType);
            if (!result)
            {
                return Enums.Response.BadCmd;
            }

            SPacket packet = (SPacket)Activator.CreateInstance(pType);
            byte[] actualPacket = { };
            if (data.Length > 2) {
                actualPacket = new byte[data.Length - 2];
                Array.Copy(data, 2, actualPacket, 0, data.Length - 2);
            }

            packet.ParsePacket(new ByteBuffer(actualPacket));
            PacketRecieved?.Invoke(this, packet);

            return Enums.Response.OK;
        }

        public event EventHandler<SPacket> PacketRecieved;
    }
}
