using SKMNET.Client.Networking;
using SKMNET.Client.Networking.Client;
using SKMNET.Client.Rendering;
using SKMNET.Client.Stromkreise;
using SKMNET.Client.Stromkreise.ML;
using SKMNET.Client.Tasten;
using System;
using System.Collections.Generic;
using SKMNET.Client.Networking.Client.SKMON;
using SKMNET.Util;

namespace SKMNET.Client
{
    [Serializable]
    public sealed partial class LightingConsole
    {
        public LightingConsole(string ip, ConsoleSettings settings)
        {
            Settings = settings;
            SKMSteckbrief steckbrief = new SKMSteckbrief
            {
                Bedientasten = Settings.Bedientasten,
                BefMeldZeile = Settings.BefMeldZeile,
                FuncKeys = Settings.FuncKeys,
                Lki = Settings.Lki,
                BlockInfo = Settings.BlockInfo,
                AzZeilen = Settings.AzZeilen,
                ExtKeys = Settings.ExtKeys,
                AktInfo = Settings.AktInfo,
                Steller = Settings.Steller
            };

            Stromkreise = new SK[Settings.SkSize];
            SKSize = Settings.SkSize;

            Connection = new ConnectionHandler(ip, this, steckbrief, Settings.SkmType);
            Connection.Errored += Connection_Errored;

            Bedienstelle = Settings.Bedienstelle;
            Logger = Settings.Logger;

            ScreenManager = new ScreenManager(this);

            TastenManager = new TastenManager();

            Paletten = new Dictionary<MlPal.Flag, List<MlPal>>
            {
                { MlPal.Flag.I, new List<MlPal>() },
                { MlPal.Flag.F, new List<MlPal>() },
                { MlPal.Flag.C, new List<MlPal>() },
                { MlPal.Flag.B, new List<MlPal>() },
                { MlPal.Flag.SKG, new List<MlPal>() },
                { MlPal.Flag.BLK, new List<MlPal>() },
                { MlPal.Flag.DYN, new List<MlPal>() },
                { MlPal.Flag.CUR_SEL, new List<MlPal>() }
            };
        }

        public sealed class ConsoleSettings
        {
            public bool Bedientasten { get; private set; }
            public bool BefMeldZeile { get; private set; }
            public bool FuncKeys { get; private set; }
            public bool Lki { get; private set; }
            public bool BlockInfo { get; private set; }
            public bool AzZeilen { get; private set; }
            public bool ExtKeys { get; private set; }
            public bool AktInfo { get; private set; }
            public bool Steller { get; private set; }

            /// <summary>
            /// Type of SKM (0=regular, 1=tsd, 2=mlc)
            /// </summary>
            public byte SkmType { get; private set; }
            public Enums.Bedienstelle Bedienstelle { get; private set; } = Enums.Bedienstelle.Libra;

            /// <summary>
            /// Logger for this Connection
            /// </summary>
            public ILogger Logger { get; private set; }

            /// <summary>
            /// Size of <see cref="LightingConsole.Stromkreise"/>
            /// </summary>
            public int SkSize { get; private set; } = 512 * 2;

            /// <summary>
            /// Initialize default settings
            /// </summary>
            /// <param name="skSize">Size of <see cref="LightingConsole.Stromkreise"/></param>
            /// <param name="skmType">Type of SKM (0 = regular, 1 = TSD, 2 = MLC)</param>
            /// <param name="bedienstelle">Connection type</param>
            /// <param name="logger">Logger for the connection</param>
            /// <param name="state">State of all flags</param>
            /// <returns>Default <see cref="ConsoleSettings"/></returns>
            public static ConsoleSettings All(int skSize = 512 * 2, byte skmType = 2, Enums.Bedienstelle bedienstelle = Enums.Bedienstelle.Libra, ILogger logger = null, bool state = true)
            {
                return new ConsoleSettings
                {
                    Bedientasten = state,
                    AktInfo      = state,
                    AzZeilen     = state,
                    BefMeldZeile = state,
                    BlockInfo    = state,
                    ExtKeys      = state,
                    FuncKeys     = state,
                    Lki          = state,
                    Steller      = state,

                    Logger       = logger,
                    Bedienstelle = bedienstelle,
                    SkmType      = skmType,
                    SkSize       = skSize
                };
            }
        }
    }
}
