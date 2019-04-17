using Newtonsoft.Json;
using SKMNET.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MacroTest
{
    public class MacroManager
    {
        public List<Macro> Macros { get; set; }

        public MacroManager()
        {
            Macros = new List<Macro>();
        }

        public void LoadJSON(string path)
        {
            Macros.Add(JsonConvert.DeserializeObject<Macro>(File.ReadAllText(path)));
        }

        public void BlockThread(LightingConsole console)
        {
            ConsoleKeyInfo info;
            while((info = Console.ReadKey(true)).Key != ConsoleKey.Escape){
                Macro macro = Macros.Find((x) => x.Key == info.Key);
                macro?.Execute(console);
            }
        }
    }
}
