using System;

namespace SKMNET.Client
{
    [Serializable]
    public class Register
    {
        public string Name { get; }
        public string Text { get; set; }
        public bool Aw { get; set; }

        public Register(string name, string text, bool aw)
        {
            Name = name;
            Text = text;
            this.Aw = aw;
        }
    }
}
