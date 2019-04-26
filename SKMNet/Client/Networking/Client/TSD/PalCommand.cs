﻿using SKMNET.Util;
using System.Collections.Generic;
 
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
                (buf, _) => buf.Write(console.BdstNo).Write((short)command),
                (entry, buf) => buf.Write(entry.Palkenn).Write(entry.Palno)
           );
        }

        public class PalCmdEntry
        {
            public readonly short Palkenn;
            public readonly short Palno;

            public PalCmdEntry(MLUtil.MLPalFlag mask, short palno)
            {
                Palkenn = (short)mask;
                this.Palno = palno;
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
