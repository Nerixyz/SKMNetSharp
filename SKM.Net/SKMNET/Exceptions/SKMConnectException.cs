using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SKMNET.Exceptions
{
    public class SKMConnectException : Exception
    {
        public IPEndPoint EndPoint { get; }
        public SKMConnectException(IPEndPoint endPoint) : base()
        {
            this.EndPoint = endPoint;
        }

        public SKMConnectException(IPEndPoint endPoint, string message) : base(message)
        {
            this.EndPoint = endPoint;
        }

        public SKMConnectException(IPEndPoint endPoint, string message, Exception innerException) : base(message, innerException)
        {
            this.EndPoint = endPoint;
        }
    }
}
