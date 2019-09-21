using System;
using System.Net;
using SKMNET.Client;

namespace SKMNET.Util.Networking
{
    public interface ISendClient : IDisposable
    {
        Action<byte[]> Receive { get; set; }
        event EventHandler<Exception> Errored;
        void SendData(byte[] data);
        void SendData(ISendable sendable, LightingConsole console);

        void Start(IPEndPoint endPoint);
    }
}