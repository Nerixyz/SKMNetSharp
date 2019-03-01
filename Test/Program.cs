using Newtonsoft.Json;
using SKMNET;
using SKMNET.Client;
using SKMNET.Client.Networking;
using SKMNET.Client.Networking.Client;
using SKMNET.Client.Networking.Server.TSD;
using SKMNET.Logging;
using System;
using CoreClipboard;
using System.Threading;
using System.Diagnostics;

namespace Test
{
    class Program
    {

        private static Stopwatch stopwatch;

        [STAThread]
        static void Main(string[] args)
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
                    Bedienstelle = Enums.Bedienstelle.Infrarot,
                    SKMType = 2
                }
            );

            console.Errored += Console_Errored;
            console.Connection.PacketReceived += Connection_PacketReceived;

            while (true)
            {
                string cmd = Console.ReadLine();
                if (cmd.Equals("end")) break;

                stopwatch.Reset();
                stopwatch.Start();

                string[] dat = cmd.Split(':');

                console.GetSKByNumber(short.Parse(dat[0]), entireSet: true).Intensity = byte.Parse(dat[1]);
                console.PushChanges(callback: BASIC_CALLBACK, src: console.ActiveSK);

            }

            console.Query(new PalCommand(new PalCommand.PalCmdEntry(SKMNET.Util.MLUtil.MLPalFlag.BLK, console.Paletten[SKMNET.Client.Stromkreise.ML.MLPal.MLPalFlag.BLK][0].PalNO)), BASIC_CALLBACK);

            Console.ReadLine();
            Console.WriteLine(JsonConvert.SerializeObject(console));
            Console.ReadLine();

            Console.WriteLine("start");
            Console.ReadLine();
        }

        private static void VALUE_HANDLER(double value)
        {
            Console.WriteLine(value);
        }

        private static void Connection_PacketReceived(object sender, PacketRecievedEventArgs args)
        {
            Console.WriteLine("received " + (int)args.type + " - " + args.packet.GetType().Name);
        }

        private static void Console_Errored(object sender, Exception e)
        {
            Console.WriteLine("ERROR:\n" + e.Message + "\n" + e.Source + "\n" + e.StackTrace);
        }

        private readonly static Action<Enums.FehlerT> BASIC_CALLBACK = new Action<Enums.FehlerT>((fehler) =>
        {
            stopwatch.Stop();
            Console.WriteLine($"Delay: {stopwatch.Elapsed.TotalMilliseconds}ms");
            Console.WriteLine($"Response: {fehler.ToString()} ");
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
