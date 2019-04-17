using SKMNET.Client.Networking.Client;
using SKMNET.Client.Networking.Server;
using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking
{
    public class ConnectionHandler
    {
        public const ushort MAGIC_NUMBER = 0x1fe2;
        private readonly SendClient sender;
        private readonly RecieveClient reciever;
        public readonly LightingConsole console;
        //private readonly Thread sendThread;
        private readonly PacketDispatcher packetDispatcher;

        private readonly Queue<TaskCompletionSource<Enums.FehlerT>> completionQueue;


        public ConnectionHandler(string ipAdress, LightingConsole parent, SKMSteckbrief steckbrief, byte SKMType)
        {
            this.console = parent;

            this.sender = new SendClient(new IPEndPoint(IPAddress.Parse(ipAdress), 5063));
            this.sender.Recieve += Sender_Recieve;
            this.sender.Errored += Sender_Errored;
            this.sender.Start();

            this.reciever = new RecieveClient();
            this.reciever.Recieve += Reciever_Recieve;
            this.reciever.Errored += Reciever_Errored;

            this.packetDispatcher = new PacketDispatcher(this);

            this.completionQueue = new Queue<TaskCompletionSource<Enums.FehlerT>>();

            var sync = new SKMSync(steckbrief, SKMType);
            SendToConsole(MakePacket(sync.GetDataToSend(console), sync.Type));
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
            }
            catch (Exception e)
            {
                args.ResponseCode = Enums.Response.BadCmd;
                console.Logger?.Log(ByteUtils.ArrayToString(args.Data));
                OnErrored(this, e);
                console.Logger?.Log(e.StackTrace);
            }
        }

        private void Sender_Errored(object sender, Exception e)
        {
            OnErrored(this, e);
        }

        private void Sender_Recieve(object sender, byte[] e)
        {

            ByteBuffer buf = new ByteBuffer(e);
            Enums.FehlerT fehler = Enums.GetEnum<Enums.FehlerT>(buf.ReadUInt());


            if (completionQueue.Count > 0)
            {
                TaskCompletionSource<Enums.FehlerT> res = completionQueue.Dequeue();
                res.SetResult(fehler);
            }
        }

        private void SendToConsole(byte[] data)
        {
            sender.SendData(data);
        }

        public void SendRawData(byte[] arr)
        {
            SendToConsole(arr);
        }

        [Obsolete("Use SendPacketAsync instead", false)]
        public void SendPacket(byte[] data, short type)
        {
            SendToConsole(MakePacket(data, type));
        }

        [Obsolete("Use SendPacketAsync instead", false)]
        public void SendPacket(CPacket header)
        {
            byte[] arr = MakePacket(header.GetDataToSend(console), header.Type);

            SendToConsole(arr);
        }

        [Obsolete("Use SendPacketAsync instead", false)]
        public void SendPacket(SplittableHeader header)
        {
            foreach (byte[] arr in header.GetData(console))
            {
                byte[] arrOut = MakePacket(arr, header.Type);
                SendToConsole(arrOut);
            }
        }

        private byte[] MakePacket(byte[] data, short type)
        {
            ByteBuffer buf = new ByteBuffer();
            buf.Write(MAGIC_NUMBER).Write(type).Write(GetLocalIPAddress()).Write(data);
            return buf.ToArray();
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
                    return ip.GetAddressBytes();
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public event EventHandler<Exception> Errored;
        public void OnErrored(object sender, Exception data) { Errored?.Invoke(sender, data); }

        public event EventHandler<PacketRecievedEventArgs> PacketReceived;
        public void OnPacketRecieved(PacketDispatcher sender, PacketRecievedEventArgs args) { PacketReceived?.Invoke(sender, args); }

        public async Task<Enums.FehlerT> SendPacketAsync(CPacket packet)
        {
            TaskCompletionSource<Enums.FehlerT> src = new TaskCompletionSource<Enums.FehlerT>();
            completionQueue.Enqueue(src);

            byte[] arr = MakePacket(packet.GetDataToSend(console), packet.Type);

            SendToConsole(arr);
            return await src.Task;
        }

        public async Task<Enums.FehlerT> SendPacketAsync(byte[] data, short type)
        {
            TaskCompletionSource<Enums.FehlerT> src = new TaskCompletionSource<Enums.FehlerT>();
            completionQueue.Enqueue(src);

            byte[] arr = MakePacket(data, type);

            SendToConsole(arr);
            return await src.Task;
        }

        public async Task<Enums.FehlerT> SendPacketAsync(SplittableHeader header)
        {
            TaskCompletionSource<Enums.FehlerT> src = new TaskCompletionSource<Enums.FehlerT>();
            completionQueue.Enqueue(src);

            foreach (byte[] arr in header.GetData(console))
            {
                byte[] arrOut = MakePacket(arr, header.Type);
                SendToConsole(arrOut);
            }

            return await src.Task;
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

    public class PacketRecievedEventArgs
    {
        public Enums.Type type;
        public SPacket packet;
        public Enums.Response response = Enums.Response.OK;

        public PacketRecievedEventArgs(Enums.Type type, SPacket packet)
        {
            this.type = type;
            this.packet = packet;
        }
    }
}
