using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using SKMNET.Client;

namespace SKMNET.Util.Networking
{
    /// <summary>
    /// EventBasedSendClient
    /// </summary>
    public class SendClient : ISendClient
    {

        private readonly UdpClient udpClient;

        private readonly AsyncCallback receiveCallback;
        private readonly AsyncCallback sendCallback;

        private bool Closing { get; set; }

        public SendClient()
        {
            udpClient = new UdpClient {EnableBroadcast = true};
            receiveCallback = ReceiveCallback;
            sendCallback = SendCallback;
            Closing = false;
        }
        
        public void Dispose()
        {
            Closing = true;
            udpClient.Close();
        }

        public Action<byte[]> Receive { get; set; }
        public event EventHandler<Exception> Errored;
        public void SendData(byte[] data)
        {
            udpClient.BeginSend(data, data.Length, sendCallback, new EbSendState
            {
                UdpClient = udpClient,
            });
        }

        public void SendData(ISendable sendable, LightingConsole console)
        {
            SendData(sendable.GetDataToSend(console));
        }

        public void Start(IPEndPoint ipEndPoint)
        {
            udpClient.Connect(ipEndPoint);
            udpClient.BeginReceive(receiveCallback, new EbReceiveState
            {
                UdpClient = udpClient,
                Receive = Receive,
                EndPoint = new IPEndPoint(IPAddress.Any, 0),
                Sender = this,
                AsyncReceiveCallback = receiveCallback,
            });
        }

        private static void ReceiveCallback(IAsyncResult result)
        {
            if (result == null) return;

            EbReceiveState state = (EbReceiveState) result.AsyncState;
            byte[] receivedBytes = null;
            try
            {
                receivedBytes = state.UdpClient.EndReceive(result, ref state.EndPoint);
            }
            catch (ObjectDisposedException)
            {
            }

            if (receivedBytes?.Length > 0 && state.Receive != null)
                Task.Factory.StartNew(() => state.Receive(receivedBytes));
            
            if (!state.Sender.Closing)
            {
                state.UdpClient.BeginReceive(state.AsyncReceiveCallback, state);
            }
        }
        
        private static void SendCallback(IAsyncResult result)
        {
            if(result == null) return;

            EbSendState state = (EbSendState) result.AsyncState;

            state.UdpClient.EndSend(result);
        }

        private class EbReceiveState
        {
            public UdpClient UdpClient;
            public Action<byte[]> Receive;
            public IPEndPoint EndPoint;
            public SendClient Sender;
            public AsyncCallback AsyncReceiveCallback;
        }

        private class EbSendState
        {
            public UdpClient UdpClient;
        }
    }
}