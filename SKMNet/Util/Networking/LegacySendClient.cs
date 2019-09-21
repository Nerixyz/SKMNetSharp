using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using SKMNET.Client;
using SKMNET.Exceptions;

namespace SKMNET.Util.Networking
{
    public class LegacySendClient: ISendClient
    {
        public IPEndPoint IpEndPoint
        {
            get => ipEndPoint;
            private set => ipEndPoint = value;
        }

        private readonly UdpClient baseClient;
        private Thread readThread;
        public bool Local { get; private set; }
        public bool Run = true;
        private IPEndPoint ipEndPoint;

        public LegacySendClient()
        {
            baseClient = new UdpClient
            {
                EnableBroadcast = true
            };
        }

        public void Start(IPEndPoint endPoint)
        {
            try
            {
                IpEndPoint = endPoint;
                byte[] address = endPoint.Address.GetAddressBytes();
                Local = address[0] == 127 && address[1] == 0 && address[2] == 0 && address[3] == 1;
                readThread = new Thread(() =>
                {
                    try
                    {
                        while (Run)
                        {
                            byte[] data = baseClient.Receive(ref ipEndPoint);
                            Receive(data);
                        }
                    }catch(Exception e)
                    {
                        OnErrored(e);
                    }
                });
                baseClient.Connect(IpEndPoint);
                readThread.Start();
            }catch (Exception e)
            {
                throw new SKMConnectException(endPoint, "Could not connect.", e);
            }
        }

        public Action<byte[]> Receive { get; set; }
        public event EventHandler<Exception> Errored;
        private void OnErrored(Exception data) { Errored?.Invoke(this, data); }

        public void SendData(byte[] data)
        {
            baseClient.Send(data, data.Length);
        }

        public void SendData(ISendable data, LightingConsole console)
        {
            SendData(data.GetDataToSend(console));
        }

        public void Dispose()
        {
            Run = false;
            readThread.Join();
            baseClient?.Dispose();
        }
    }
}
