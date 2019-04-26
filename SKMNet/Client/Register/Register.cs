using System;

namespace SKMNET.Client
{
    [Serializable]
    public class Register
    {
        public string Name { get; }
        public string Text { get; set; }
        public bool AW { get; set; }

        public Register(string name, string text, bool AW)
        {
            Name = name;
            Text = text;
            this.AW = AW;
        }
    }
}
