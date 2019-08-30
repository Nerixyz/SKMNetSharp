using System.Collections.Generic;
using SKMNET.Util;

namespace SKMNET.Client.Networking.Client.MLC
{
    /// <summary>
    /// Palettendaten bearbeiten
    /// </summary>
    public class PalEdit : SplittableHeader
    {
        public override short Type => 26;

        private readonly short subcmd;
        private readonly short editcmd;
        private readonly PalEditEntry[] entries;

        public PalEdit(Cmd cmd, params PalEditEntry[] entries)
        {
            this.entries = entries;
            editcmd = (short) cmd;
            subcmd = 0;
        }

        public override IEnumerable<byte[]> GetData(LightingConsole console) =>
            Make(entries, 30, CountShort, (buf, _) => buf
                .Write(console.BdstNo)
                .Write(subcmd)
                .Write(editcmd), (entry, buf) => buf
                .Write(entry.Palkenn)
                .Write(entry.PalNo)
                .Write(entry.PalMask)
                .Write(entry.Param)
                .Write(entry.SKSelect)
                .Write(entry.Smode)
                .Write(entry.Text, 31)
                .Write((byte) 0));

        public class PalEditEntry
        {
            public readonly short Palkenn;
            public readonly short PalNo;
            public readonly short PalMask;
            public readonly short Param;
            public readonly short SKSelect;
            public readonly short Smode;
            public readonly string Text;

            public PalEditEntry(MlUtil.MlPalFlag palkenn, short palNo, MlUtil.MlPalFlag palMask, Param param,
                SkSelect skSelect, SMode smode, string text)
            {
                Palkenn = (short) palkenn;
                PalNo = palNo;
                PalMask = (short) palMask;
                Param = (short) param;
                SKSelect = (short) skSelect;
                Smode = (short) smode;
                Text = text;
            }
        }

        public enum Param
        {
            Default,
            All,
            Bet,
            Mod
        }

        public enum SkSelect
        {
            Default,
            All,
            Aw
        }

        public enum SMode
        {
            Default,
            All,
            Pos,
            FW,
            LED,
            Media
        }

        public enum Cmd
        {
            Create = 1,
            Rename,
            Delete,
            Update
        }
    }
}