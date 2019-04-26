using System;
using System.Net;

namespace SKMNET.Exceptions
{
    public class SKMConnectException : Exception
    {
        public IPEndPoint EndPoint { get; }

        public SKMConnectException(IPEndPoint endPoint, string message, Exception innerException) : base(message, innerException)
        {
            EndPoint = endPoint;
        }
    }
}
