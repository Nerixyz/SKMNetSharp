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
using SKMNET.Client.Networking.Server;
using static SKMNET.Enums;
using Type = System.Type;

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
            LightingConsole console = new LightingConsole(
                "127.0.0.1",
                LightingConsole.ConsoleSettings.All(
                    logger: new ConsoleLogger()
                    )
                );

            console.Errored += Console_Errored;
            console.Connection.PacketReceived += Connection_PacketReceived;

            Console.ReadLine();
            _stopwatch.Start();
            Print(
                await Examples.FunctionIntensity(
                    console, 
                    x => (byte)Math.Min(x % 100 * 2.55, 255.0),
                    Enumerable
                        .Range(0, 200)
                        .Select(i => (short)i)
                        .ToArray()
                    )
                );
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
            Console.WriteLine("received " + (int) args.Type + " - " + args.Packet.GetType().Name);
            if (args.Packet is SKRegData)
            {
                Console.WriteLine(JsonConvert.SerializeObject(args.Packet));
            }
        }

        public static byte[] FromHex(string hex)
        {
            byte[] raw = new byte[hex.Length / 2];
            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }

            return raw;
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