using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Client
{
    // max len = 80 
    /// <summary>
    /// Mailbox-Aufträge
    /// </summary>
    public class Mailbox : SplittableHeader
    {
        public override short Type => 22;

        private readonly List<MailboxEntry> entries;

        public Mailbox(List<MailboxEntry> entries)
        {
            this.entries = entries;
        }

        public Mailbox(MailboxEntry entry)
        {
            this.entries = new List<MailboxEntry>();
            entries.Add(entry);
        }

        public override List<byte[]> GetData(LightingConsole console) =>
            Make(
                entries,
                80,
                CountShort,
                new Action<ByteBuffer, int>((buf, _) => buf.WriteShort(console.BdstNo).Write(0)),
                new Action<MailboxEntry, ByteBuffer>((entry, buf) => buf.Write(entry.mailboxNo).Write(entry.pad).Write(entry.job).Write(entry.par1).Write(entry.par2))
           );

        public class MailboxEntry
        {
            public readonly short mailboxNo;
            public readonly short pad;
            public readonly int job;
            public readonly int par1;
            public readonly int par2;

            public MailboxEntry(short mailboxNo, int job, int par1, int par2, short pad = 0)
            {
                this.mailboxNo = mailboxNo;
                this.pad = pad;
                this.job = job;
                this.par1 = par1;
                this.par2 = par2;
            }
        }

    }
}
