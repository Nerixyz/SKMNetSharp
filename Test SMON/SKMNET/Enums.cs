﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET
{
    class Enums
    {
        enum Type
        {
            // #define SKMON_X
            /* von RMON übernommene Telegramme */
            Sync            = 0  ,   /* Synctelegramm */
            ScreenData           ,   /* Bildschirmdaten */
            PalData              ,   /* Palettendaten (Komplett-Telegramm) */
            ReadKey              ,   /* Keyboard Eingabe abholen */
            Pipes                ,   /* Piepsen */
            BLamp                ,   /* Lampendaten fuer Bedientasten (Komplett-Telegramm) */
            ACK_Reset            ,   /* Auf das RESET wurde ein vollständiges Update gesendet */

            MScreenData     = 11 ,   /* Multiscreen Bildschirmdaten */
            MPalData             ,   /* Multiscreen Palettendaten */

            /* SKMON-spezifische Telegramme */
            SkData          = 100,   /* Stromkreiswerte (1..999) */
            SkAttr               ,   /* Stromkreis-Attribute (1..999) */
            Headline             ,   /* Kopfzeile */
            Conf                 ,   /* Konfigurationsdaten */
            Cmd                  ,   /* Kommando, s.u. */

            /* Erweiterungen fuer ISKMON */
            BTastConf       = 105,   /* Bedientasten-Konfiguration */
            FKeyConf             ,   /* Funktionstasten-Konfiguration */
            Bedienzeile          ,   /* Bedienzeile */
            Meldezeile           ,   /* Meldezeile */
            AZ_IST          = 110,   /* Aktuellzeile IST */
            AZ_ZIEL              ,   /* Aktuellzeile ZIEL */
            AZ_VOR               ,   /* Aktuellzeile VOR */
            SKG_Conf        = 115,   /* SKG-Konfiguration */ 

            /* 
             * Erweiterungen für T98 
             * 
             * Hinter den speziellen T98 Telegrammen dürfen nur noch weitere
             * T98 Telegramm folgen, da alte SKMON Versionen bei nicht implementierten
             * Teilpaketen den Rest des Pakets wegwerfen.
            */
            //T98Kenn       = 120,   /* ab dieser Kennung die T98-Erweiterungen */ DUPLICATE
            SKRegSync       = 120,   /* Abfrage auf Stromkreisregister   */
            SKRegConf            ,   /* Stromkreisregister-Aufbau */
            SKRegData            ,   /* SK-Werte in Stromkreisregister-Order */
            SKRegAttr            ,   /* SK-Attr. in Stromkreisregister-Order */

            /*
             * Erweiterungen für TSD
             *
             * Hinter den speziellen TSD Telegrammen dürfen nur noch weitere
             * TSD Telegramm folgen, da alte SKMON Versionen bei nicht implementierten
             * Teilpaketen den Rest des Pakets wegwerfen.
             */
            //TSD_Kenn      = 130,   /* ab dieser Kennung die TSD-Erweiterungen */ DUPLICATE
            TSD_Sync        = 130,   /* Abfrage auf TSD-Erweiterungen */
            TSD_DMXData          ,   /* DMX-orientierte Kreiswerte */
            TSD_MPal             ,   /* ML-Palettendaten */
            TSD_MPalSelect       ,   /* Selektierte ML-Paletteneintraege */
            MLC_Job         = 150,   /* einfache Jobkommandos an MLC */
            MLC_SelPar           ,   /* Parameter des selektierten Geraets */
            MLC_SelRange         ,   /* Range-Daten des selektierten Parameters */
            MLC_ParDef           ,   /* Parameterdefinitionen (Name usw.) */
            MLPal_Conf           ,   /* Palettenkonfiguration mit langen Namen */ 
            MLPal_SK             ,   /* SK-Beteiligung an Paletten */ 

            /* Für Libra 1.5 Parameterliste der Objektdarstellung */
            MLPar                ,   /* Neusenden Parameterwerte */
            MLRange              ,   /* Neusenden Parameter-Ranges */ 

            /* Für Libra 1.8 Erweiterungen */
            AKTInfo              ,   /* aktuelle Liste und Register */

            MLParUpdate          ,   /* Update Neusenden Parameterwerte */
            MlParRemove          ,   /* Entfernen Parameterwert oder Geraet */

            /* Für Libra 1.9 Erweiterungen */
            Steller              ,   /* Stellerwerte */
            FBHost               ,   /* FB-Uebernahme durch anderen Host */

            // Telegramme 200-299 reserviert fuer Libra-Kommunikation
        }
        enum Response
        {
            // #define SKMON_RES_X
            // Ergebniskennungen auf ein SKMON-Telegramm
            OK              , /* Alles OK */
            Reset           , /* Client wurde rückgesetzt */
            KeyPending      , /* Client hat eine Keyboard Eingabe */
            BadCmd          , /* Falsches Kommando */
            Offline         , /* Client kann nicht ausgeben */
        }

    }
}
