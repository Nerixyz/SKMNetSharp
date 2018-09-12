﻿using Newtonsoft.Json;
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
        
        private Action<Enums.FehlerT> queuedAction;


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
            
            SendPacket(new SKMSync(SKMSync.Flags.Bedientasten));
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
            bool ret = true;
            foreach (byte b in e) ret = ret && b == 0;
            if (ret)
            {
                queuedAction?.Invoke(Enums.FehlerT.FT_OK);
                queuedAction = null;
                return;
            }

            ByteBuffer buf = new ByteBuffer(e);
            Enums.FehlerT fehler = Enums.GetEnum<Enums.FehlerT>(buf.ReadUInt());

            queuedAction?.Invoke(fehler);
            queuedAction = null;
        }
        
        public void SendPacket(CPacket header)
        {
            if(header is SKMSync)
            {
                Logger.Log(ByteUtils.ArrayToString(header.GetDataToSend()));
            }
            ByteBuffer buf = new ByteBuffer();
            buf.Write(MAGIC_NUMBER).Write(header.Type).Write(GetLocalIPAddress()).Write(header.GetDataToSend());
            sendQueue.Enqueue(buf.ToArray());
        }
        public void SendPacket(SplittableHeader header)
        {
            foreach (byte[] arr in header.GetData())
            {
                ByteBuffer buf = new ByteBuffer();
                buf.Write(MAGIC_NUMBER).Write(header.Type).Write(GetLocalIPAddress()).Write(arr);
                byte[] arrOut = buf.ToArray();
                Console.WriteLine(ByteUtils.ArrayToString(arrOut));
                sendQueue.Enqueue(arrOut);
            }
        }

        public void SendPacket(byte[] data, short type)
        {
            ByteBuffer buf = new ByteBuffer();
            buf.Write(MAGIC_NUMBER).Write(type).Write(GetLocalIPAddress()).Write(data);
            sendQueue.Enqueue(buf.ToArray());
        }

        public void SendPacket(byte[] data, short type, Action<Enums.FehlerT> callback)
        {
            SendPacket(data, type);
            this.queuedAction = callback;
        }

        public void SendRawData(byte[] arr)
        {
            sendQueue.Enqueue(arr);
        }

        public void SendPacket(CPacket header, Action<Enums.FehlerT> callback)
        {
            SendPacket(header);
            this.queuedAction = callback;
        }
        public void SendPacket(SplittableHeader header, Action<Enums.FehlerT> callback)
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