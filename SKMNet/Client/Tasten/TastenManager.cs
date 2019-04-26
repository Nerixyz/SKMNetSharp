using System;
using System.Collections.Generic;

namespace SKMNET.Client.Tasten
{
    [Serializable]
    public class TastenManager
    {
        public List<Taste> Tasten { get; }


        public TastenManager() => Tasten = new List<Taste>();

        public void Add(Taste taste)          => Tasten.Add(taste);

        public Taste FindByName(string name)  => Tasten.Find(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        public Taste FindByNumber(int number) => Tasten.Find(x => x.TastNR == number);
    }
}
