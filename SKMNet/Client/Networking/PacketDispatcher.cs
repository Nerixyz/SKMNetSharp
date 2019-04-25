using System;
using System.Collections.Generic;
using SKMNET.Client.Networking.Server;
using SKMNET.Client.Networking.Server.T98;
using SKMNET.Client.Networking.Server.TSD;
using SKMNET.Client.Networking.Server.RMON;
using SKMNET.Client.Networking.Server.SKMON;
using SKMNET.Client.Networking.Server.ISKMON;
using SKMNET.Client.Networking.Server.LIBRAExt;
using System.Diagnostics;
using SKMNET.Exceptions;

namespace SKMNET.Client.Networking
{
    public class PacketDispatcher
    {
        private readonly ConnectionHandler connection;
        private readonly Dictionary<int, Type> serverPacketMap;

        public PacketDispatcher(ConnectionHandler handler)
        {
            this.connection = handler;
            this.serverPacketMap = new Dictionary<int, Type>
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
                { 156, typeof(SelPar) }, // = SKMON_MLPAR

                /* Libra 1.8 */
                { 158, typeof(AktInfo) },

                /* MLC 2.0 ? */
                { 159, typeof(SelPar) }, // = SKMON_MLPAR_UPD
                { 160, typeof(SelPar) } // = SKMON_MLPAR_REMOVE
            };

        }

        public Enums.Response OnDataIncoming(byte[] data)
        {
            try
            {
                ByteBuffer packetBuffer = new ByteBuffer(data);

                Enums.Response code = Enums.Response.OK;

                ushort type;
                //check if packet fits in buffer and check nullterminator
                while (packetBuffer.Position < packetBuffer.Length - 2 && (type = packetBuffer.ReadUShort()) != 0)
                {
                    Enums.Type eType = Enums.GetEnum<Enums.Type>(type);

                    //get PacketClass/Type
                    if (!serverPacketMap.TryGetValue(type, out Type packetType))
                    {
                        connection.OnErrored(this, new UnknownSKMPacketException(type, data, packetBuffer));
                        return Enums.Response.BadCmd;
                    }

                    SPacket packet = (SPacket)Activator.CreateInstance(packetType);

                    packet.ParsePacket(packetBuffer);
                    code = packet.ProcessPacket(connection.Console, connection, type);

                    PacketReceivedEventArgs consolePacket = new PacketReceivedEventArgs(eType, packet);
                    connection.OnPacketReceived(this, consolePacket);
                    if (consolePacket.response != Enums.Response.OK)
                        code = consolePacket.response;

                    if (code != Enums.Response.OK)
                        break;
                }
                return code;

            } catch(Exception e)
            {
                connection.OnErrored(this, e);
                return Enums.Response.BadCmd;
            }
        }
    }
}
