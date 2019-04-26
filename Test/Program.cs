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
            EffectManager manager = new EffectManager(new List<EffectInfo>
            {
                new EffectInfo<DimmerFade>
                {
                    Key = ConsoleKey.D
                },
                new EffectInfo<ColorEffect>
                {
                    Key = ConsoleKey.R,
                    Arguments = new object[] { new []{101, 102, 103, 104}, new EffectColor{Red = 255, Green = 0, Blue = 0}}
                },
                new EffectInfo<ColorEffect>
                {
                    Key = ConsoleKey.G,
                    Arguments = new object[] { new []{101, 102, 103, 104}, new EffectColor{Red = 0, Green = 255, Blue = 0} }
                },
                new EffectInfo<ColorEffect>
                {
                    Key = ConsoleKey.B,
                    Arguments = new object[] { new []{101, 102, 103, 104}, new EffectColor{Red = 0, Green = 0, Blue = 255}}
                }
                
            });
            manager.BlockThread(console);
            

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
