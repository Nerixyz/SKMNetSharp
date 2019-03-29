using Newtonsoft.Json;
using SKMNET;
using SKMNET.Client;
using SKMNET.Client.Networking;
using SKMNET.Client.Networking.Client;
using SKMNET.Logging;
using System;
using CoreClipboard;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using SKMNET.Client.Stromkreise;
using SKMNET.Client.Networking.Server.T98;
using SKMNET.Exceptions;
using System.Reflection;
using ConsoleUI;

namespace Test
{
    internal static class Program
    {
        private static Stopwatch stopwatch;

        [STAThread]
        private static void Main(string[] _)
        {
            stopwatch = new Stopwatch();
            LightingConsole console = new LightingConsole(
                "127.0.0.1",
                new LightingConsole.ConsoleSettings() {
                    Bedientasten = true,
                    AktInfo = true,
                    AZ_Zeilen = true,
                    BefMeldZeile = true,
                    BlockInfo = true,
                    ExtKeys = true,
                    FuncKeys = true,
                    LKI = true,
                    Steller = true,

                    Logger = new ConsoleLogger(),
                    Bedienstelle = Enums.Bedienstelle.Libra,
                    SKMType = 2
                }
            );

            console.Errored += Console_Errored;
            console.Connection.PacketReceived += Connection_PacketReceived;

            Console.ReadLine();

            new ConsoleInterface().StartAndBlock(console, BASIC_CALLBACK, ()=> stopwatch.Start());

            Console.ReadLine();
            //Console.WriteLine(JsonConvert.SerializeObject(console.TastenManager.Tasten));
            Console.ReadLine();

            Console.WriteLine("start");
            Console.ReadLine();
        }

        private static void Connection_PacketReceived(object sender, PacketRecievedEventArgs args)
        {
            Console.WriteLine("received " + (int)args.type + " - " + args.packet.GetType().Name);
            if (args.packet is SKRegData)
            {
                Console.WriteLine(JsonConvert.SerializeObject(args.packet));
            }
        }

        private static void Console_Errored(object sender, Exception e)
        {
            Console.WriteLine("ERROR:\n" + e.Message + "\n" + e.Source + "\n" + e.StackTrace);
            if(e is UnknownSKMPacketException)
            {
                UnknownSKMPacketException exception = e as UnknownSKMPacketException;
                Console.WriteLine(ByteUtils.ArrayToString(exception.Remaining));
            }
        }

        private readonly static Action<Enums.FehlerT> BASIC_CALLBACK = new Action<Enums.FehlerT>((fehler) =>
        {
            stopwatch.Stop();
            Console.WriteLine($"Delay: {stopwatch.Elapsed.TotalMilliseconds}ms");
            Console.WriteLine($"Response: {fehler.ToString()} ");

            stopwatch.Reset();
        });

        private class ConsoleLogger : ILogger
        {
            public void Log(object toLog)
            {
                Console.WriteLine(toLog);
            }
        }
    }
}
