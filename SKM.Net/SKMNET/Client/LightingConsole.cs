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
        public void Query        (byte[] data, short type, Action<Enums.FehlerT> callback = null) => Connection.SendPacket(data, type, callback);

        /// <summary>
        /// Send CPacket
        /// </summary>
        /// <param name="packet">Packet to send</param>
        /// <param name="callback">Errorcode</param>
        public void Query        (CPacket packet         , Action<Enums.FehlerT> callback = null) => Connection.SendPacket(packet, callback);

        public void Query        (SplittableHeader packet, Action<Enums.FehlerT> callback = null) => Connection.SendPacket(packet, callback);

        public void SendRawData(byte[] arr)                                                       => Connection.SendRawData(arr);


        public SK GetSKByNumber(short num, bool entireSet = false)
        {
            if (entireSet)
            {
                return Stromkreise.Find((x) => x.Number == num);
            }
            else
            {
                return ActiveSK.Find((x) => x.Number == num);
            }
        }

        /// <summary>
        /// Create a Scene
        /// </summary>
        /// <param name="name">LTX</param>
        /// <param name="number">BLK Nr</param>
        /// <param name="callback">Result-Action</param>
        public void CreateScene(string name, double number, Action<Enums.FehlerT> callback = null)
        {
            EditPal(
                name,
                number,
                MLUtil.MLPalFlag.BLK,
                PalEdit.Param.Default,
                PalEdit.SkSelect.Default,
                PalEdit.SMode.Default,
                PalEdit.Cmd.Create,
                callback);
        }

        /// <summary>
        /// Palettenbearbeitungskommando
        /// </summary>
        /// <param name="name">Palettenname</param>
        /// <param name="number">Palettennummer</param>
        /// <param name="type">Palettenkennung</param>
        /// <param name="param">Parameterauswahl fuer Create Paletten und BLK</param>
        /// <param name="select">Geraeteauswahl fuer Create BLK</param>
        /// <param name="smode">Schreibmodus fuer Create BLK</param>
        /// <param name="action">Das Bearbeitungskommando</param>
        /// <param name="callback">Result-Action</param>
        public void EditPal(
            string                  name,
            double                  number,
            MLUtil.MLPalFlag        type,
            PalEdit.Param           param  = PalEdit.Param.Default,
            PalEdit.SkSelect        select = PalEdit.SkSelect.Default,
            PalEdit.SMode           smode  = PalEdit.SMode.Default,
            PalEdit.Cmd             action = PalEdit.Cmd.Create,
            Action<Enums.FehlerT>   callback = null)
        {
            Query(
                new PalEdit(
                    new PalEdit.PalEditEntry(
                        type,
                        (short)(number * 10),
                        0,
                        param,
                        select,
                        smode,
                        name),
                    action),
                callback);
        }

        /// <summary>
        /// Push changed intensities
        /// </summary>
        /// <param name="src">Can be null, either ActiveSK or AllSK</param>
        /// <param name="dst">Destination register</param>
        /// <param name="callback">Result-Action</param>
        public void PushChanges(List<SK> src = null, Enums.FixParDst dst = Enums.FixParDst.Current, Action<Enums.FehlerT> callback = null)
        {
            if (src == null)
                src = ActiveSK;

            List<SK> changed = src.FindAll((sk) => sk.dirty);
            changed.ForEach((sk) => sk.dirty = false);

            Query(new FixParNative(changed, dst), callback);
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
