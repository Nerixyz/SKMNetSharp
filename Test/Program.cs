using Newtonsoft.Json;
using SKMNET;
using SKMNET.Client;
using SKMNET.Client.Networking;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using SKMNET.Client.Networking.Server.T98;
using SKMNET.Exceptions;
using System.Threading.Tasks;
using System.Timers;
using SKMNET.Client.Networking.Client;
using SKMNET.Client.Networking.Client.TSD;
using SKMNET.Client.Networking.Server;
using SKMNET.Client.Stromkreise.ML;
using SKMNET.Util;
using static SKMNET.Enums;
using Type = System.Type;

namespace Test
{
    internal static class Program
    {
        private static Stopwatch _stopwatch;

        private static void Main(string[] _) => MainAsync().Wait();

        private static async Task MainAsync()
        {
            _stopwatch = new Stopwatch();
            LightingConsole console = new LightingConsole(
                Environment.GetEnvironmentVariable("SKM_IP") ?? "127.0.0.1",
                LightingConsole.ConsoleSettings.All(
                    logger: new ConsoleLogger()
                    )
                );

            console.Errored += Console_Errored;
            console.Connection.PacketReceived += Connection_PacketReceived;

            Console.ReadLine();
            await Print(console.QueryAsync(
                new PalCommand(commands: new PalCommand.PalCmdEntry(MlUtil.MlPalFlag.BLK, 10))));
            Console.ReadLine();
        }

        /// <summary>
        /// Prints delay and response
        /// </summary>
        /// <param name="fehler">Request</param>
        /// <returns>void</returns>
        private static async Task Print(Task<FehlerT> fehler)
        {
            _stopwatch.Start();
            FehlerT f = await fehler;
            _stopwatch.Stop();
            Console.WriteLine($"Delay: {_stopwatch.Elapsed.TotalMilliseconds}ms\nResponse: {f.ToString()}");
            _stopwatch.Reset();
        }

        private static void Connection_PacketReceived(object sender, PacketReceivedEventArgs args)
        {
            Console.WriteLine($"received {(int) args.Type} - {args.Packet.GetType().Name}");
            if (args.Packet is SKRegData)
            {
                Console.WriteLine(JsonConvert.SerializeObject(args.Packet));
            }
        }

        public static byte[] FromHex(string hex)
        {
            byte[] raw = new byte[hex.Length / 2];
            for (int i = 0; i < raw.Length; i++)
                raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            return raw;
        }

        private static void Console_Errored(object sender, Exception e)
        {
            Console.WriteLine("ERROR:\n" + e.Message + "\n" + e.Source + "\n" + e.StackTrace);
            if (!(e is UnknownSKMPacketException)) return;
            UnknownSKMPacketException exception = (UnknownSKMPacketException) e;
            Console.WriteLine(exception.Remaining.ToHexString());
        }

        private class ConsoleLogger : ILogger
        {
            public void Log(object toLog) => Console.WriteLine(toLog);
        }
    }
}