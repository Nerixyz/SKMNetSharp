using SKMNET.Exceptions;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace SKMNET.Util
{
    internal sealed class ReceiveClient
    {
        private const int T90_TO_SKM_PORT = 5064;
        private bool dropConnection = false;

        public ReceiveClient()
        {
            UdpClient client;
            try
            {
                client = new UdpClient(T90_TO_SKM_PORT)
                {
                    EnableBroadcast = true
                };
            }
            catch(Exception e)
            {
                throw new SKMConnectException(new IPEndPoint(0x0, T90_TO_SKM_PORT), "Could not bind to port " + T90_TO_SKM_PORT, e);
            }
            Thread readThread = new Thread(() =>
            {
                IPEndPoint endPoint = null;
                while (!dropConnection)
                {
                    byte[] data = client.Receive(ref endPoint);

                    RecieveEventArgs eventArgs = new RecieveEventArgs(data, endPoint);
                    IPEndPoint point = endPoint;
                    Task.Run(() =>
                    {
                        Recieve?.Invoke(this, eventArgs);
                        byte[] arr = new ByteBuffer().Write((int)eventArgs.ResponseCode).ToArray();
                        client.Send(arr, arr.Length, point);
                    });

                }
            });

            readThread.Start();
        }

        public event EventHandler<RecieveEventArgs> Recieve;

        public event EventHandler<Exception> Errored;
        private void OnErrored(Exception data) { Errored?.Invoke(this, data); }

        public class RecieveEventArgs
        {
            public byte[] Data { get; }
            public Enums.Response ResponseCode;
            public bool DropConnection;
            public IPEndPoint EndPoint;

            public RecieveEventArgs(byte[] data, IPEndPoint endPoint)
            {
                Data = data;
                ResponseCode = Enums.Response.OK;
                DropConnection = false;
                EndPoint = endPoint;
            }
        }

    }
}
