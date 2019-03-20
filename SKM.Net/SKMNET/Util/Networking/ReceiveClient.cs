
using SKMNET.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SKMNET.Util
{
    internal class RecieveClient
    {
        private const int T90_TO_SKM_PORT = 5064;
        private readonly UdpClient client;
        private readonly Thread readThread;
        private bool dropConnection = false;

        public RecieveClient()
        {
            try
            {
                client = new UdpClient(T90_TO_SKM_PORT);
                client.EnableBroadcast = true;
            }catch(Exception e)
            {
                throw new SKMConnectException(new IPEndPoint(0x0, T90_TO_SKM_PORT), "Could not bind to port " + T90_TO_SKM_PORT, e);
            }
            readThread = new Thread(() =>
            {
                IPEndPoint endPoint = null;
                while (!dropConnection)
                {
                    byte[] data = client.Receive(ref endPoint);
                    if (data == null) continue;

                    RecieveEventArgs eventArgs = new RecieveEventArgs(data, endPoint);
                    Task.Run(() =>
                    {
                        Recieve?.Invoke(this, eventArgs);
                        byte[] arr = new ByteBuffer().Write((int)eventArgs.ResponseCode).ToArray();
                        client.Send(arr, arr.Length, endPoint);
                    });

                }
            });

            readThread.Start();
        }

        public event EventHandler<RecieveEventArgs> Recieve;

        public event EventHandler<Exception> Errored;
        protected virtual void OnErrored(Exception data) { Errored?.Invoke(this, data); }

        public class RecieveEventArgs
        {
            public byte[] Data { get; set; }
            public Enums.Response ResponseCode;
            public bool dropConnection;
            public IPEndPoint endPoint;

            public RecieveEventArgs(byte[] data, IPEndPoint endPoint)
            {
                this.Data = data;
                ResponseCode = Enums.Response.OK;
                dropConnection = false;
                this.endPoint = endPoint;
            }
        }

    }
}
