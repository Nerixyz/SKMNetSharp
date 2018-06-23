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
    class UdpBaseClient
    {
        Socket socket;
        IPEndPoint endPoint;
        Thread readThread;

        public UdpBaseClient(IPEndPoint dst)
        {
            socket = new Socket(SocketType.Dgram, ProtocolType.Udp);
            socket.Bind(dst);
            readThread = new Thread(() =>
            {
                while (true)
                {
                    if (socket.ReceiveBufferSize != 0)
                    {
                        byte[] buffer = new byte[socket.ReceiveBufferSize];
                        socket.Receive(buffer);
                        OnRecieve(buffer);
                    }
                }
            });
        }

        public void Send(ISendable data)
        {
            socket.SendTo(data.GetDataToSend(), endPoint);
        }

        public event EventHandler<byte[]> Recieve;
        protected virtual void OnRecieve(byte[] data) { Recieve?.Invoke(this, data); }

        public event EventHandler<Exception> Errored;
        protected virtual void OnErrored(Exception data) { Errored?.Invoke(this, data); }
    }
}
