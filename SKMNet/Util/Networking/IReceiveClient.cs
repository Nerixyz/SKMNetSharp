using System;
using System.Net;

namespace SKMNET.Util.Networking
{
    public interface IReceiveClient : IDisposable
    {
        Action<ReceiveEventArgs> Receive { get; set; }
        event EventHandler<Exception> Errored;

        void Start(IPEndPoint ipEndPoint);
    }
    
    public class ReceiveEventArgs
    {
        public byte[] Data { get; }
        public Enums.Response ResponseCode;
        public bool DropConnection;
        public IPEndPoint EndPoint;

        public ReceiveEventArgs(byte[] data, IPEndPoint endPoint)
        {
            Data = data;
            ResponseCode = Enums.Response.OK;
            DropConnection = false;
            EndPoint = endPoint;
        }
    }
}