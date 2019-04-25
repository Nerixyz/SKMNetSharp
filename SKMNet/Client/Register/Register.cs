using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client
{
    [Serializable]
    public class Register
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public bool AW { get; set; }

        public Register(string name, string text, bool AW)
        {
            this.Name = name;
            this.Text = text;
            this.AW = AW;
        }
    }
}
