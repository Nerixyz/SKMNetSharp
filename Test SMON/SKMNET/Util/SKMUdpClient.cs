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
    class SKMUdpClient
    {
        IPEndPoint endPoint;
        UdpClient baseClient;
        Thread readThread;
        public bool local;
        
        public SKMUdpClient(IPEndPoint endPoint)
        {
            byte[] adress = endPoint.Address.GetAddressBytes();
            local = adress[0] == 127 && adress[1] == 0 && adress[2] == 0 && adress[3] == 1;
            this.endPoint = endPoint;
            readThread = new Thread(() =>
            {
                try
                {
                    Console.WriteLine(endPoint.ToString() + " -> " + baseClient.Available);
                    while (true)
                    {
                        byte[] data = baseClient.Receive(ref endPoint);
                        if(data != null)
                        {
                            OnRecieveBasePort(data);
                        }
                    }
                }catch(Exception e)
                {
                    OnErrored(e);
                }
            });
        }

        public void Start()
        {
            baseClient = new UdpClient();
            baseClient.Connect(endPoint);
            readThread.Start();
        }

        public event EventHandler<byte[]> Recieve;
        protected virtual void OnRecieveBasePort(byte[] data) { Recieve?.Invoke(this, data); }

        public event EventHandler<Exception> Errored;
        protected virtual void OnErrored(Exception data) { Errored?.Invoke(this, data); }

        public void SendData(byte[] data)
        {
            baseClient.Send(data, data.Length);
        }

        public void SendData(ISendable data)
        {
            SendData(data.GetDataToSend());
        }
    }
}
