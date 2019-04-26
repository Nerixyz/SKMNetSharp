﻿using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Client
{
    /// <summary>
    /// Palettendaten-Auswahlaktion
    /// </summary>
    public class PalCommand : SplittableHeader
    {
        public override short Type => 25;

        private readonly PalCmdEntry[] commands;
        private readonly Cmd command;

        public PalCommand(Cmd command = Cmd.Abs, params PalCmdEntry[] commands)
        {
            this.commands = commands;

            this.command = command;
        }

        public override IEnumerable<byte[]> GetData(LightingConsole console)
        {
            return Make(
                commands,
                2,
                CountShort,
                new Action<ByteBuffer, int>((buf, _) => buf.Write(console.BdstNo).Write((short)command)),
                new Action<PalCmdEntry, ByteBuffer>((entry, buf) => buf.Write(entry.palkenn).Write(entry.palno))
           );
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
