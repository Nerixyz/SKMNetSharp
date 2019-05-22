﻿using SKMNET.Client.Networking.Client;
using SKMNET.Client.Stromkreise;
using SKMNET.Client.Stromkreise.ML;
using System;
 
namespace SKMNET.Client.Networking.Server.TSD
{
    [Serializable]
    public class SelPar : SPacket
    {
        public ushort Fixture;
        public string FixtureName;
        public bool Last;
        public ushort Count;
        public SelParData[] Parameters;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            Fixture = buffer.ReadUShort();
            FixtureName = buffer.ReadString(8);
            Last = buffer.ReadUShort() != 0;
            Count = buffer.ReadUShort();
            Parameters = new SelParData[Count];
            for(int i = 0; i < Count; i++)
            {
                Parameters[i] = new SelParData(
                    buffer.ReadShort(),
                    buffer.ReadUShort(),
                    buffer.ReadString(8),
                    buffer.ReadString(8),
                    buffer.ReadString(8));
            }
            return this;
        }

        public override Enums.Response ProcessPacket(LightingConsole console, int type)
        {
            //Apply to SK
            SK sk = console.Stromkreise[Fixture];
            if (sk == null) return Enums.Response.BadCmd;
            
            if(type == 160 /* SKMON_MLPAR_REMOVE */)
            {
                sk.Parameters.Clear();
                return Enums.Response.OK;
            }

            // load MLCParams
            foreach (SelParData par in Parameters)
            {
                MLCParameter mlcParameter = console.MLCParameters.Find(x => x.Number == par.Parno);
                if(mlcParameter == null)
                {
                    MLCParameter parameter = new MLCParameter(par.Parno, Enums.SelRangeDisp.Normal, par.Parname);
                    console.MLCParameters.Add(parameter);

                    //get info (not loaded yet)
                    console.QueryAsync(new ParSelect(par.Parno)).ConfigureAwait(false);
                }
                else
                {
                    mlcParameter.Name = par.Parname;
                }
                MLParameter param = sk.Parameters.Find(inc => inc.ParNo == par.Parno);
                if (param != null)
                {
                    param.Value = (par.Val16 & 0xff00) >> 8;
                    param.Display = par.Parval;
                    param.PalName = par.Palname;
                    param.Sk = sk;
                }
                else
                {
                    param = new MLParameter(par.Parname, par.Parno, (par.Val16 & 0xff00) >> 8)
                    {
                        PalName = par.Palname,
                        Display = par.Parname,
                        Sk = sk
                    };
                    sk.Parameters.Add(param);
                }
            }
            return Enums.Response.OK;

        }

        [Serializable]
        public struct SelParData
        {
            /// <summary>
            /// Parameternummer (0-199)
            /// </summary>
            public readonly short Parno;

            /// <summary>
            /// Parameterwert
            /// </summary>
            public readonly ushort Val16;

            /// <summary>
            /// Parametername
            /// </summary>
            public readonly string Parname;

            /// <summary>
            /// Parameterwert als String
            /// </summary>
            public readonly string Parval;

            /// <summary>
            /// Palettenname als String
            /// </summary>
            public readonly string Palname;

            public SelParData(short parno, ushort val16, string parname, string parval, string palname)
            {
                Parno = parno;
                Val16 = val16;
                Parname = parname;
                Parval = parval;
                Palname = palname;
            }
        }
    }
}
