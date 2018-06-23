using SKMNET.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Test_SMON
{
    class Program
    {
        static void Main(string[] args)
        {
            ConnectionHandler handler = new ConnectionHandler("127.0.0.1", null);
            Console.ReadLine();
        }
    }
}
