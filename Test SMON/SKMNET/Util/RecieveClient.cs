
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
    class RecieveClient
    {
        private readonly static IPEndPoint T90_TO_SKM_PORT = new IPEndPoint(IPAddress.Any, 5064);
        private readonly Socket socket;
        private readonly Thread readThread;

        public RecieveClient()
        {
            socket = new Socket(SocketType.Dgram, ProtocolType.Udp);
            socket.Bind(T90_TO_SKM_PORT);
            readThread = new Thread(() =>
            {
                try
                {
                    while (true)
                    {
                        byte[] buffer = new byte[socket.ReceiveBufferSize];
                        EndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
                        int dataLength = socket.ReceiveFrom(buffer, ref endPoint);
                        byte[] data = new byte[dataLength];
                        Array.Copy(buffer, data, dataLength);

                        RecieveEventArgs recieveEventArgs = new RecieveEventArgs(data);
                        OnRecieve(recieveEventArgs);

                        byte[] arr = new ByteArrayParser().Add((int)recieveEventArgs.ResponseCode).GetArray();
                        socket.SendTo(arr, endPoint);
                        Thread.Sleep(50);
                    }
                }catch(Exception e)
                {
                    OnErrored(e);
                }
            });
            readThread.Start();
        }

        public event EventHandler<RecieveEventArgs> Recieve;
        protected virtual void OnRecieve(RecieveEventArgs args) { Recieve?.Invoke(this, args); }

        public event EventHandler<Exception> Errored;
        protected virtual void OnErrored(Exception data) { Errored?.Invoke(this, data); }

        public class RecieveEventArgs
        {
            public byte[] Data { get; set; }
            public Enums.Response ResponseCode;

            public RecieveEventArgs(byte[] data)
            {
                this.Data = data;
                ResponseCode = Enums.Response.OK;
            }
        }

    }
}
