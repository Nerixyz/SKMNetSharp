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

            Connection = new ConnectionHandler(ip, this, ref steckbrief, Settings.SKMType);
            Connection.Errored += Connection_Errored;

            this.Bedienstelle = Settings.Bedienstelle;
            this.Logger = Settings.Logger;

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

            public byte SKMType { get; set; } = 0;
            public Enums.Bedienstelle Bedienstelle { get; set; } = Enums.Bedienstelle.Meistertastatur;
            public ILogger Logger { get; set; }
        }
    }
}
