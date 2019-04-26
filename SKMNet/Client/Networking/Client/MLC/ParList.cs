﻿using SKMNET.Client.Stromkreise;
using System.Collections.Generic;

namespace SKMNET.Client.Networking.Client
{
    /// <summary>
    /// Anforderungsliste ML-Parameterwerte
    /// </summary>
    public class ParList : SplittableHeader
    {
        public override short Type => 29;

        private readonly SK[] entries;
        private readonly bool sendRange;

        public ParList(bool sendRange = true, params SK[] sks)
        {
            entries = sks;
            this.sendRange = sendRange;
        }

        public override IEnumerable<byte[]> GetData(LightingConsole console)
        {
            return Make(
                entries,
                230,
                CountShort,
                (buf, total) => buf.Write(console.BdstNo).Write((short)0).Write((short)(total + 230 > entries.Length ? 1 : 0)),
                (sk, buf) => buf.Write(sk.Number).Write((short)-2).Write((short)(sendRange ? 0x0001 : 0))
           );
        }
    }
}
