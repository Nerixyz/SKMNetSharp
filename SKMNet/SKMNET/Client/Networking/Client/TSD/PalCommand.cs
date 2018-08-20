using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Client
{
    public class PalCommand : SplittableHeader
    {
        public override short Type => 25;

        private readonly List<PalCmdEntry> entries;
        private readonly Cmd command;
        private readonly short bdstNo;

        public PalCommand(List<PalCmdEntry> entries, Cmd command = Cmd.Abs, short bdstNo = 0)
        {
            this.entries = entries;
            this.command = command;
            this.bdstNo = bdstNo;
        }

        public PalCommand(PalCmdEntry entry, Cmd command = Cmd.Abs, short bdstNo = 0)
        {
            this.entries = new List<PalCmdEntry>();
            entries.Add(entry);

            this.command = command;
            this.bdstNo = bdstNo;
        }

        public override List<byte[]> GetData()
        {
            return Make(entries, 2, CountShort, new Action<ByteBuffer, int>((buf, total) =>
            {
                buf.Write(bdstNo).Write((short)command);
            }), new Action<PalCmdEntry, ByteBuffer>((entry, buf) =>
            {
                buf.Write(entry.palkenn).Write(entry.palno);
            }));
        }

        public class PalCmdEntry
        {
            public readonly short palkenn;
            public readonly short palno;

            public PalCmdEntry(MLUtil.MLPalFlag mask, short palno)
            {
                this.palkenn = (short)mask;
                this.palno = palno;
            }
        }

        public enum Cmd
        {
            Abs,
            Add,
            Subtract
        }
    }
}
