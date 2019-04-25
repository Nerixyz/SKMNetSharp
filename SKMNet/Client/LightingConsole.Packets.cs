using SKMNET.Client.Networking.Client;
using SKMNET.Client.Stromkreise;
using SKMNET.Client.Tasten;
using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SKMNET.Enums;

namespace SKMNET.Client
{
    public sealed partial class LightingConsole
    {
        #region PalEdit
        /// <summary>
        /// Create a Scene
        /// </summary>
        /// <param name="name">LTX</param>
        /// <param name="number">BLK Nr.</param>
        public async Task<FehlerT> CreateScene(string name,
                                               double number)
        {
            return await EditPal(
                name,
                number,
                MLUtil.MLPalFlag.BLK).ConfigureAwait(false);
        }

        /// <summary>
        /// Edit an existing scene
        /// </summary>
        /// <param name="name">LTX</param>
        /// <param name="number">BLK Nr.</param>
        /// <param name="param">Parameterauswahl</param>
        /// <param name="skSelect">Geräteauswahl</param>
        /// <param name="saveMode">Schreibmodus</param>
        public async Task<FehlerT> EditScene(string name,
                                             double number,
                                             PalEdit.Param param = PalEdit.Param.Default,
                                             PalEdit.SkSelect skSelect = PalEdit.SkSelect.Default,
                                             PalEdit.SMode saveMode = PalEdit.SMode.Default)
        {
            return await EditPal(
                name,
                number,
                MLUtil.MLPalFlag.BLK,
                param,
                skSelect,
                saveMode,
                PalEdit.Cmd.Update
            ).ConfigureAwait(false);
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
        public async Task<FehlerT> EditPal(string name,
                                           double number,
                                           MLUtil.MLPalFlag type,
                                           PalEdit.Param param = PalEdit.Param.Default,
                                           PalEdit.SkSelect select = PalEdit.SkSelect.Default,
                                           PalEdit.SMode smode = PalEdit.SMode.Default,
                                           PalEdit.Cmd action = PalEdit.Cmd.Create)
        {
            return await QueryAsync(new PalEdit(action,
                                                new PalEdit.PalEditEntry(type,
                                                                         (short)(number * 10),
                                                                         0,
                                                                         param,
                                                                         select,
                                                                         smode,
                                                                         name))).ConfigureAwait(false);
        }

        #endregion PalEdit

        #region Anwahl

        public async Task<FehlerT> Select   (AWType awType,        SK      sk)  => await QueryAsync(new SKAnwahl (awType, sk)).ConfigureAwait(false);

        public async Task<FehlerT> Select   (AWType awType,        short   sk)  => await QueryAsync(new SKAnwahl (awType, sk )).ConfigureAwait(false);
        public async Task<FehlerT> Select   (AWType awType, params SK   [] sk)  => await QueryAsync(new SKAnwahl (awType, sk )).ConfigureAwait(false);
        public async Task<FehlerT> Select   (AWType awType, params short[] sk)  => await QueryAsync(new SKAnwahl (awType, sk )).ConfigureAwait(false);
        public async Task<FehlerT> Select   (AWType awType,        SKG     skg) => await QueryAsync(new SKGAnwahl(awType, skg)).ConfigureAwait(false);
        public async Task<FehlerT> Select   (AWType awType, params SKG  [] skg) => await QueryAsync(new SKGAnwahl(awType, skg)).ConfigureAwait(false);
        public async Task<FehlerT> SelectSKG(AWType awType,        short   skg) => await QueryAsync(new SKGAnwahl(awType, skg)).ConfigureAwait(false);
        public async Task<FehlerT> SelectSKG(AWType awType, params short[] skg) => await QueryAsync(new SKGAnwahl(awType, skg)).ConfigureAwait(false);

        #endregion Anwahl

        #region Events

        public async Task<FehlerT> PushKey   (byte taste     ) => await QueryAsync(Event.Chain(new BedientasteEvent(taste, true), new BedientasteEvent(taste, false))).ConfigureAwait(false);

        public async Task<FehlerT> PushKey   (EnumTaste taste) => await PushKey((byte)taste).ConfigureAwait(false);

        public async Task<FehlerT> PushKey   (Taste taste    ) => await PushKey((byte)taste.TastNR).ConfigureAwait(false);

        public async Task<FehlerT> HoldKey   (byte taste     ) => await QueryAsync(new BedientasteEvent(taste, true)).ConfigureAwait(false);

        public async Task<FehlerT> HoldKey   (EnumTaste taste) => await HoldKey((byte)taste).ConfigureAwait(false);

        public async Task<FehlerT> HoldKey   (Taste taste    ) => await HoldKey((byte)taste.TastNR).ConfigureAwait(false);

        public async Task<FehlerT> ReleaseKey(byte taste     ) => await QueryAsync(new BedientasteEvent(taste, false)).ConfigureAwait(false);

        public async Task<FehlerT> ReleaseKey(EnumTaste taste) => await ReleaseKey((byte)taste).ConfigureAwait(false);

        public async Task<FehlerT> ReleaseKey(Taste taste    ) => await ReleaseKey((byte)taste.TastNR).ConfigureAwait(false);

        public async Task<FehlerT> PushKeys(params byte[] keys)
        {
            Event[] events = new Event[keys.Length * 2];
            for(int i = 0; i < keys.Length; i++)
            {
                events[i * 2    ] = new BedientasteEvent(keys[i], true);
                events[i * 2 + 1] = new BedientasteEvent(keys[i], false);
            }
            return await QueryAsync(Event.Chain(events)).ConfigureAwait(false);
        }
        //should be a lot faster than mapping [] to byte[]

        public async Task<FehlerT> PushKeys(params EnumTaste[] keys)
        {
            Event[] events = new Event[keys.Length * 2];
            for (int i = 0; i < keys.Length; i++)
            {
                events[i * 2    ] = new BedientasteEvent((byte)keys[i], true);
                events[i * 2 + 1] = new BedientasteEvent((byte)keys[i], false);
            }
            return await QueryAsync(Event.Chain(events)).ConfigureAwait(false);
        }

        public async Task<FehlerT> PushKey(params Taste[] tasten)
        {
            Event[] events = new Event[tasten.Length * 2];
            for (int i = 0; i < tasten.Length; i++)
            {
                events[ i * 2     ] = new BedientasteEvent((byte)tasten[i].TastNR, true);
                events[(i * 2) + 1] = new BedientasteEvent((byte)tasten[i].TastNR, false);
            }
            return await QueryAsync(Event.Chain(events)).ConfigureAwait(false);
        }

        public async Task<FehlerT> MoveMouse(byte x, byte y)
        {
            return await QueryAsync(new MouseEvent(x, y, 0)).ConfigureAwait(false);
        }

        #endregion

        /// <summary>
        /// Push changed intensities
        /// </summary>
        /// <param name="dst">Destination register</param>
        public async Task<FehlerT> PushChanges(FixParDst dst = FixParDst.Current)
        {
            SK[] changed = Stromkreise.Where(s => s?.dirty == true).ToArray();


            foreach(SK s in changed) s.dirty = false;

            return await QueryAsync(new FixParDimmer(dst, changed.ToArray())).ConfigureAwait(false);
        }
    }
}
