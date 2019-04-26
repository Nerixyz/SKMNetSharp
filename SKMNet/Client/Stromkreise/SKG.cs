using System;

namespace SKMNET.Client.Stromkreise
{
    [Serializable]
    public class SKG
    {
        public ushort Number { get; set; }
        public string Name { get; set; }

        public SKG(ushort number, string name)
        {
            Number = number;
            Name = name;
        }

    }
}
