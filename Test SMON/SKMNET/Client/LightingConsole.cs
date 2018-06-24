using SKMNET.Client.Stromkreise;
using SKMNET.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client
{
    class LightingConsole
    {
        public List<SK> Stromkreise { get; set; }
        public string Headline { get; set; }
        public string AktReg { get; set; }
        public string AktList { get; set; }

        public ConnectionHandler Connection { get; }

        public LightingConsole(string ip)
        {

        }
    }
}
