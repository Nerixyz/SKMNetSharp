using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using SKMNET.Exceptions;

namespace SKMNET.Util.Networking
{
    public class LegacyReceiveClient : IReceiveClient
    {
        private const int T90_TO_SKM_PORT = 5064;
        private bool dropConnection = false;
        private Thread readThread;
        private readonly UdpClient udpClient;

        public LegacyReceiveClient()
        {
            try
            {
                udpClient = new UdpClient(T90_TO_SKM_PORT)
                {
                    EnableBroadcast = true
                };
            }
            catch(Exception e)
            {
                throw new SKMConnectException(new IPEndPoint(0x0, T90_TO_SKM_PORT), "Could not bind to port " + T90_TO_SKM_PORT, e);
            }
        }

        public Action<ReceiveEventArgs> Receive { get; set; }
        public event EventHandler<Exception> Errored;
        public void Start(IPEndPoint _)
        {
            readThread = new Thread(() =>
            {
                IPEndPoint endPoint = null;
                while (!dropConnection)
                {
                    byte[] data = udpClient.Receive(ref endPoint);

                    ReceiveEventArgs eventArgs = new ReceiveEventArgs(data, endPoint);
                    IPEndPoint point = endPoint;
                    Task.Run(() =>
                    {
                        Receive(eventArgs);
                        byte[] arr = new ByteBuffer().Write((int)eventArgs.ResponseCode).ToArray();
                        udpClient.Send(arr, arr.Length, point);
                    });

                }
            });

            readThread.Start();
        }

        private void OnErrored(Exception data) { Errored?.Invoke(this, data); }

        public void Dispose()
        {
            dropConnection = true;
            readThread.Join();
            udpClient.Dispose();
        }
    }
}
