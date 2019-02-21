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
    public partial class LightingConsole
    {
        [NonSerialized]
        public List<SK> Stromkreise = new List<SK>();
        public List<SK> ActiveSK { get; set; } = new List<SK>();

        public Enums.Bedienstelle Bedienstelle { get; }
        internal short BdstNo { get {
                return (short)Bedienstelle;
            }
        }

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

        public Dictionary<MLPal.MLPalFlag, List<MLPal>> Paletten { get; private set; }

        /// <summary>
        /// Alle Parameter (für zB GUI)
        /// </summary>
        public List<ParPrefab> Prefabs { get; set; } = new List<ParPrefab>();

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
        /// Darstellungsmodus fuer 100% 
        /// </summary>
        public Enums.OVDisp DisplayMode { get; set; } = Enums.OVDisp.FL;

        [NonSerialized]
        public readonly ConnectionHandler Connection;

        [NonSerialized]
        public readonly ScreenManager ScreenManager;

        public TastenManager TastenManager { get; }

        public LightingConsole(string ip, SKMSteckbrief steckbrief, Enums.Bedienstelle bedienstelle = Enums.Bedienstelle.Meistertastatur)
        {
            Connection = new ConnectionHandler(ip, this, ref steckbrief);
            Connection.Errored += Connection_Errored;

            this.Bedienstelle = bedienstelle;

            ScreenManager = new ScreenManager(this);

            TastenManager = new TastenManager(this);

            Paletten = new Dictionary<MLPal.MLPalFlag, List<MLPal>>()
            {
                { MLPal.MLPalFlag.I, new List<MLPal>() },
                { MLPal.MLPalFlag.F, new List<MLPal>() },
                { MLPal.MLPalFlag.C, new List<MLPal>() },
                { MLPal.MLPalFlag.B, new List<MLPal>() },
                { MLPal.MLPalFlag.SKG, new List<MLPal>() },
                { MLPal.MLPalFlag.BLK, new List<MLPal>() },
                { MLPal.MLPalFlag.DYN, new List<MLPal>() },
                { MLPal.MLPalFlag.CUR_SEL, new List<MLPal>() },
            };
        }
    }
}
