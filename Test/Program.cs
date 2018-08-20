using System;
using SKMNET;
using SKMNET.Client;
using SKMNET.Client.Networking;
using SKMNET.Client.Networking.Client;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.Threading;
using SKMNET.Client.Stromkreise.ML;

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

            byte[] data = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0xff};
            console.Query(data, 24, new Action<byte[]>((arr) => 
            {
                Console.WriteLine($"Response: {ByteUtils.ArrayToString(arr)} ");
            }));

            Console.ReadLine();
            string json = JsonConvert.SerializeObject(console);
            Clipboard.SetText(json);
            Console.WriteLine("Copied!");
            Console.ReadLine();
        }

        private static void Connection_PacketRecieved(object sender, PacketRecievedEventArgs args)
        {
            Console.WriteLine("recieved " + (int)args.type + " - " + args.packet.GetType().Name);
        }

        private static void Console_Errored(object sender, Exception e)
        {
            Console.WriteLine("!ERROR!\n" + e.Message + "\n" + e.Source + "\n" + e.StackTrace);
        }
    }
}
