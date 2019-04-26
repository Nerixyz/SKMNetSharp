using SKMNET.Client;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using SKMNET.Exceptions;

namespace SKMNET.Util
{
    internal class SendClient
    {
        private readonly IPEndPoint endPoint;
        private readonly UdpClient baseClient;
        private readonly Thread readThread;
        public readonly bool Local;

        public SendClient(IPEndPoint endPoint)
        {
            byte[] adress = endPoint.Address.GetAddressBytes();
            Local = adress[0] == 127 && adress[1] == 0 && adress[2] == 0 && adress[3] == 1;
            this.endPoint = endPoint;

            baseClient = new UdpClient
            {
                EnableBroadcast = true
            };

            readThread = new Thread(() =>
            {
                try
                {
                    while (true)
                    {
                        byte[] data = baseClient.Receive(ref endPoint);
                        OnReceiveBasePort(data);
                    }
                }catch(Exception e)
                {
                    OnErrored(e);
                }
            });
        }

        public void Start()
        {
            try
            {
                baseClient.Connect(endPoint);
                readThread.Start();
            }catch (Exception e)
            {
                throw new SKMConnectException(endPoint, "Could not connect.", e);
            }
        }

        public event EventHandler<byte[]> Receive;
        protected virtual void OnReceiveBasePort(byte[] data) { Receive?.Invoke(this, data); }

        public event EventHandler<Exception> Errored;
        protected virtual void OnErrored(Exception data) { Errored?.Invoke(this, data); }

        public void SendData(byte[] data)
        {
            baseClient.Send(data, data.Length);
        }

        public void SendData(ISendable data, LightingConsole console)
        {
            SendData(data.GetDataToSend(console));
        }
    }
}
