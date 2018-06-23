using SKMNET.Client;
using SKMNET.Networking.Client;
using SKMNET.Networking.Server;
using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SKMNET.Networking
{
    class ConnectionHandler
    {
        const ushort MAGIC_NUMBER = 0x1fe2;
        SKMUdpClient sender;
        SKMUdpClient reciever;
        LightingConsole console;
        Thread syncThread;
        Thread recSendThread;
        DateTime lastResponse;

        public ConnectionHandler(string ipAdress, LightingConsole parent)
        {
            this.console = parent;
            this.sender = new SKMUdpClient(new IPEndPoint(IPAddress.Parse(ipAdress), 5063));
            this.reciever = new SKMUdpClient(new IPEndPoint(IPAddress.Parse(ipAdress), 5064));
            this.sender.Recieve += Sender_Recieve;
            this.sender.Errored += Sender_Errored;
            this.sender.Start();
            this.reciever.Recieve += Reciever_Recieve;
            this.reciever.Errored += Reciever_Errored;
            this.reciever.Start();
            SendPacket(new SKMSync(-1));
            syncThread = new Thread(() =>
            {
                Thread.Sleep(1000);
                SendPacket(new PalSelect((short)MLPal.MLPalFlag.BLK));
            });
            syncThread.Start();
            lastResponse = DateTime.Now;
        }

        private void Reciever_Errored(object sender, Exception e)
        {
            throw e;
        }

        private void Reciever_Recieve(object sender, byte[] e)
        {
            short type = BitConverter.ToInt16(e, 0);
            byte[] data = null;
            if (e.Length > 2)
            {
                Array.Copy(e, 2, data, 0, e.Length - 2);
            }

            if (Enum.IsDefined(typeof(Enums.Type), type)){
                RespondInc(SKMON_RES.OK);
                Enums.Type t = (Enums.Type)type;
                switch (t)
                {
                    case Enums.Type.Sync:
                        {
                            RespondInc(SKMON_RES.OK);
                            break;
                        }
                    case Enums.Type.ScreenData:
                        {
                            ScreenData packet = (ScreenData)new ScreenData().ParseHeader(data);
                            Console.WriteLine("[in] Got Screen Data: " + packet.count);
                            break;
                        }
                    case Enums.Type.PalData:
                        {
                            break;
                        }
                    case Enums.Type.ReadKey:
                        {
                            break;
                        }
                    case Enums.Type.Pieps:
                        {
                            break;
                        }
                    case Enums.Type.BLamp:
                        {
                            break;
                        }
                    case Enums.Type.ACK_Reset:
                        {
                            break;
                        }
                    case Enums.Type.MScreenData:
                        {
                            break;
                        }
                    case Enums.Type.MPalData:
                        {
                            break;
                        }
                    case Enums.Type.SkData:
                        {
                            break;
                        }
                    case Enums.Type.SkAttr:
                        {
                            break;
                        }
                    case Enums.Type.Headline:
                        {
                            break;
                        }
                    case Enums.Type.Conf:
                        {
                            break;
                        }
                    case Enums.Type.Cmd:
                        {
                            break;
                        }
                    case Enums.Type.BTastConf:
                        {
                            break;
                        }
                    case Enums.Type.FKeyConf:
                        {
                            break;
                        }
                    case Enums.Type.Bedienzeile:
                        {
                            break;
                        }
                    case Enums.Type.Meldezeile:
                        {
                            break;
                        }
                    case Enums.Type.AZ_IST:
                    case Enums.Type.AZ_ZIEL:
                    case Enums.Type.AZ_VOR:
                        {
                            break;
                        }
                    case Enums.Type.SKG_Conf:
                        {
                            break;
                        }
                    case Enums.Type.SKRegSync:
                        {
                            break;
                        }
                    case Enums.Type.SKRegConf:
                        {
                            break;
                        }
                    case Enums.Type.SKRegData:
                        {
                            break;
                        }
                    case Enums.Type.SKRegAttr:
                        {
                            break;
                        }
                    case Enums.Type.TSD_Sync:
                        {
                            break;
                        }
                    case Enums.Type.TSD_DMXData:
                        {
                            break;
                        }
                    case Enums.Type.TSD_MPal:
                    case Enums.Type.TSD_MPalSelect:
                        {
                            break;
                        }
                    case Enums.Type.MLC_Job:
                        {
                            break;
                        }
                    case Enums.Type.MLC_SelPar:
                    case Enums.Type.MLC_SelRange:
                        {
                            break;
                        }
                    case Enums.Type.MLC_ParDef:
                        {
                            break;
                        }
                    case Enums.Type.MLPal_Conf:
                        {
                            break;
                        }
                    case Enums.Type.MLPal_SK:
                        {
                            break;
                        }
                    default:
                        {
                            RespondInc(SKMON_RES.BAD_CMD);
                            break;
                        }
                }
            }
            else
            {
                RespondInc(SKMON_RES.BAD_CMD);
            }
        }

        private void Sender_Errored(object sender, Exception e)
        {
            throw e;
        }

        private void Sender_Recieve(object sender, byte[] e)
        {
            if (e[0] == 0x0)
                RespondInc(SKMON_RES.OK);
            else
                throw new Exception("not implemented");
        }

        public void SendData(ISendable data)
        {
            sender.SendData(data.GetDataToSend());
        }

        public void SendPacket(Client.Header header)
        {
            ByteArrayParser parser = new ByteArrayParser();
            byte[] ip = GetLocalIPAddress();
            parser.Add(MAGIC_NUMBER).Add(header.Type).Add(GetLocalIPAddress()).Add(header.GetDataToSend());
            sender.SendData(parser.GetArray());
        }

        public byte[] GetLocalIPAddress()
        {
            if (sender.local)
            {
                return new byte[] { 127, 0, 0, 1 };
            }
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.GetAddressBytes();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public void RespondInc(SKMON_RES response)
        {
            if (recSendThread == null || recSendThread.ThreadState == ThreadState.Stopped)
            {
                recSendThread = new Thread(() =>
                {
                    reciever.SendData(new ByteArrayParser().Add((short)response).GetArray());
                    Thread.Sleep(100);
                });
                recSendThread.Start();
            }
        }

        public enum SKMON_RES
        {
            OK,
            RESET,
            KEY_PENDING,
            BAD_CMD,
            OFFLINE
        }
    }
}
