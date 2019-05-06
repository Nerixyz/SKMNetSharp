using Newtonsoft.Json;
using SKMNET;
using SKMNET.Client;
using SKMNET.Client.Networking;
using SKMNET.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using SKMNET.Client.Networking.Server.T98;
using SKMNET.Exceptions;
using System.Threading.Tasks;
using System.Timers;
using static SKMNET.Enums;

namespace Test
{
    internal static class Program
    {
        private static Stopwatch _stopwatch;

        private static void Main(string[] _)
        {
            MainAsync().Wait();
            Console.ReadLine();
        }

        private static async Task MainAsync()
        {
            _stopwatch = new Stopwatch();
            LightingConsole console = new LightingConsole("127.0.0.1",
                                                          LightingConsole.ConsoleSettings.All(logger: new ConsoleLogger()));

            console.Errored += Console_Errored;
            console.Connection.PacketReceived += Connection_PacketReceived;

            Console.ReadLine();
            Examples.Rainbow(console, 101);
            Console.ReadLine();
        }

        private static void Print(FehlerT fehler)
        {
            _stopwatch.Stop();
            Console.WriteLine($"Delay: {_stopwatch.Elapsed.TotalMilliseconds}ms\nResponse: {fehler.ToString()}");
            _stopwatch.Reset();
        }

        private static void Connection_PacketReceived(object sender, PacketReceivedEventArgs args)
        {
            Console.WriteLine("received " + (int)args.Type + " - " + args.Packet.GetType().Name);
            if (args.Packet is SKRegData)
            {
                Console.WriteLine(JsonConvert.SerializeObject(args.Packet));
            }
        }

        private static void Console_Errored(object sender, Exception e)
        {
            Console.WriteLine("ERROR:\n" + e.Message + "\n" + e.Source + "\n" + e.StackTrace);
            if (!(e is UnknownSKMPacketException)) return;
            UnknownSKMPacketException exception = (UnknownSKMPacketException) e;
            Console.WriteLine(ByteUtils.ArrayToString(exception.Remaining));
        }
        private class ConsoleLogger : ILogger
        {
            public void Log(object toLog)
            {
                Console.WriteLine(toLog);
            }
        }
    }
}
