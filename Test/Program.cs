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
using NiL.JS.Core;
using Scripting;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using SKMNET.Client.Rendering;
using SKMNET.Client.Stromkreise.ML;
using static SKMNET.Enums;
using MacroTest;
using EffectSystem;
using EffectSystem.Effects;

namespace Test
{
    internal static class Program
    {
        private static Stopwatch stopwatch;

        private static void Main(string[] _)
        {
            MainAsync().Wait();
            Console.ReadLine();
        }

        private static async Task MainAsync()
        {
            stopwatch = new Stopwatch();
            LightingConsole console = new LightingConsole("127.0.0.1",
                                                          LightingConsole.ConsoleSettings.All(logger: new ConsoleLogger()));

            console.Errored += Console_Errored;
            console.Connection.PacketReceived += Connection_PacketReceived;

            Console.ReadLine();
            EffectManager manager = new EffectManager(new List<EffectInfo>()
            {
                new EffectInfo<DimmerFade>()
                {
                    Key = ConsoleKey.D
                }
            });
            manager.BlockThread(console);

            Console.ReadLine();

        }

        private static void Print(ushort u)
        {
            Console.WriteLine(u);
        }

        private static void Print(Enums.FehlerT fehler)
        {
            stopwatch.Stop();
            Console.WriteLine($"Delay: {stopwatch.Elapsed.TotalMilliseconds}ms\nResponse: {fehler.ToString()}");
            stopwatch.Reset();
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

        private class ConsoleLogger : ILogger
        {
            public void Log(object toLog)
            {
                Console.WriteLine(toLog);
            }
        }
    }
}
