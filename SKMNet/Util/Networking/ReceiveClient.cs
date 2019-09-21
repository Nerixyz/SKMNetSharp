using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using SKMNET.Exceptions;

namespace SKMNET.Util.Networking
{
    public class ReceiveClient : IReceiveClient
    {
        private const int T90_TO_SKM_PORT = 5064;

        private IPEndPoint ipEndPoint;

        private readonly UdpClient udpClient;

        private AsyncCallback receiveCallback;

        public ReceiveClient()
        {
            try
            {
                udpClient = new UdpClient(T90_TO_SKM_PORT) {EnableBroadcast = true};
                Closing = false;
            }
            catch (SocketException ex)
            {
                throw new SKMConnectException(new IPEndPoint(0, T90_TO_SKM_PORT),
                    $"Failed binding to port {T90_TO_SKM_PORT.ToString()}", ex);
            }
        }

        private bool Closing { get; set; }

        public void Dispose()
        {
            Closing = true;
            udpClient.Close();
        }

        public Action<ReceiveEventArgs> Receive { get; set; }
        public event EventHandler<Exception> Errored;

        public void Start(IPEndPoint endPoint)
        {
            ipEndPoint = endPoint ?? new IPEndPoint(IPAddress.Any, 0);
            udpClient.BeginReceive(receiveCallback = ReceiveCallback, new EbReceiveState
            {
                UdpClient = udpClient,
                Receive = Receive,
                Sender = this,
                EndPoint = ipEndPoint,
                AsyncReceiveCallback = receiveCallback,
            });
        }

        private static void ReceiveCallback(IAsyncResult result)
        {
            if(result == null) return;
            
            byte[] receivedBytes = null;
            EbReceiveState state = (EbReceiveState) result.AsyncState;
            try
            {
                receivedBytes = state.UdpClient.EndReceive(result, ref state.EndPoint);
            }
            catch (ObjectDisposedException)
            {
            }

            if (receivedBytes?.Length > 0)
            {
                ReceiveEventArgs eventArgs = new ReceiveEventArgs(receivedBytes, state.EndPoint);

                if (state.Receive != null)
                {
                    Task.Factory.StartNew(() => state.Receive(eventArgs)).ContinueWith((task =>
                    {
                        byte[] toWrite = new ByteBuffer().Write((int) eventArgs.ResponseCode).ToArray();
                        state.UdpClient.Send(toWrite, toWrite.Length, state.EndPoint);
                    }));
                }
            }
            
            if (!state.Sender.Closing)
            {
                state.UdpClient.BeginReceive(state.AsyncReceiveCallback, state);
            }
        }

        private class EbReceiveState
        {
            public UdpClient UdpClient;
            public Action<ReceiveEventArgs> Receive;
            public ReceiveClient Sender;
            public IPEndPoint EndPoint;
            public AsyncCallback AsyncReceiveCallback;
        }
    }
}