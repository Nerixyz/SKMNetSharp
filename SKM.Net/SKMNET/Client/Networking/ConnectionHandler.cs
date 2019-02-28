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
        private          SendClient      sender;
        private readonly RecieveClient   reciever;
        public  readonly LightingConsole console;
        private readonly Thread sendThread;
        private readonly PacketDispatcher packetDispatcher;
        
        private Action<Enums.FehlerT> queuedAction;


        public ConnectionHandler(string ipAdress, LightingConsole parent, ref SKMSteckbrief steckbrief)
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
            
            SendPacket(new SKMSync(steckbrief));
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

            queuedAction?.Invoke(fehler);
            queuedAction = null;
        }

        private void SendToConsole(byte[] data)
        {
            sender.SendData(data);
        }

        public void SendRawData(byte[] arr)
        {
            SendToConsole(arr);
        }

        public void SendPacket(byte[] data, short type, Action<Enums.FehlerT> callback = null)
        {
            ByteBuffer buf = new ByteBuffer();
            buf.Write(MAGIC_NUMBER).Write(type).Write(GetLocalIPAddress()).Write(data);
            SendToConsole(buf.ToArray());
            this.queuedAction = callback;
        }

        public void SendPacket(CPacket header, Action<Enums.FehlerT> callback = null)
        {
            if (header is SKMSync)
            {
                console.Logger?.Log(ByteUtils.ArrayToString(header.GetDataToSend(console)));
            }
            ByteBuffer buf = new ByteBuffer();
            buf.Write(MAGIC_NUMBER).Write(header.Type).Write(GetLocalIPAddress()).Write(header.GetDataToSend(console));
            byte[] arr = buf.ToArray();
            console.Logger?.Log(ByteUtils.ArrayToString(arr));

            SendToConsole(arr);
            this.queuedAction = callback;
        }

        public void SendPacket(SplittableHeader header, Action<Enums.FehlerT> callback = null)
        {
            foreach (byte[] arr in header.GetData(console))
            {
                ByteBuffer buf = new ByteBuffer();
                buf.Write(MAGIC_NUMBER).Write(header.Type).Write(GetLocalIPAddress()).Write(arr);
                byte[] arrOut = buf.ToArray();
                SendToConsole(arrOut);
            }
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

        public event EventHandler<PacketRecievedEventArgs> PacketReceived;
        public void OnPacketRecieved(PacketDispatcher sender, PacketRecievedEventArgs args) { PacketReceived?.Invoke(sender, args); }

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
