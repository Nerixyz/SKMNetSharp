﻿using SKMNET.Client.Stromkreise;
using System;
using System.Collections.Generic;
using System.Text;

namespace SKMNET.Client.Networking.Client
{
    public class FixParDimmer : SplittableHeader
    {
        private readonly short subCmd;
        private readonly Enums.FixParDst dstReg;
        private readonly SK[] sks;
        public override short Type => 20;

        public override IEnumerable<byte[]> GetData(LightingConsole console) =>
            Make(
                sks,
                200,
                CountShort,
                new Action<ByteBuffer, int>((buf, _) => buf.Write(console.BdstNo).Write(subCmd).Write((short)dstReg)),
                new Action<SK, ByteBuffer>((par, buf) => buf.Write((short)par.Number).WriteShort(0).Write((short)((int)par.Intensity << 8)))
            );

        public FixParDimmer( Enums.FixParDst reg = Enums.FixParDst.Current, params SK[] sks)
        {
            this.sks = sks;
            this.subCmd = 0; //ABS
            this.dstReg = reg;
        }
    }
}
