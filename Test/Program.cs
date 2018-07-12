using System;
using SKMNET;
using SKMNET.Client;
using SKMNET.Client.Networking;
using Newtonsoft.Json;
using System.Windows.Forms;
using SKMNET.Client.Networking.Client;

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


            Console.WriteLine("pre");

            console.Query(new SKAnwahl(SKAnwahl.AWType.Abs, new short[] { 1, 2 }), (arr) =>
            {
                Console.WriteLine($"Response: {ByteUtils.ArrayToString(arr)} ");
            });

            Console.WriteLine("post");

            Console.ReadLine();





























            string json = JsonConvert.SerializeObject(console);
            Clipboard.SetText(json);
            /*byte[] arr = new byte[] { 1, 2, 3, 4, 5, 6 };
            ByteBuffer buffer = new ByteBuffer(arr);
            for(int i = 0; i < 3; i++)
            {
                Console.WriteLine(buffer.ReadUShort());
            }*/
            Console.ReadLine();

        }

        private static void Connection_PacketRecieved(object sender, PacketRecievedEventArgs args)
        {
            Console.WriteLine("recieved " + (int)args.type + " - " + Enum.GetName(typeof(Enums.Type), args.type));
        }

        private static void Console_Errored(object sender, Exception e)
        {
            Console.WriteLine("!ERROR!\n" + e.Message + "\n" + e.Source + "\n" + e.StackTrace);
        }
    }
}
