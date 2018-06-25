using Newtonsoft.Json;
using SKMNET.Client;
using SKMNET.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Test_SMON
{
    class Program
    {
        static void Main(string[] args)
        {
            LightingConsole console = new LightingConsole("127.0.0.1");
            Console.ReadLine();
            int i = 1;
            if (i == 1)
            {
                string json = JsonConvert.SerializeObject(console);
                Console.WriteLine(json);
                Console.ReadLine();
            }
            else {
                Console.WriteLine("1");
            }
        }
    }
}
