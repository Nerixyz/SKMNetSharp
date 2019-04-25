using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// ReSharper disable All

namespace SKMNET
{
    public static class Enums
    {
        /// <summary>
        /// SPacket-Type / Telegrammart
        /// </summary>
        public enum Type
        {
            /// <summary>
            /// Synctelegramm
            /// </summary>
            Sync = 0,
            /// <summary>
            /// Bildschirmdaten
            /// </summary>
            ScreenData,
            /// <summary>
            /// Palettendaten (Komplett-Telegramm)
            /// </summary>
            PalData,
            /// <summary>
            /// Keyboard Eingabe abholen
            /// </summary>
            ReadKey,
            /// <summary>
            /// Piepsen
            /// </summary>
            Pieps,
            /// <summary>
            /// Lampendaten fuer Bedientasten (Komplett-Telegramm)
            /// </summary>
            BLamp,
            /// <summary>
            /// Auf das RESET wurde ein vollständiges Update gesendet 
            /// </summary>
            ACK_Reset,

            /// <summary>
            /// Multiscreen Bildschirmdaten
            /// </summary>
            MScreenData = 11,
            /// <summary>
            /// Multiscreen Palettendaten
            /// </summary>
            MPalData,

            /// <summary>
            /// Stromkreiswerte (1..999)
            /// </summary>
            SkData = 100,
            /// <summary>
            /// Stromkreis-Attribute (1..999)
            /// </summary>
            SkAttr,
            /// <summary>
            /// Kopfzeile
            /// </summary>
            Headline,
            /// <summary>
            /// Komfigurationsdaten
            /// </summary>
            Conf,
            /// <summary>
            /// Kommando, s.u.
            /// </summary>
            Cmd,

            /// <summary>
            /// Bedientasen-Konfiguration
            /// </summary>
            BTastConf = 105,
            /// <summary>
            /// Funktionstasten-Konfiguration
            /// </summary>
            FKeyConf,
            /// <summary>
            /// Bedienzeile
            /// </summary>
            Bedienzeile,
            /// <summary>
            /// Meldezeile
            /// </summary>
            Meldezeile,
            /// <summary>
            /// Aktuellzeile IST
            /// </summary>
            AZ_IST = 110,
            /// <summary>
            /// Aktuellzeile ZIEL
            /// </summary>
            AZ_ZIEL,
            /// <summary>
            /// Aktuellzeile VOR
            /// </summary>
            AZ_VOR,
            /// <summary>
            /// SKG-Konfiguration
            /// </summary>
            SKG_Conf = 115,

            /* 
             * Erweiterungen für T98 
             * 
             * Hinter den speziellen T98 Telegrammen dürfen nur noch weitere
             * T98 Telegramm folgen, da alte SKMON Versionen bei nicht implementierten
             * Teilpaketen den Rest des Pakets wegwerfen.
            */
            //T98Kenn       = 120,   /* ab dieser Kennung die T98-Erweiterungen */ DUPLICATE
            /// <summary>
            /// Abfrage auf Stromkreisregister
            /// (gibt <see cref="Response.OK"/> zurück)
            /// </summary>
            SKRegSync = 120,
            /// <summary>
            /// Stromkreisregister-Aufbau
            /// </summary>
            SKRegConf,
            /// <summary>
            /// SK-Werte in Stromkreisregister-Order
            /// </summary>
            SKRegData,
            /// <summary>
            /// SK-Attr. in Stromkreisregister-Order
            /// </summary>
            SKRegAttr,

            /*
             * Erweiterungen für TSD
             *
             * Hinter den speziellen TSD Telegrammen dürfen nur noch weitere
             * TSD Telegramm folgen, da alte SKMON Versionen bei nicht implementierten
             * Teilpaketen den Rest des Pakets wegwerfen.
             */
            //TSD_Kenn      = 130,   /* ab dieser Kennung die TSD-Erweiterungen */ DUPLICATE
            
            /// <summary>
            /// Abfrage auf TSD-Erweiterungen
            /// (gibt <see cref="Response.OK"/> zurück)
            /// </summary>
            TSD_Sync = 130,
            /// <summary>
            /// DMX-orientierte Kreiswerte
            /// </summary>
            TSD_DMXData,
            /// <summary>
            /// ML-Palettendaten
            /// </summary>
            TSD_MPal,
            /// <summary>
            /// Selektierte ML-Palleteneinträge
            /// </summary>
            TSD_MPalSelect,
            /// <summary>
            /// einfache Jobkommandos an MLC
            /// </summary>
            MLC_Job = 150,
            /// <summary>
            /// Parameter des selektierten Geraets
            /// </summary>
            MLC_SelPar,
            /// <summary>
            /// Range-Daten des selektierten Parameters
            /// </summary>
            MLC_SelRange,
            /// <summary>
            /// Parameterdefinitionen (Name usw.)
            /// </summary>
            MLC_ParDef,
            /// <summary>
            /// Palettenkonfiguration mit langen Namen
            /// </summary>
            MLPal_Conf,
            /// <summary>
            /// SK-Beteiligung an Paletten
            /// </summary>
            MLPal_SK,

            /* Für Libra 1.5 Parameterliste der Objektdarstellung */
            /// <summary>
            /// Neusenden Parameterwerte
            /// </summary>
            MLPar,
            /// <summary>
            /// Neusenden Parameter-Ranges
            /// </summary>
            MLRange,

            /* Für Libra 1.8 Erweiterungen */
            /// <summary>
            /// aktuelle Liste und Register
            /// </summary>
            AKTInfo,

            /// <summary>
            /// Update Neusenden Parameterwerte
            /// </summary>
            MLParUpdate,
            /// <summary>
            /// Entfernen Parameterwert oder Geraet
            /// </summary>
            MlParRemove,

            /* Für Libra 1.9 Erweiterungen */
            /// <summary>
            /// Stellerwerte
            /// </summary>
            Steller,
            /// <summary>
            /// FB-Uebernahme durch andern Host
            /// </summary>
            FBHost,

            // Telegramme 200-299 reserviert fuer Libra-Kommunikation (nur NTX bzw. ab v6.0)
        }
        /// <summary>
        /// Ergebniskennungen auf ein SKMON-Telegramm
        /// </summary>
        public enum Response
        {
            /// <summary>
            /// Alles OK
            /// </summary>
            OK,
            /// <summary>
            /// Client wurde rückgesetzt
            /// </summary>
            Reset,
            /// <summary>
            /// Client hat eine Keybord Eingabe
            /// </summary>
            KeyPending,
            /// <summary>
            /// Falsches Kommando
            /// </summary>
            BadCmd,
            /// <summary>
            /// Client kann nicht ausgeben
            /// </summary>
            Offline,
        }
        /// <summary>
        /// Systemfarben werden 1:1 aus der Anlagenpalette (pal.h) uebernommen
        /// </summary>
        public enum Pal
        {
            /// <summary>
            /// Befehls- und Meldezeile
            /// </summary>
            Bef = 8,
            /// <summary>
            /// Aktuellzeile
            /// </summary>
            RegHead = 13,
            /// <summary>
            /// Aktuellzeile angewaehlt
            /// </summary>
            RegHeadAW,

            /* Paletteneintraege fuer die SK- und Wertezeilen. */
            /// <summary>
            /// Stromkreisnummer, nicht angewaehlt
            /// </summary>
            SKNorm = 24,
            /// <summary>
            /// Stomkreisnummer, angewaehlt
            /// </summary>
            SKAnw,
            /// <summary>
            /// Stromkreiswert normal
            /// </summary>
            ValNorm,
            /// <summary>
            /// Stromkreiswert blind
            /// </summary>
            ValBld,
            /// <summary>
            /// Stromkreiswert bei SKU-Fehler
            /// </summary>
            ValSkuErr,
            
            /* Erweiterungen fuer ISKMON */
            /// <summary>
            /// Bedientaste normal (Lampe aus)
            /// </summary>
            ButtonNorm,
            /// <summary>
            /// Bedientaste mit Lampe ein
            /// </summary>
            ButtonLED,
            /// <summary>
            /// Funktionstaste
            /// </summary>
            FKey,
            /// <summary>
            /// Stromkreiswert in der Maske
            /// </summary>
            ValMas,
            /// <summary>
            /// Stromkreisgruppenfeld
            /// </summary>
            SKG,
        }
        public enum SkAttribute
        {
            /// <summary>
            /// Sk-Angewählt
            /// </summary>
            AW = 0x01,
            /// <summary>
            /// SKU-Fehler
            /// </summary>
            SKU = 0x02,
            /// <summary>
            /// SK in Maske
            /// </summary>
            MAS = 0x04,
            /// <summary>
            /// SK an Register beteiligt
            /// </summary>
            BET = 0x08,
            /// <summary>
            /// Dimmerwert geändert
            /// </summary>
            MOD = 0x10,
            /// <summary>
            /// SK Gesperrt
            /// </summary>
            SPERR = 0x20,
            /// <summary>
            /// SK wird heller
            /// </summary>
            HE = 0x40,
            /// <summary>
            /// SK wird dunkler
            /// </summary>
            DU = 0x80
        }

        /// <summary>
        /// Anwahltyp
        /// </summary>
        public enum AWType
        {
            /// <summary>
            /// Absolut
            /// </summary>
            Abs = 1,
            /// <summary>
            /// Hinzufügend
            /// </summary>
            Add,
            /// <summary>
            /// Subtrahierend
            /// </summary>
            Sub
        }

        /// <summary>
        /// Darstellungsmodus fuer 100%
        /// </summary>
        public enum OVDisp
        {
            /// <summary>
            /// als '100'
            /// </summary>
            Normal,
            /// <summary>
            /// als 'FF'
            /// </summary>
            FF,
            /// <summary>
            /// als 'FL'
            /// </summary>
            FL
        }

        /// <summary>
        /// Die möglichen Bedienstellen der SKM
        /// </summary>
        public enum Bedienstelle
        {
            None = -1,
            Meistertastatur = 0,
            Libra = 1,
            Handtermianl1 = 2,
            Handterminal2 = 3,
            Handterminal3 = 4,
            Handterminal4 = 5,
            RemoteMonitor = 6
        }

        /// <summary>
        /// Kommandos an den SKMON-PC
        /// </summary>
        public enum SKCmd
        {
            /// <summary>
            /// page up (niedrigere Seite), kein wrap
            /// </summary>
            PGUp = 1,
            /// <summary>
            /// page down (hoehrere Seite), kein wrap
            /// </summary>
            PGDown,
            /// <summary>
            /// home (erste Seite)
            /// </summary>
            Home,
            /// <summary>
            /// end (letzte Seite)
            /// </summary>
            End
        }
        public enum SelRangeFlags
        {
            /// <summary>
            /// nur Setwert, kein linearer Bereich
            /// </summary>
            SetPoint = 0x01,
            /// <summary>
            /// Bereich wird bei Encoder uebersprungen
            /// </summary>
            EncoderSkip = 0x02,
            /// <summary>
            /// absoluten DMX-Wert darstellen
            /// </summary>
            DMXVal = 0x04,
            /// <summary>
            /// relativen Prozentwert darstellen
            /// </summary>
            RelVal = 0x08,
        }
        public enum SelRangeDisp
        {
            /// <summary>
            /// Standard: 0..100 (%)
            /// </summary>
            Normal,
            /// <summary>
            /// DMX-Wert: 000..255
            /// </summary>
            DMX,
            /// <summary>
            /// normierte Darstellung -99.9 .. +99.9
            /// </summary>
            Pos,
            /// <summary>
            /// 0..65565
            /// </summary>
            L16,
        }
        /// <summary>
        /// Zielregister
        /// </summary>
        public enum FixParDst
        {
            Current = 0,
            SKMON = 1001,
            Voyager,
            ShowDesigner,
        }

        public enum FehlerT
        {
            FT_OK = 0x00,
            LK_NICHT_AUSGEBAUT = 0x01,
            LK_NICHT_GEKOPPELT = 0x02,
            LK_BEREITS_GEKOPPELT = 0x03,
            KEIN_SK_GEFUNDEN = 0x04,
            BK_NICHT_AUSGEBAUT = 0x05,
            NUL_UND_VOL = 0x06,
            NUR_BEI_UE = 0x07,
            NUR_BEI_GR = 0x08,
            NUR_BEI_HAND = 0x09,
            NUR_BEI_BLD = 0x0a,
            NUR_BEI_BLD_ODER_VOR = 0x0b,
            NICHT_BEI_UE = 0x0c,
            NICHT_BEI_GR = 0x0d,
            NICHT_BEI_HAND = 0x0e,
            NICHT_BEI_BLD = 0x0f,
            SK_NICHT_IM_SATZ = 0x10,
            SEQUENZ_AM_ENDE = 0x11,
            SEQUENZ_AM_ANFANG = 0x12,
            UEBENDE = 0x13,
            KEIN_BLOCK_GEFUNDEN = 0x14,
            KEIN_BLOCK_KOPIERT = 0x15,
            FALSCHE_KENNLINIE = 0x16,
            NONVARKL = 0x17,
            MASKE_ZU = 0x18,
            ALT_LAEUFT = 0x19,
            UEB_LAEUFT = 0x1a,
            FALSCHE_ZAHLENEINGABE = 0x1b,
            FALSCHE_EINGABE = 0x1c,
            TPVOLL = 0x1d,
            BIGZ = 0x1e,
            INT0100 = 0x1f,
            INT0300 = 0x20,
            N01 = 0x21,
            NUR_GR_IST = 0x22,
            FALSCHE_BLOCKNUMMER = 0x23,
            FALSCHE_KENNLINIENNUMMER = 0x24,
            N5 = 0x25,
            BK0 = 0x26,
            FALSCHE_ZEITEINGABE = 0x27,
            BIG_ZEIT = 0x28,
            NUR_MIT_FRG = 0x29,
            SYNTAXFEHLER = 0x2a,
            MAN_LAEUFT = 0x2b,
            ABR_LAEUFT = 0x2c,
            KOR_EINS = 0x2d,
            SPERR_EINGABE = 0x2e,
            NUR_BEI_KL = 0x2f,
            NUR_FUER_INT = 0x30,
            NUR_MIT_T = 0x31,
            NUR_FUER_ZEIT = 0x32,
            BAD_MENU = 0x33,
            KEIN_BK_IM_BEREICH = 0x34,
            BAD_LKNO = 0x35,
            CSC_LAEUFT = 0x36,
            CSC_SPOILED = 0x37,
            MONKOP = 0x38,
            START_LAEUFT = 0x39,
            NICHT_FUER_ZM = 0x3a,
            BK_NICHT_GEKOPPELT = 0x3b,
            NO_ZM = 0x3c,
            NICHT_BEI_VOR = 0x3d,
            NO_PUT = 0x3e,
            NO_GET = 0x3f,
            NO_UNDO = 0x40,
            NO_INS = 0x41,
            NO_DEL = 0x42,
            NO_X = 0x43,
            NO_SKBER = 0x44,
            NEED_SKBER = 0x45,
            NEED_SKWAHL = 0x46,
            TIEFENTLADUNG = 0x47,
            SYSTEMFEHLER = 0x48,
            SYSERR = 0x49,
            SKLIST_VOLL = 0x4a,
            NEUSTART = 0x4b,
            LKLIST_VOLL = 0x4c,
            BAD_REQUEST = 0x4d,
            BAD_POS = 0x4e,
            POS_BELEGT = 0x4f,
            LIST_OVERFLOW = 0x50,
            BDST_BAD = 0x51,
            MB_TIMEOUT = 0x52,
            MB_VOLL = 0x53,
            MB_LEER = 0x54,
            LISTE_VOLL = 0x55,
            LAMPENTEST = 0x56,
            NONIMP = 0x57,
            DRUCKER_AUS = 0x58,
            DRUCKER_NETZ = 0x59,
            DRUCKER_HW = 0x5a,
            DRUCKER_BUSY = 0x5b,
            DRUCKAKT = 0x5c,
            K40VORST = 0x5d,
            K40PROBE = 0x5e,
            T40VORST = 0x5f,
            T20VORST = 0x60,
            FLSONST = 0x61,
            T40UT = 0x62,
            ERS_VOLL = 0x63,
            ERSAKTIV = 0x64,
            ERSATZ_NICHT_VORBEREITET = 0x65,
            KEIN_TRICK = 0x66,
            TE_OPT = 0x67,
            NUR_BEI_TE = 0x68,
            TEKEY_FALSCH = 0x69,
            NICHT_BEI_TE = 0x6a,
            ORGELTAKT_VERBOTEN = 0x6b,
            INTA_NICHT_BEI_TE = 0x6c,
            CHANLIST_VOLL = 0x6d,
            NUR_BEI_CHAN = 0x6e,
            NICHT_BEI_STARTCHAN = 0x6f,
            STP_AN = 0x70,
            STP_AUS = 0x71,
            LKTAFEL_AUS = 0x72,
            HT_EIN = 0x73,
            IR_EIN = 0x74,
            NA_AUS = 0x75,
            KOP_KOLL = 0x76,
            KOP_WORK = 0x77,
            KOP_TIMEOUT = 0x78,
            NA_MASTER = 0x79,
            SNA_LAEUFT = 0x7a,
            FB_AUS = 0x7b,
            WARN_SPEILEER = 0x7c,
            SPEICHER_LEER = 0x7d,
            MEMSHORT = 0x7e,
            SPEI_BUSY = 0x7f,
            WARN_SS = 0x80,
            LKTAB_SYNTAX = 0x81,
            LKTAB_NOTTHERE = 0x82,
            RANGTAB_SYNTAX = 0x83,
            RANGTAB_NOTTHERE = 0x84,
            HLP_SYNTAX = 0x85,
            BKTAB_SYNTAX = 0x86,
            BLOCK_OVERLAP = 0x87,
            KEINE_PROBENFOLGE = 0x88,
            TTT_SYNTAX = 0x89,
            FL1_TO_FL2 = 0x8a,
            BLOCKNO_TOO_LARGE = 0x8b,
            KEIN_BLOCK_IN_SEQUENZ = 0x8c,
            BLOCK_BELEGT = 0x8d,
            WARN_BLOCKART = 0x8e,
            WARN_BELEGT = 0x8f,
            FALSCHE_BLOCKART = 0x90,
            BLOCK_NICHT_GESCHRIEBEN = 0x91,
            KEIN_BLOCK_IN_VORWAHL = 0x92,
            VST_NOT_FOUND = 0x93,
            VST_EXISTS = 0x94,
            KEINE_VST = 0x95,
            WARN_KEIN_BLOCK_IN_VORWAHL = 0x96,
            KEIN_BLOCK_IM_BEREICH = 0x97,
            KEIN_BLOCK_GESCHRIEBEN = 0x98,
            DATEI_BAD = 0x99,
            KEINE_DISKETTE = 0x9a,
            FLOPPY_UNFORMATTIERT = 0x9b,
            DISKWECHSEL = 0x9c,
            SCHREIBSCHUTZ = 0x9d,
            FLOPPY_HARDWARE = 0x9e,
            FLOPPY_LEER = 0x9f,
            LESEFEHLER = 0xa0,
            SCHREIBFEHLER = 0xa1,
            FLOPPYDIR = 0xa2,
            FLOPPY_NICHT_GELOESCHT = 0xa3,
            FLOPPY_NICHT_AUSGEBAUT = 0xa4,
            FILE_NOT_FOUND = 0xa5,
            NO_FILE_FOUND = 0xa6,
            STDIO_EACCES = 0xa7,
            STDIO_EROFS = 0xa8,
            STDIO_EDQUOT = 0xa9,
            STDIO_ENOENT = 0xaa,
            STDIO_ENODEV = 0xab,
            STDIO_ENOTBLK = 0xac,
            VOR_NOT_READY = 0xad,
            SUM_LEER = 0xae,
            SRC_IS_DEST = 0xaf,
            NICHT_BEI_ADD = 0xb0,
            NETDOWN = 0xb1,
            NOMACRO = 0xb2,
            MACRO_FULL = 0xb3,
            NICHT_BEI_ZIEL = 0xb4,
            UEB_ANFANG = 0xb5,
            NUR_BEI_MAN = 0xb6,
            NO_CUT = 0xb7,
            NICHT_BEI_FIX = 0xb8,
            NUR_BEI_SQL = 0xb9,
            SQL_SYNC = 0xba,
            SEQNO_EXISTS = 0xbb,
            SEQNO_NOT_FOUND = 0xbc,
            NICHT_FUER_AKTSEQ = 0xbd,
            MACRO_RECORDING = 0xbe,
            NICHT_BEI_SQL = 0xbf,
            DINGDONG = 0xc0,
            NUR_FUER_AKTSEQ = 0xc1,
            KEIN_NETZWERK = 0xc2,
            NICHT_BEI_GRBO = 0xc3,
            WARN_MAX_50_PROZ = 0xc4,
            NO_LINK = 0xc5,
            KEIN_SNA_BEI_MIDI = 0xc6,
            NO_RET = 0xc7,
            FT_BAD = 0xc8,
            DO_VST_SEND = 0xc9,
            NA_NEUSTART = 0xca,
            MT_ONLY = 0xcb,
            KNA_END = 0xcc,
            VST_SEND_DONE = 0xcd,
            NO_HD_OPT = 0xce,
            NO_ABR_OPT = 0xcf,
            NO_PRO_OPT = 0xd0,
            NO_TSK_OPT = 0xd1,
            MAN_NOT_FINISHED = 0xd2,
            NO_UEB2_OPT = 0xd3,
            NEED_FRG_UEB_LOE = 0xd4,
            RTC_BAD = 0xd5,
            NUR_BEI_ADD = 0xd6,
            BAD_OPT = 0xd7,
            DIMMER_BAD = 0xd8,
            DMX90_BAD = 0xd9,
            DMX90_ERR = 0xda,
            DMX90_ILL_STATION = 0xdb,
            DMX90_NO_INPUT = 0xdc,
            FB_BEGIN = 0xdd,
            FB_END = 0xde,
            FB_FAIL = 0xdf,
            FB_RESET = 0xe0,
            NO_FB_OPT = 0xe1,
            NICHT_BEI_FB = 0xe2,
            FB_CLIENT_MODE = 0xe3,
            FW_BAD = 0xe4,
            COLOR_BAD = 0xe5,
            FW_RANGE = 0xe6,
            FW_LEER = 0xe7,
            POSNUM_BAD = 0xe8,
            POS_RANGE = 0xe9,
            POS_LEER = 0xea,
            POSPAR_BAD = 0xeb,
            BTAFEL_NOT_READY = 0xec,
            DEMO_NO_SAVE = 0xed,
            KNA_ABORT = 0xee,
            DMX90_OLDVERS = 0xef,
            DMX90_UPHASE = 0xf0,
            BAD_RANGE = 0xf1,
            NO_MTC_OPT = 0xf2,
            DMX90_DISABLE = 0xf3,
            NOT_FOR_PC90 = 0xf4,
            NICHT_FUER_POS = 0xf5,
            NEED_POSWAHL = 0xf6,
            DMX90_OVERTEMP = 0xf7,
            NEED_PROF = 0xf8,
            NICHT_BEI_PROF = 0xf9,
            NO_MK = 0xfa,
            NICHT_BEI_TRACK = 0xfb,
            SAVE_VST_DONE = 0xfc,
            BTAFEL_READY = 0xfd,
            SK_GESPERRT = 0xfe,
            OLDFWCONF = 0xff,
            OLDPOSCONF = 0x100,
            HOGCONF = 0x101,
            NO_DIMMER = 0x102,
            NICHT_BEI_PROG = 0x103,
            NO_PALETTE = 0x104,
            NO_POSFWOPT = 0x105,
            NO_DMXIN = 0x106,
            FTEXT_DUM1 = 0x107,
            FTEXT_DUM2 = 0x108,
            FTEXT_DUM3 = 0x109,
            FTEXT_DUM4 = 0x10a,
            FTEXT_DUM5 = 0x10b,
            FTEXT_DUM6 = 0x10c,
            FTEXT_DUM7 = 0x10d,
            FTEXT_DUM8 = 0x10e,
            FTEXT_DUM9 = 0x10f,
            FTEXT_DUM0 = 0x110,
            FTEXT_DUM11 = 0x111,
            FTEXT_DUM12 = 0x112,
            FTEXT_DUM13 = 0x113,
            FTEXT_DUM14 = 0x114,
            FTEXT_DUM15 = 0x115,
            FTEXT_DUM16 = 0x116,
            FTEXT_DUM17 = 0x117,
            FTEXT_DUM18 = 0x118,
            FTEXT_DUM19 = 0x119,
            FTEXT_DUM20 = 0x11a,
            LASTFEHLNO = 0x11b,
        }

        public enum Steller
        {
            STELL_NONE = -1,            /* keine gueltige Stellernummer */

            DIGI_SKSTELL = 0,       /* digitaler SK-Steller */

            /* Gruppensteller in aufsteigender Reihenfolge */
            ANA_GRSTELL_1,
            ANA_GRSTELL_2,
            ANA_GRSTELL_3,
            ANA_GRSTELL_4,
            ANA_GRSTELL_5,
            ANA_GRSTELL_6,
            ANA_GRSTELL_7,
            ANA_GRSTELL_8,
            ANA_GRSTELL_9,
            ANA_GRSTELL_10,
            ANA_GRSTELL_11,
            ANA_GRSTELL_12,
            ANA_GRSTELL_13,
            ANA_GRSTELL_14,
            ANA_GRSTELL_15,
            ANA_GRSTELL_16,
            ANA_GRSTELL_17,
            ANA_GRSTELL_18,
            ANA_GRSTELL_19,
            ANA_GRSTELL_20,

            /* Hauptsteller */
            /// <summary>
            /// 1. Hauptsteller (li)
            /// </summary>
            ANA_HSTELL_1,
            /// <summary>
            /// 2. Hauptsteller (re)
            /// </summary>
            ANA_HSTELL_2,

            /// <summary>
            /// 1. Hauptsteller (li)
            /// </summary>
            DIGI_HSTELL_1,
            /// <summary>
            /// 2. Hauptsteller (re)
            /// </summary>
            DIGI_HSTELL_2,

            /// <summary>
            /// 1. Überblender (li)
            /// </summary>
            ANA_UESTELL_A1,
            /// <summary>
            /// 1. Überblender (re)
            /// </summary>
            ANA_UESTELL_A2,

            /// <summary>
            /// 2. Überblender (li)
            /// </summary>
            ANA_UESTELL_B1,
            /// <summary>
            /// 2. Überblender (re)
            /// </summary>
            ANA_UESTELL_B2,

            /// <summary>
            /// 1. Überblender (li)
            /// </summary>
            DIGI_UESTELL_A1,
            /// <summary>
            /// 1. Überblender (re)
            /// </summary>
            DIGI_UESTELL_A2,

            /// <summary>
            /// 2. Überblender (li)
            /// </summary>
            DIGI_UESTELL_B1,
            /// <summary>
            /// 2. Überblender (re)
            /// </summary>
            DIGI_UESTELL_B2,

            /* externe Analogsignale */
            ANASIG_1,
            ANASIG_2,
            ANASIG_3,
            ANASIG_4,
            ANASIG_5,
            ANASIG_6,
            ANASIG_7,
            ANASIG_8,

            /* fuer T90-E zusaetzlich eingefuehrte interne Stellernummern */
            /// <summary>
            /// Trick-Hauptsteller
            /// </summary>
            ANA_ESTELL,
            /// <summary>
            /// Gruppenhauptsteller
            /// </summary>
            ANA_GRGENSTELL,

            /// <summary>
            /// Trackball X
            /// </summary>
            TRACK_X_STELL,
            /// <summary>
            /// Trackball Y
            /// </summary>
            TRACK_Y_STELL,

            /// <summary>
            /// Kennung Bedientafel-Reset
            /// </summary>
            BTAFEL_RESET,

            /* Digitale Encoder */
            DIGI_ENCODER_1,
            DIGI_ENCODER_2,
            DIGI_ENCODER_3,
            DIGI_ENCODER_4,

            /// <summary>
            /// Überblend-Hauptsteller (li)
            /// </summary>
            ANA_UEGENSTELL_A,
            /// <summary>
            /// Überblendhauptsteller (re)
            /// </summary>
            ANA_UEGENSTELL_B,

            /// <summary>
            /// Digitalsteller von externen Clients
            /// </summary>
            DIGI_DIGI_STELL
        }

        public static T GetEnum<T>(object value)
        {
            return (T)Enum.ToObject(typeof(T), value);
        }
    }
}
