using SKMNET.Client.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Tasten
{
    [Serializable]
    public class TastenManager
    {
        public List<Taste> Tasten { get; }

        [NonSerialized]
        private readonly LightingConsole console;

        public TastenManager(LightingConsole console)
        {
            this.console = console;
            this.Tasten = new List<Taste>();
        }

        public void Add(Taste taste)
        {
            Tasten.Add(taste);
        }

        public Taste FindByName(string name)
        {
            return Tasten.Find((x) => x.Name.ToLower().Equals(name.ToLower()));
        }

        public Taste FindByNumber(int number)
        {
            return Tasten.Find((x) => x.TastNR == number);
        }
    }
}
