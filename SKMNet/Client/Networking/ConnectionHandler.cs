﻿using SKMNET.Client.Networking.Client;
using SKMNET.Client.Networking.Server;
using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking
{
    public class ConnectionHandler
    {
        private const ushort MAGIC_NUMBER = 0x1fe2;
        private readonly SendClient sendClient;
        private readonly ReceiveClient receiveClient;
        public readonly LightingConsole Console;
        //private readonly Thread sendThread;
        private readonly PacketDispatcher packetDispatcher;

        private readonly Queue<TaskCompletionSource<Enums.FehlerT>> completionQueue;


        public ConnectionHandler(string ipAddress, LightingConsole parent, SKMSteckbrief steckbrief, byte SKMType)
        {
            Console = parent;

            sendClient = new SendClient(new IPEndPoint(IPAddress.Parse(ipAddress), 5063));
            sendClient.Receive += SendClientReceive;
            sendClient.Errored += SendClientErrored;
            sendClient.Start();

            receiveClient = new ReceiveClient();
            receiveClient.Recieve += ReceiveClientReceive;
            receiveClient.Errored += ReceiveClientErrored;

            packetDispatcher = new PacketDispatcher(this);

            completionQueue = new Queue<TaskCompletionSource<Enums.FehlerT>>();

            var sync = new SKMSync(steckbrief, SKMType);
            SendToConsole(MakePacket(sync.GetDataToSend(Console), sync.Type));
        }

        private void ReceiveClientErrored(object sender, Exception e) => OnErrored(this, e);

        private void ReceiveClientReceive(object sender, ReceiveClient.RecieveEventArgs args)
        {
            try
            {
                args.ResponseCode = packetDispatcher.OnDataIncoming(args.Data);
            }
            catch (Exception e)
            {
                args.ResponseCode = Enums.Response.BadCmd;
                Console.Logger?.Log(ByteUtils.ArrayToString(args.Data));
                OnErrored(this, e);
                Console.Logger?.Log(e.StackTrace);
            }
        }

        private void SendClientErrored(object sender, Exception e)
        {
            OnErrored(this, e);
        }

        private void SendClientReceive(object sender, byte[] e)
        {

            ByteBuffer buf = new ByteBuffer(e);
            Enums.FehlerT fehler = Enums.GetEnum<Enums.FehlerT>(buf.ReadUInt());

            if (completionQueue.Count <= 0) return;
            
            TaskCompletionSource<Enums.FehlerT> res = completionQueue.Dequeue();
            res?.SetResult(fehler);
        }

        private void SendToConsole(byte[] data) => sendClient.SendData(data);

        public void SendRawData(byte[] arr) => SendToConsole(arr);

        public void SendPacket(byte[] data, short type) => SendToConsole(MakePacket(data, type));

        public void SendPacket(CPacket header)
        {
            byte[] arr = MakePacket(header.GetDataToSend(Console), header.Type);

            SendToConsole(arr);
        }

        public void SendPacket(SplittableHeader header)
        {
            foreach (byte[] arr in header.GetData(Console))
            {
                byte[] arrOut = MakePacket(arr, header.Type);
                SendToConsole(arrOut);
            }
        }

        private byte[] MakePacket(byte[] data, short type)
        {
            ByteBuffer buf = new ByteBuffer();
            buf.Write(MAGIC_NUMBER).Write(type).Write(GetLocalIpAddress()).Write(data);
            return buf.ToArray();
        }

        private byte[] GetLocalIpAddress()
        {
            if (sendClient.Local) return new byte[] { 127, 0, 0, 1 };
            
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip.GetAddressBytes();
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public async Task<Enums.FehlerT> SendPacketAsync(CPacket packet)
        {
            TaskCompletionSource<Enums.FehlerT> src = new TaskCompletionSource<Enums.FehlerT>();
            completionQueue.Enqueue(src);

            byte[] arr = MakePacket(packet.GetDataToSend(Console), packet.Type);

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

            foreach (byte[] arr in header.GetData(Console))
            {
                byte[] arrOut = MakePacket(arr, header.Type);
                SendToConsole(arrOut);
            }

            return await src.Task;
        }
        
        public event EventHandler<Exception> Errored;
        public void OnErrored(object sender, Exception data) { Errored?.Invoke(sender, data); }

        public event EventHandler<PacketReceivedEventArgs> PacketReceived;
        public void OnPacketReceived(PacketDispatcher sender, PacketReceivedEventArgs args) { PacketReceived?.Invoke(sender, args); }
    }

    public class PacketReceivedEventArgs
    {
        public readonly Enums.Type Type;
        public readonly SPacket Packet;
        public Enums.Response Response = Enums.Response.OK;

        public PacketReceivedEventArgs(Enums.Type type, SPacket packet)
        {
            Type = type;
            Packet = packet;
        }
    }
}
