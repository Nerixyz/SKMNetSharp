﻿using SKMNET.Client.Stromkreise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Client
{
    /// <summary>
    /// Anforderungsliste ML-Parameterwerte
    /// </summary>
    public class ParList : SplittableHeader
    {
        public override short Type => 29;

        private readonly List<SK> entries;
        private readonly bool sendRange;

        public ParList(List<SK> list, bool sendRange = true)
        {
            this.entries = list;
            this.sendRange = sendRange;
        }

        public ParList(SK sk, bool sendRange = true)
        {
            this.entries = new List<SK>()
            {
                sk
            };
            this.sendRange = sendRange;
        }

        public override List<byte[]> GetData(LightingConsole console)
        {
            return Make(entries, 230, CountShort, new Action<ByteBuffer, int>((buf, total) =>
            {
                buf.Write(console.BdstNo).Write((short)0).Write((short)((total + 230 > entries.Count) ? 1 : 0));
            }), new Action<SK, ByteBuffer>((sk, buf) =>
            {
                buf.Write(sk.Number).Write((short)-2).Write((short)(sendRange ? 0x0001 : 0));
            }));
        }
    }
}
