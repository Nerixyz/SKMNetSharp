﻿using SKMNET.Client.Networking;
using SKMNET.Client.Networking.Client;
using SKMNET.Client.Stromkreise;
using SKMNET.Client.Stromkreise.ML;
using SKMNET.Client.Vorstellungen;
using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client
{
    [Serializable]
    public class LightingConsole
    {
        [NonSerialized]
        public List<SK> Stromkreise = new List<SK>();
        public List<SK> ActiveSK { get; set; } = new List<SK>();

        /// <summary>
        /// Alle Stromkreisgruppen
        /// </summary>
        public List<SKG> Stromkreisgruppen = new List<SKG>();
        
        public string Headline    { get; set; }
        public string Bedienzeile { get; set; }
        public string Meldezeile  { get; set; }
        public string AktReg      { get; set; }
        public string AktList     { get; set; }
        
        /// <summary>
        /// I-Pallettendaten
        /// </summary>
        public List<MLPal> IPal { get; set; } = new List<MLPal>();
        /// <summary>
        /// F-Pallettendaten
        /// </summary>
        public List<MLPal> FPal { get; set; } = new List<MLPal>();
        /// <summary>
        /// C-Pallettendaten
        /// </summary>
        public List<MLPal> CPal { get; set; } = new List<MLPal>();
        /// <summary>
        /// B-Pallettendaten
        /// </summary>
        public List<MLPal> BPal { get; set; } = new List<MLPal>();
        /// <summary>
        /// Stimmungen
        /// </summary>
        public List<MLPal> BLK { get; set; } = new List<MLPal>();

        /// <summary>
        /// Alle Parameter (für zB GUI)
        /// </summary>
        public List<ParPrefab> Prefabs { get; set; } = new List<ParPrefab>();

        /// <summary>
        /// Registerinfo IST
        /// </summary>
        public Register RegIST  { get; set; } = new Register("IST", "", true);
        /// <summary>
        /// Registerinfo ZIEL
        /// </summary>
        public Register RegZIEL { get; set; } = new Register("ZIEL", "", false);
        /// <summary>
        /// Registerinfo VOR
        /// </summary>
        public Register RegVOR  { get; set; } = new Register("VOR", "", false);

        public List<Vorstellung> Vorstellungen { get; set; } = new List<Vorstellung>();

        /// <summary>
        /// Darstellungsmodus fuer 100% 
        /// </summary>
        public Enums.OVDisp DisplayMode { get; set; } = Enums.OVDisp.FL;

        [NonSerialized]
        public readonly ConnectionHandler Connection;

        public LightingConsole(string ip)
        {
            Connection = new ConnectionHandler(ip, this);
            Connection.Errored += Connection_Errored;
        }

        private void Connection_Errored(object sender, Exception e)
        {
            OnErrored(e);
        }

        public void Query(CPacket packet)
        {
            Connection.SendPacket(packet);
        }

        public void Query(SplittableHeader packet)
        {
            Connection.SendPacket(packet);
        }

        public void Query(CPacket packet, Action<byte[]> callback)
        {
            Console.WriteLine(ByteUtils.ArrayToString(packet.GetDataToSend()));
            Connection.SendPacket(packet, callback);
        }

        public void Query(SplittableHeader packet, Action<byte[]> callback)
        {
            Connection.SendPacket(packet, callback);
        }

        public event EventHandler<Exception> Errored;
        protected virtual void OnErrored(Exception data) { Errored?.Invoke(this, data); }
    }
}
