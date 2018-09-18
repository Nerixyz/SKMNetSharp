using System;
using SKMNET;
using SKMNET.Client;
using SKMNET.Client.Networking;
using SKMNET.Client.Networking.Client;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.Threading;
using SKMNET.Client.Stromkreise.ML;
using SKMNET.Client.Networking.Server.TSD;

namespace Test
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            LightingConsole console = new LightingConsole("127.0.0.1");

            console.Errored += Console_Errored;
            console.Connection.PacketRecieved += Connection_PacketRecieved;

            Console.ReadLine();

            console.CreateScene("LTX", 18.8, BASIC_CALLBACK);


            Console.ReadLine();
            string json = JsonConvert.SerializeObject(console);
            Clipboard.SetText(json);
            Console.WriteLine("Copied!");
            Console.ReadLine();
        }

        private static void Connection_PacketRecieved(object sender, PacketRecievedEventArgs args)
        {
            Console.WriteLine("recieved " + (int)args.type + " - " + args.packet.GetType().Name);
            if(args.type == Enums.Type.TSD_MPalSelect)
            {
                TSD_MLPal pal = (TSD_MLPal)args.packet;
                Console.WriteLine(JsonConvert.SerializeObject(pal));
            }
        }

        private static void Console_Errored(object sender, Exception e)
        {
            Console.WriteLine("ERROR:\n" + e.Message + "\n" + e.Source + "\n" + e.StackTrace);
        }

        private static Action<Enums.FehlerT> BASIC_CALLBACK = new Action<Enums.FehlerT>((fehler) =>
        {
            Console.WriteLine($"Response: {fehler.ToString()} ");
        });
    }
}
