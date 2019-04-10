using SKMNET.Client.Stromkreise;
using SKMNET.Client.Stromkreise.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Server.TSD
{
    [Serializable]
    public class SelPar : SPacket
    {

        public ushort fixture;
        public string fixtureName;
        public bool last;
        public ushort count;
        public SelParData[] parameters;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            fixture = buffer.ReadUShort();
            fixtureName = buffer.ReadString(8);
            last = buffer.ReadUShort() != 0;
            count = buffer.ReadUShort();
            parameters = new SelParData[count];
            for(int i = 0; i < count; i++)
            {
                parameters[i] = new SelParData(
                    buffer.ReadShort(),
                    buffer.ReadUShort(),
                    buffer.ReadString(8),
                    buffer.ReadString(8),
                    buffer.ReadString(8));

            }
            return this;
        }

        public override Enums.Response ProcessPacket(LightingConsole console, ConnectionHandler handler, int type)
        {
            //Apply to SK
            SK sk = console.ActiveSK.Find((inc) => inc.Number == fixture);
            if (sk != null)
            {
                if(type == 160 /* SKMON_MLPAR_REMOVE */)
                {
                    sk.Parameters.Clear();
                    return Enums.Response.OK;
                }

                // load MLCParams
                foreach (SelParData par in parameters)
                {
                    MLCParameter mlcParameter = console.MLCParameters.Find((x) => x.Number == par.parno);
                    if(mlcParameter == null)
                    {
                        MLCParameter parameter = new MLCParameter(par.parno, Enums.SelRangeDisp.Normal, par.parname);
                        console.MLCParameters.Add(parameter);
                    }
                    else
                    {
                        mlcParameter.Name = par.parname;
                    }
                    MLParameter param = sk.Parameters.Find((inc) => inc.ParNo == par.parno);
                    if (param != null)
                    {
                        param.Value = (par.val16 & 0xff00) >> 8;
                        param.Display = par.parval;
                        param.PalName = par.palname;
                        param.SK = sk;
                    }
                    else
                    {
                        param = new MLParameter(par.parname, par.parno, (par.val16 & 0xff00) >> 8)
                        {
                            PalName = par.palname,
                            Display = par.parname,
                            SK = sk
                        };
                        sk.Parameters.Add(param);
                    }
                }
                return Enums.Response.OK;
            }
            else
            {
                return Enums.Response.BadCmd;
            }
        }

        [Serializable]
        public struct SelParData
        {
            /// <summary>
            /// Parameternummer (0-199)
            /// </summary>
            public short parno;
            /// <summary>
            /// Parameterwert
            /// </summary>
            public ushort val16;
            /// <summary>
            /// Parametername
            /// </summary>
            public string parname;
            /// <summary>
            /// Parameterwert als String
            /// </summary>
            public string parval;
            /// <summary>
            /// Palettenname als String
            /// </summary>
            public string palname;

            public SelParData(short parno, ushort val16, string parname, string parval, string palname)
            {
                this.parno = parno;
                this.val16 = val16;
                this.parname = parname;
                this.parval = parval;
                this.palname = palname;
            }
        }
    }
}
