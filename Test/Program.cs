using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using NAudio.Wave;
using SKMNET;
using SKMNET.Client;
using System.Threading;
using Test.Util;
using NAudio.Dsp;
using SKMNET.Util;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            LightingConsole console = new LightingConsole("127.0.0.1");
            console.Errored += Console_Errored;
            console.Connection.PacketRecieved += Connection_PacketRecieved;
            Console.ReadLine();
        }

        private static void Connection_PacketRecieved(object sender, Enums.Type e)
        {
            Console.WriteLine("recieved " + (int)e + " - " + Enum.GetName(typeof(Enums.Type), e));
        }

        private static void Console_Errored(object sender, Exception e)
        {
            Console.WriteLine("!ERROR!\n" + e.Message + "\n" + e.Source + "\n" + e.StackTrace);
        }
    }
}
