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
        RecieveClient reciever;
        readonly LightingConsole console;
        Thread syncThread;
        readonly DateTime lastResponse;

        public ConnectionHandler(string ipAdress, LightingConsole parent)
        {
            this.console = parent;
            this.sender = new SKMUdpClient(new IPEndPoint(IPAddress.Parse(ipAdress), 5063));
            this.reciever = new RecieveClient();
            this.sender.Recieve += Sender_Recieve;
            this.sender.Errored += Sender_Errored;
            this.sender.Start();
            this.reciever.Recieve += Reciever_Recieve;
            this.reciever.Errored += Reciever_Errored;
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

        private void Reciever_Recieve(object sender, RecieveClient.RecieveEventArgs args)
        {
            byte[] e = args.Data;
            short type = BitConverter.ToInt16(e, 0);
            byte[] data = new byte[e.Length - 2];
            if (e.Length > 2)
            {
                Array.Copy(e, 2, data, 0, e.Length - 2);
            }

            if (Enum.IsDefined(typeof(Enums.Type), (int)type)){
                args.ResponseCode = Enums.Response.OK;
                Enums.Type t = (Enums.Type)type;
                switch (t)
                {
                    case Enums.Type.Sync:
                        {
                            args.ResponseCode = Enums.Response.OK;
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
                            args.ResponseCode = Enums.Response.BadCmd;
                            break;
                        }
                }
            }
            else
            {
                args.ResponseCode = Enums.Response.BadCmd;
            }
        }

        private void Sender_Errored(object sender, Exception e)
        {
            throw e;
        }

        private void Sender_Recieve(object sender, byte[] e)
        {
            
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
