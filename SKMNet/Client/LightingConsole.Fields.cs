using SKMNET.Client.Networking;
using SKMNET.Client.Networking.Client;
using SKMNET.Client.Rendering;
using SKMNET.Client.Stromkreise;
using SKMNET.Client.Stromkreise.ML;
using SKMNET.Client.Tasten;
using SKMNET.Client.Vorstellungen;
using SKMNET.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client
{
    public sealed partial class LightingConsole
    {
        [NonSerialized]
        public SK[] Stromkreise;

        public readonly int SKSize;

        public Enums.Bedienstelle Bedienstelle { get; }

        internal short BdstNo => (short)Bedienstelle;

        public ILogger Logger { get; set; }

        /// <summary>
        /// Alle Stromkreisgruppen
        /// </summary>
        public List<SKG> Stromkreisgruppen = new List<SKG>();

        public string Headline { get; set; }
        public string Bedienzeile { get; set; }
        public string Meldezeile { get; set; }
        public string AktReg { get; set; }
        public string AktList { get; set; }

        public Dictionary<MLPal.Flag, List<MLPal>> Paletten { get; }

        /// <summary>
        /// Alle Parameter
        /// </summary>
        public List<MLCParameter> MLCParameters { get; } = new List<MLCParameter>();

        /// <summary>
        /// Registerinfo IST
        /// </summary>
        public Register RegIST { get; set; } = new Register("IST", "", true);

        /// <summary>
        /// Registerinfo ZIEL
        /// </summary>
        public Register RegZIEL { get; set; } = new Register("ZIEL", "", false);

        /// <summary>
        /// Registerinfo VOR
        /// </summary>
        public Register RegVOR { get; set; } = new Register("VOR", "", false);

        public List<Vorstellung> Vorstellungen { get; set; } = new List<Vorstellung>();

        /// <summary>
        /// Darstellungsmodus für 100% 
        /// </summary>
        public Enums.OVDisp DisplayMode { get; set; } = Enums.OVDisp.FL;

        [NonSerialized]
        public readonly ConnectionHandler Connection;

        [NonSerialized]
        public readonly ScreenManager ScreenManager;

        [NonSerialized]
        public readonly TastenManager TastenManager;

        [NonSerialized]
        public readonly ConsoleSettings Settings;
    }
}
