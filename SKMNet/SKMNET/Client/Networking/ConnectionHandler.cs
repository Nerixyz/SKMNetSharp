using SKMNET.Client.Networking.Client;
using SKMNET.Client.Networking.Server;
using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SKMNET.Client.Networking
{
    public class ConnectionHandler
    {
        public  const    ushort          MAGIC_NUMBER = 0x1fe2;
        private          SKMUdpClient    sender;
        private readonly RecieveClient   reciever;
        public  readonly LightingConsole console;
        private readonly Queue<byte[]> sendQueue;
        private readonly Thread sendThread;
        private readonly PacketDispatcher packetDispatcher;
        
        private Action<byte[]> queuedAction;


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
            this.packetDispatcher = new PacketDispatcher(this);
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

            SendPacket(new SKMSync((int)SKMSync.Flags.Steller));
        }

        private void Reciever_Errored(object sender, Exception e)
        {
            OnErrored(this, e);
        }

        private void Reciever_Recieve(object sender, RecieveClient.RecieveEventArgs args)
        {
            try
            {
                args.ResponseCode = packetDispatcher.OnDataIncoming(args.Data);
            }catch(Exception e)
            {
                OnErrored(this, e);
                Logger.Log(e.StackTrace);
            }
        }

        private void Sender_Errored(object sender, Exception e)
        {
            OnErrored(this, e);
        }

        private void Sender_Recieve(object sender, byte[] e)
        {
            queuedAction?.Invoke(e);
            queuedAction = null;
        }
        
        public void SendPacket(CPacket header)
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
                byte[] arrOut = parser.GetArray();
                Console.WriteLine(ByteUtils.ArrayToString(arrOut));
                sendQueue.Enqueue(arrOut);
            }
        }

        public void SendPacket(byte[] data, short type)
        {
            ByteArrayParser parser = new ByteArrayParser();
            parser.Add(MAGIC_NUMBER).Add(type).Add(GetLocalIPAddress()).Add(data);
            sendQueue.Enqueue(parser.GetArray());
        }

        public void SendPacket(byte[] data, short type, Action<byte[]> callback)
        {
            SendPacket(data, type);
            this.queuedAction = callback;
        }

        public void SendRawData(byte[] arr)
        {
            sendQueue.Enqueue(arr);
        }

        public void SendPacket(CPacket header, Action<byte[]> callback)
        {
            SendPacket(header);
            this.queuedAction = callback;
        }
        public void SendPacket(SplittableHeader header, Action<byte[]> callback)
        {
            SendPacket(header);
            this.queuedAction = callback;
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

        public event EventHandler<Exception> Errored;
        public void OnErrored(object sender, Exception data) { Errored?.Invoke(this, data); }

        public event EventHandler<PacketRecievedEventArgs> PacketRecieved;
        public void OnPacketRecieved(PacketDispatcher sender, PacketRecievedEventArgs args) { PacketRecieved?.Invoke(sender, args); }

        public enum SKMON_RES
        {
            OK,
            RESET,
            KEY_PENDING,
            BAD_CMD,
            OFFLINE
        }
    }

    public class PacketRecievedEventArgs
    {
        public Enums.Type type;
        public SPacket packet;

        public PacketRecievedEventArgs(Enums.Type type, SPacket packet)
        {
            this.type = type;
            this.packet = packet;
        }
    }
}
