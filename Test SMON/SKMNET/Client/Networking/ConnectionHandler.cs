using Newtonsoft.Json;
using SKMNET.Client;
using SKMNET.Client.Networking.Client;
using SKMNET.Client.Stromkreise;
using SKMNET.Client.Stromkreise.ML;
using SKMNET.Networking.Client;
using SKMNET.Networking.Server;
using SKMNET.Networking.Server.ISKMON;
using SKMNET.Networking.Server.LIBRAExt;
using SKMNET.Networking.Server.RMON;
using SKMNET.Networking.Server.SKMON;
using SKMNET.Networking.Server.T98;
using SKMNET.Networking.Server.TSD;
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
        public  const    ushort          MAGIC_NUMBER = 0x1fe2;
        private          SKMUdpClient    sender;
        private readonly RecieveClient   reciever;
        private readonly LightingConsole console;
        private readonly Queue<byte[]> sendQueue;
        private readonly Thread sendThread;

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
            this.sendQueue = new Queue<byte[]>();
            sendThread = new Thread(() =>
            {
                while (true)
                {
                    if(sendQueue.Count > 0)
                    {
                        sender.SendData(sendQueue.Dequeue());
                    }
                    Thread.Sleep(50);
                }
            });
            sendThread.Start();

            SendPacket(new SKMSync(-1));
        }

        private void Reciever_Errored(object sender, Exception e)
        {
            throw e;
        }

        private void Reciever_Recieve(object sender, RecieveClient.RecieveEventArgs args)
        {
            try
            {
                byte[] e = args.Data;
                ushort type = ByteUtils.ToUShort(e, 0);
                byte[] data = new byte[e.Length - 2];
                if (e.Length > 2)
                {
                    Array.Copy(e, 2, data, 0, e.Length - 2);
                }
                if (Enum.IsDefined(typeof(Enums.Type), (int)type))
                {
                    args.ResponseCode = Enums.Response.OK;
                    Enums.Type t = (Enums.Type)type;
                    if (t != Enums.Type.Sync)
                    {
                        Console.Write("{0:x2}", type);
                        Logger.Log(" " + Enum.GetName(typeof(Enums.Type), t));
                    }

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
                                Logger.Log("[in] Got Screen Data: " + packet.count);
                                break;
                            }
                        case Enums.Type.PalData:
                            {
                                PalData palData = (PalData)new PalData().ParseHeader(data);
                                string json = JsonConvert.SerializeObject(palData.farbeintrag);
                                Logger.Log(json);
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
                                BLamp lamp = (BLamp)new BLamp().ParseHeader(data);
                                //string json = JsonConvert.SerializeObject(lamp);
                                //Logger.Log(json);
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
                                MPalData palData = (MPalData)new MPalData().ParseHeader(data);
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
                                Headline headline = (Headline)new Headline().ParseHeader(data);
                                Logger.Log("Headline: fNo:" + headline.farbno + " d: " + headline.data);
                                return; 
                            }
                        case Enums.Type.Conf:
                            {
                                Conf conf = (Conf)new Conf().ParseHeader(data);
                                string json = JsonConvert.SerializeObject(conf);
                                Logger.Log(json);
                                break;
                            }
                        case Enums.Type.Cmd:
                            {
                                break;
                            }
                        case Enums.Type.BTastConf:
                            {
                                BTastConf packet = (BTastConf)new BTastConf().ParseHeader(data);
                                string json = JsonConvert.SerializeObject(packet.entries);
                                Logger.Log(json);
                                return;
                            }
                        case Enums.Type.FKeyConf:
                            {
                                FKeyConf conf = (FKeyConf)new FKeyConf().ParseHeader(data);
                                string json = JsonConvert.SerializeObject(conf);
                                Logger.Log(json);
                                break;
                            }
                        case Enums.Type.Bedienzeile:
                            {
                                Bed bed = (Bed)new Bed().ParseHeader(data);
                                string json = JsonConvert.SerializeObject(bed);
                                Logger.Log(json);
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
                                SKGConf conf = (SKGConf)new SKGConf().ParseHeader(data);
                                string json = JsonConvert.SerializeObject(conf);
                                Logger.Log(json);
                                break;
                            }
                        case Enums.Type.SKRegSync:
                            {
                                // 0 OK
                                break;
                            }
                        case Enums.Type.SKRegConf:
                            {
                                SKRegConf conf = (SKRegConf)new SKRegConf().ParseHeader(data);
                                if (conf.clear)
                                {
                                    console.Stromkreise.Clear();

                                }
                                for (ushort i = conf.start; i < conf.count + conf.start; i++)
                                {
                                    console.Stromkreise.Insert(i, new SK(conf.data[i - conf.start]));
                                }
                                break;
                            }
                        case Enums.Type.SKRegData:
                            {
                                SKRegData regData = (SKRegData)new SKRegData().ParseHeader(data);
                                for(int i = regData.start; i < regData.start + regData.count; i++)
                                {
                                    SK reg = console.Stromkreise[i];
                                    if(reg != null)
                                    {
                                        reg.Intensity = regData.data[i - regData.start];
                                    }
                                }
                                break;
                            }
                        case Enums.Type.SKRegAttr:
                            {
                                SKRegAttr attr = (SKRegAttr)new SKRegAttr().ParseHeader(data);
                                console.ActiveSK.Clear();
                                for (int i = attr.start; i < attr.start + attr.count; i++)
                                {
                                    SK sk = console.Stromkreise[i];
                                    if (sk != null)
                                    {
                                        sk.Attrib = attr.data[i - attr.start];
                                    }
                                }
                                // TODO: optimize speed
                                foreach(SK sk in console.Stromkreise)
                                {
                                    if(sk.Attrib != 0&& sk.Number != 0)
                                    {
                                        console.ActiveSK.Add(sk);
                                    }
                                }
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
                                TSD_MLPal pal = (TSD_MLPal)new TSD_MLPal().ParseHeader(data);
                                string json = JsonConvert.SerializeObject(pal);
                                Logger.Log(json);
                                break;
                            }
                        case Enums.Type.MLC_Job:
                            {
                                MLCJob job = (MLCJob)new MLCJob().ParseHeader(data);
                                string json = JsonConvert.SerializeObject(job);
                                Logger.Log(json);
                                break;
                            }
                        case Enums.Type.MLC_SelPar:
                            {
                                SelPar selPar = (SelPar)new SelPar().ParseHeader(data);
                                SK sk = console.ActiveSK.Find((inc) =>
                                {
                                    if (inc.Number == selPar.fixture)
                                        return true;
                                    return false;
                                });
                                if(sk != null)
                                {
                                    foreach(SelPar.SelParData par in selPar.parameters)
                                    {
                                        MLParameter param = sk.Parameters.Find((inc) => { return inc.ParNo == par.parno; });
                                        if (param != null)
                                        {
                                            param.Value = (par.val16 & 0xff00) >> 8;
                                            param.Display = par.parval;
                                            param.PalName = par.palname;
                                        }
                                        else
                                        {
                                            param = new MLParameter(par.parname, (-1, -1), par.parno, (par.val16 & 0xff00) >> 8)
                                            {
                                                PalName = par.palname,
                                                Display = par.parname
                                            };
                                            sk.Parameters.Add(param);
                                        }
                                    }
                                }
                                else
                                {
                                    args.ResponseCode = Enums.Response.BadCmd;
                                    return;
                                }
                                string json = JsonConvert.SerializeObject(selPar);
                                Logger.Log(json);
                                break;
                            }
                        case Enums.Type.MLC_SelRange:
                            {
                                SelRange selRange = (SelRange)new SelRange().ParseHeader(data);
                                string json = JsonConvert.SerializeObject(selRange);
                                Logger.Log(json);
                                break;
                            }
                        case Enums.Type.MLC_ParDef:
                            {
                                break;
                            }
                        case Enums.Type.MLPal_Conf:
                            {
                                MLPalConf palConf = (MLPalConf)new MLPalConf().ParseHeader(data);
                                if (palConf.absolute)
                                {
                                    console.IPal.Clear();
                                    console.FPal.Clear();
                                    console.CPal.Clear();
                                    console.BPal.Clear();
                                }
                                if ((palConf.Mlpaltype & 0x0070) == 0)
                                {
                                    MLPal.MLPalFlag flag = (palConf.Mlpaltype & 0x0001) != 0 ? MLPal.MLPalFlag.I : (palConf.Mlpaltype & 0x0002) != 0 ? MLPal.MLPalFlag.F : (palConf.Mlpaltype & 0x0004) != 0 ? MLPal.MLPalFlag.C : MLPal.MLPalFlag.B;
                                    foreach (var pal in palConf.Entries)
                                    {
                                        switch (flag)
                                        {
                                            case MLPal.MLPalFlag.I:
                                                {
                                                    console.IPal.Add(new MLPal(flag, pal.Text, pal.Palno));
                                                    break;
                                                }
                                            case MLPal.MLPalFlag.F:
                                                {
                                                    console.FPal.Add(new MLPal(flag, pal.Text, pal.Palno));
                                                    break;
                                                }
                                            case MLPal.MLPalFlag.C:
                                                {
                                                    console.CPal.Add(new MLPal(flag, pal.Text, pal.Palno));
                                                    break;
                                                }
                                            case MLPal.MLPalFlag.B:
                                                {
                                                    console.BPal.Add(new MLPal(flag, pal.Text, pal.Palno));
                                                    break;
                                                }
                                        }
                                    }
                                }
                                string json = JsonConvert.SerializeObject(palConf);
                                Logger.Log(json);
                                return;
                            }
                        case Enums.Type.MLPal_SK:
                            {
                                break;
                            }
                        case Enums.Type.AKTInfo:
                            {
                                AktInfo info = (AktInfo)new AktInfo().ParseHeader(data);
                                string json = JsonConvert.SerializeObject(info);
                                Logger.Log(json);
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
            }catch(Exception e)
            {
                Logger.Log(e.StackTrace);
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
            sendQueue.Enqueue(data.GetDataToSend());
        }

        public void SendData(ISplittable data)
        {
            foreach(byte[] arr in data.GetData())
            {
                sendQueue.Enqueue(arr);
            }
        }

        public void SendPacket(Client.Header header)
        {
            ByteArrayParser parser = new ByteArrayParser();
            parser.Add(MAGIC_NUMBER).Add(header.Type).Add(GetLocalIPAddress()).Add(header.GetDataToSend());
            sendQueue.Enqueue(parser.GetArray());
        }
        public void SendPacket(SplittableHeader header)
        {
            foreach (byte[] arr in header.GetData())
            {
                ByteArrayParser parser = new ByteArrayParser();
                parser.Add(MAGIC_NUMBER).Add(header.Type).Add(GetLocalIPAddress()).Add(arr);
                sendQueue.Enqueue(parser.GetArray());
            }
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
