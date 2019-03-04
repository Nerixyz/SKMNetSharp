using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Client
{
    /// <summary>
    /// Palettendaten bearbeiten
    /// </summary>
    public class PalEdit : SplittableHeader
    {
        public override short Type => 26;

        private readonly short subcmd;
        private readonly short editcmd;
        private readonly List<PalEditEntry> entries;

        public PalEdit(List<PalEditEntry> entries, Cmd cmd, short subcmd = 0)
        {
            this.entries = entries;
            this.editcmd = (short)cmd;
            this.subcmd = subcmd;
        }

        public PalEdit(PalEditEntry entry, Cmd cmd, short subcmd = 0)
        {
            entries = new List<PalEditEntry>
            {
                entry
            };
            this.editcmd = (short)cmd;
            this.subcmd = subcmd;
        }

        public override List<byte[]> GetData(LightingConsole console)
        {
            return Make(entries, 30, CountShort, new Action<ByteBuffer, int>((buf, _) =>
            {
                buf
                    .Write(console.BdstNo)
                    .Write(subcmd)
                    .Write(editcmd);
            }), new Action<PalEditEntry, ByteBuffer>((entry, buf) =>
            {
                buf
                    .Write(entry.palkenn)
                    .Write(entry.palNo)
                    .Write(entry.palMask)
                    .Write(entry.param)
                    .Write(entry.skSelect)
                    .Write(entry.smode)
                    .Write(entry.text, 31)
                    .Write((byte)0);
            }));
        }

        public class PalEditEntry
        {
            public readonly short palkenn;
            public readonly short palNo;
            public readonly short palMask;
            public readonly short param;
            public readonly short skSelect;
            public readonly short smode;
            public readonly string text;

            public PalEditEntry(MLUtil.MLPalFlag palkenn, short palNo, MLUtil.MLPalFlag palMask, Param param, SkSelect skSelect, SMode smode, string text)
            {
                this.palkenn = (short) palkenn;
                this.palNo = palNo;
                this.palMask = (short)palMask;
                this.param = (short)param;
                this.skSelect = (short)skSelect;
                this.smode = (short)smode;
                this.text = text;
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
