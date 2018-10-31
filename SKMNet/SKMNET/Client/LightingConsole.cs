using SKMNET.Client.Networking;
using SKMNET.Client.Networking.Client;
using SKMNET.Client.Rendering;
using SKMNET.Client.Stromkreise;
using SKMNET.Client.Stromkreise.ML;
using SKMNET.Client.Tasten;
using SKMNET.Client.Vorstellungen;
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
        public SK   GetSKByNumber(short num)                                                      => ActiveSK.Find((x) => x.Number == num);

        public void Query        (byte[] data, short type, Action<Enums.FehlerT> callback = null) => Connection.SendPacket(data, type, callback);

        /// <summary>
        /// Send CPacket
        /// </summary>
        /// <param name="packet">Packet to send</param>
        /// <param name="callback">Errorcode</param>
        public void Query        (CPacket packet         , Action<Enums.FehlerT> callback = null) => Connection.SendPacket(packet, callback);

        public void Query        (SplittableHeader packet, Action<Enums.FehlerT> callback = null) => Connection.SendPacket(packet, callback);

        public void SendRawData(byte[] arr)                                                       => Connection.SendRawData(arr);

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
    }
}
