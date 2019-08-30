using System.Collections.Generic;

namespace SKMNET.Client.Networking.Client.Ext
{
    // max len = 80 
    /// <summary>
    /// Mailbox-Aufträge
    /// </summary>
    public class Mailbox : SplittableHeader
    {
        public override short Type => 22;

        private readonly MailboxEntry[] entries;

        public Mailbox(params MailboxEntry[] entry)
        {
            entries = entry;
        }

        public override IEnumerable<byte[]> GetData(LightingConsole console) =>
            Make(
                entries,
                80,
                CountShort,
                (buf, _) => buf.WriteShort(console.BdstNo).Write(0),
                (entry, buf) => buf.Write(entry.MailboxNo).Write(entry.Pad).Write(entry.Job).Write(entry.Par1).Write(entry.Par2)
           );

        public class MailboxEntry
        {
            public readonly short MailboxNo;
            public readonly short Pad;
            public readonly int Job;
            public readonly int Par1;
            public readonly int Par2;

            public MailboxEntry(short mailboxNo, int job, int par1, int par2, short pad = 0)
            {
                MailboxNo = mailboxNo;
                Pad = pad;
                Job = job;
                Par1 = par1;
                Par2 = par2;
            }
        }

    }
}
