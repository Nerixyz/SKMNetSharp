using System;

namespace SKMNET.Client.Vorstellungen
{
    [Serializable]
    public class Vorstellung
    {
        public ushort Number { get; }

        public Vorstellung(ushort number) => Number = number;
    }
}
