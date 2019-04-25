﻿using SKMNET.Client.Networking.Client;
using SKMNET.Client.Stromkreise;
using SKMNET.Client.Stromkreise.ML;
using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Client
{
    /// <summary>
    /// Geräte-orientierte Parameterwerte
    /// </summary>
    public class FixPar : SplittableHeader
    {
        private readonly ValueType valueType;
        private readonly Enums.FixParDst dstReg;
        private readonly MLParameter[] parameters;
        public override short Type => 20;

        public override List<byte[]> GetData(LightingConsole console)
        {
            return Make(
                parameters,
                200,
                CountShort,
                new Action<ByteBuffer, int>((buf, _) => buf.Write(console.BdstNo).Write((short)valueType).Write((short)dstReg)),
                new Action<MLParameter, ByteBuffer>((par, buf) =>
                       {
                           if (par.SK == null)
                               throw new NullReferenceException("MLParameter.SK not set");
                           buf.Write((short)par.SK.Number).Write(par.ParNo).Write((short)((int)par.Value << 8));
                       })
            );
        }

        public FixPar(ValueType type = ValueType.ABS, Enums.FixParDst reg = Enums.FixParDst.Current, params MLParameter[] parameters)
        {
            this.parameters = parameters;
            this.valueType = type;
            this.dstReg = reg;
        }

        public FixPar(ValueType type = ValueType.ABS, Enums.FixParDst reg = Enums.FixParDst.Current, params SK[] sks)
        {
            int estSize = 0;
            foreach(SK s in sks) estSize += s.Parameters.Count;

            this.parameters = new MLParameter[estSize];
            for(int i = 0, pointer = 0; i < sks.Length; i++)
            {
                foreach(MLParameter parameter in sks[i].Parameters)
                {
                    this.parameters[i] = parameter;

                    pointer++;
                }
            }
            valueType = type;
            this.dstReg = reg;
        }

        public enum ValueType
        {
            ABS,
            REL,
            HOME,
            PLUS,
            MINUS
        }

    }
}
