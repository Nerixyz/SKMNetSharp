using SKMNET.Client.Stromkreise;
using SKMNET.Client.Stromkreise.ML;
using SKMNET.Networking;
using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client
{
    [Serializable]
    class LightingConsole
    {
        [NonSerialized]
        public List<SK> Stromkreise = new List<SK>();
        public string Headline { get; set; }
        public string AktReg { get; set; }
        public string AktList { get; set; }
        public List<SK> ActiveSK { get; set; } = new List<SK>();
        public List<MLPal> IPal { get; set; } = new List<MLPal>();
        public List<MLPal> FPal { get; set; } = new List<MLPal>();
        public List<MLPal> CPal { get; set; } = new List<MLPal>();
        public List<MLPal> BPal { get; set; } = new List<MLPal>();

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
