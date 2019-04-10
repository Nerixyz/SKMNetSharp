using SKMNET.Client.Networking;
using SKMNET.Client.Networking.Client;
using SKMNET.Client.Rendering;
using SKMNET.Client.Stromkreise;
using SKMNET.Client.Stromkreise.ML;
using SKMNET.Client.Tasten;
using SKMNET.Client.Vorstellungen;
using SKMNET.Logging;
using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SKMNET.Client
{
    [Serializable]
    public partial class LightingConsole
    {
        public LightingConsole(string ip, ConsoleSettings settings)
        {
            this.Settings = settings;
            SKMSteckbrief steckbrief = new SKMSteckbrief()
            {
                Bedientasten = Settings.Bedientasten,
                BefMeldZeile = Settings.BefMeldZeile,
                FuncKeys = Settings.FuncKeys,
                LKI = Settings.LKI,
                BlockInfo = Settings.BlockInfo,
                AZ_Zeilen = Settings.AZ_Zeilen,
                ExtKeys = Settings.ExtKeys,
                AktInfo = Settings.AktInfo,
                Steller = Settings.Steller
            };

            Connection = new ConnectionHandler(ip, this, steckbrief, Settings.SKMType);
            Connection.Errored += Connection_Errored;

            this.Bedienstelle = Settings.Bedienstelle;
            this.Logger = Settings.Logger;

            ScreenManager = new ScreenManager(this);

            TastenManager = new TastenManager(this);

            Paletten = new Dictionary<MLPal.Flag, List<MLPal>>()
            {
                { MLPal.Flag.I, new List<MLPal>() },
                { MLPal.Flag.F, new List<MLPal>() },
                { MLPal.Flag.C, new List<MLPal>() },
                { MLPal.Flag.B, new List<MLPal>() },
                { MLPal.Flag.SKG, new List<MLPal>() },
                { MLPal.Flag.BLK, new List<MLPal>() },
                { MLPal.Flag.DYN, new List<MLPal>() },
                { MLPal.Flag.CUR_SEL, new List<MLPal>() },
            };
        }

        public sealed class ConsoleSettings
        {
            public bool Bedientasten { get; set; } = false;
            public bool BefMeldZeile { get; set; } = false;
            public bool FuncKeys     { get; set; } = false;
            public bool LKI          { get; set; } = false;
            public bool BlockInfo    { get; set; } = false;
            public bool AZ_Zeilen    { get; set; } = false;
            public bool ExtKeys      { get; set; } = false;
            public bool AktInfo      { get; set; } = false;
            public bool Steller      { get; set; } = false;

            /// <summary>
            /// Type of SKM (0=regular, 1=tsd, 2=mlc)
            /// </summary>
            public byte SKMType { get; set; } = 0;
            public Enums.Bedienstelle Bedienstelle { get; set; } = Enums.Bedienstelle.Libra;
            public ILogger Logger { get; set; } = null;

            public static ConsoleSettings All(byte SKMType = 2, Enums.Bedienstelle bedienstelle = Enums.Bedienstelle.Libra, ILogger logger = null, bool state = true)
            {
                return new ConsoleSettings()
                {
                    Bedientasten = state,
                    AktInfo      = state,
                    AZ_Zeilen    = state,
                    BefMeldZeile = state,
                    BlockInfo    = state,
                    ExtKeys      = state,
                    FuncKeys     = state,
                    LKI          = state,
                    Steller      = state,

                    Logger       = logger,
                    Bedienstelle = bedienstelle,
                    SKMType      = SKMType
                };
            }
        }
    }
}
