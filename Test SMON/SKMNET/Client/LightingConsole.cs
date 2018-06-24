using SKMNET.Client.Stromkreise;
using SKMNET.Networking;
using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client
{
    class LightingConsole
    {
        public List<SK> Stromkreise { get; set; } = new List<SK>();
        public string Headline { get; set; }
        public string AktReg { get; set; }
        public string AktList { get; set; }
        public List<SK> ActiveSK { get; set; } = new List<SK>();

        public ConnectionHandler Connection { get; }

        public LightingConsole(string ip)
        {
            Connection = new ConnectionHandler(ip, this);
        }

        public void Query(ISendable packet)
        {
            Connection.SendData(packet);
        }
    }
}
