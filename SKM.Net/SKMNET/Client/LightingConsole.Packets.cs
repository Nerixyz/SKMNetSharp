using SKMNET.Client.Networking.Client;
using SKMNET.Client.Stromkreise;
using SKMNET.Client.Tasten;
using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.Text;
using static SKMNET.Enums;

namespace SKMNET.Client
{
    public partial class LightingConsole
    {

        #region PalEdit
        /// <summary>
        /// Create a Scene
        /// </summary>
        /// <param name="name">LTX</param>
        /// <param name="number">BLK Nr.</param>
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
        /// Edit an existing scene
        /// </summary>
        /// <param name="name">LTX</param>
        /// <param name="number">BLK Nr.</param>
        /// <param name="param">Parameterauswahl</param>
        /// <param name="skSelect">Geräteauswahl</param>
        /// <param name="saveMode">Schreibmodus</param>
        /// <param name="callback">Result-Action</param>
        public void EditScene(string name, double number, PalEdit.Param param = PalEdit.Param.Default, PalEdit.SkSelect skSelect = PalEdit.SkSelect.Default, PalEdit.SMode saveMode = PalEdit.SMode.Default, Action<Enums.FehlerT> callback = null)
        {
            EditPal(
                name,
                number,
                MLUtil.MLPalFlag.BLK,
                param,
                skSelect,
                saveMode,
                PalEdit.Cmd.Update,
                callback
            );
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
            string name,
            double number,
            MLUtil.MLPalFlag type,
            PalEdit.Param param = PalEdit.Param.Default,
            PalEdit.SkSelect select = PalEdit.SkSelect.Default,
            PalEdit.SMode smode = PalEdit.SMode.Default,
            PalEdit.Cmd action = PalEdit.Cmd.Create,
            Action<Enums.FehlerT> callback = null)
        {
            Query(
                new PalEdit(
                    action,
                    new PalEdit.PalEditEntry(
                        type,
                        (short)(number * 10),
                        0,
                        param,
                        select,
                        smode,
                        name)),
                callback);
        }

        #endregion PalEdit

        #region Anwahl

        public void Select(AWType awType, SK sk, Action<FehlerT> callback = null)
        {
            Query(new SKAnwahl(awType, sk), callback);
        }

        public void Select(AWType awType, SKG skg, Action<FehlerT> callback = null)
        {
            Query(new SKGAnwahl(awType, skg), callback);
        }

        public void Select(AWType awType, params SK[] sk)
        {
            Query(new SKAnwahl(awType, sk));
        }

        public void Select(AWType awType, params SKG[] skg)
        {
            Query(new SKGAnwahl(awType, skg));
        }

        #endregion Anwahl

        #region Events

        public void PushKey   (byte taste     , Action<FehlerT> callback = null) => Query(Event.Chain(new BedientasteEvent(taste, true), new BedientasteEvent(taste, false)), callback);

        public void PushKey   (EnumTaste taste, Action<FehlerT> callback = null) => PushKey((byte)taste, callback);

        public void PushKey   (Taste taste    , Action<FehlerT> callback = null) => PushKey((byte)taste.TastNR, callback);

        public void HoldKey   (byte taste     , Action<FehlerT> callback = null) => Query(new BedientasteEvent(taste, true), callback);

        public void HoldKey   (EnumTaste taste, Action<FehlerT> callback = null) => HoldKey((byte)taste, callback);

        public void HoldKey   (Taste taste    , Action<FehlerT> callback = null) => HoldKey((byte)taste.TastNR, callback);

        public void ReleaseKey(byte taste     , Action<FehlerT> callback = null) => Query(new BedientasteEvent(taste, false), callback);

        public void ReleaseKey(EnumTaste taste, Action<FehlerT> callback = null) => ReleaseKey((byte)taste, callback);

        public void ReleaseKey(Taste taste    , Action<FehlerT> callback = null) => ReleaseKey((byte)taste.TastNR, callback);

        public void PushKeys(Action<FehlerT> callback = null, params byte[] keys)
        {
            Event[] events = new Event[keys.Length * 2];
            for(int i = 0; i < keys.Length; i++)
            {
                events[ i * 2     ] = new BedientasteEvent(keys[i], true);
                events[(i * 2) + 1] = new BedientasteEvent(keys[i], false);
            }
            Query(Event.Chain(events), callback);
        }
        //should be a lot faster than mapping [] to byte[]

        public void PushKeys(Action<FehlerT> callback = null, params EnumTaste[] keys)
        {
            Event[] events = new Event[keys.Length * 2];
            for (int i = 0; i < keys.Length; i++)
            {
                events[ i * 2     ] = new BedientasteEvent((byte)keys[i], true);
                events[(i * 2) + 1] = new BedientasteEvent((byte)keys[i], false);
            }
            Query(Event.Chain(events), callback);
        }

        public void PushKey(Action<FehlerT> callback = null, params Taste[] tasten)
        {
            Event[] events = new Event[tasten.Length * 2];
            for (int i = 0; i < tasten.Length; i++)
            {
                events[ i * 2     ] = new BedientasteEvent((byte)tasten[i].TastNR, true);
                events[(i * 2) + 1] = new BedientasteEvent((byte)tasten[i].TastNR, false);
            }
            Query(Event.Chain(events), callback);
        }



        public void MoveMouse(byte x, byte y, Action<FehlerT> callback = null)
        {
            Query(new MouseEvent(x, y, 0), callback);
        }

        #endregion

        /// <summary>
        /// Push changed intensities
        /// </summary>
        /// <param name="src">Can be null, either ActiveSK or AllSK</param>
        /// <param name="dst">Destination register</param>
        /// <param name="callback">Result-Action</param>
        public void PushChanges(List<SK> src = null, FixParDst dst = FixParDst.Current, Action<FehlerT> callback = null)
        {
            if (src == null)
                src = ActiveSK;

            List<SK> changed = src.FindAll((sk) => sk.dirty);
            changed.ForEach((sk) => sk.dirty = false);

            Query(new FixParDimmer(dst, changed.ToArray()), callback);
        }
    }
}
