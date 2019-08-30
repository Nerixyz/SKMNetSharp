using System;

namespace SKMNET.Client.Tasten
{
    [Serializable]
    public class Taste
    {
        public ushort Data { get; }
        public ushort TastNr => (ushort)( Data & 0x3fff);
        public bool Zweiflankentaste => (Data & 0x8000) != 0;
        public bool Lamp => (Data & 0x4000) != 0;

        public string Name { get; }

        public LampState State { get; set; }

        public Taste(ushort data, string name)
        {
            Data = data;
            Name = name;
            State = LampState.Aus;
        }

        [Serializable]
        public enum LampState
        {
            Aus,
            An,
            Blinken,
            ABlinken
        }
    }
}
