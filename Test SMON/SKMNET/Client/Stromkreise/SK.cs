using SKMNET.Client.Stromkreise.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Stromkreise
{
    class SK
    {
        public short Number { get; }
        public short DMXOut;
        public short DMXLine;
        public List<MLParameter> Parameters { get; set; }
        public byte Intensity { get; set; }

        public SK(short Number, byte Intensity = 0, short DMXAdress = -1, short DMXLine = 1)
        {
            this.Number = Number;
            this.Intensity = Intensity;
            if (DMXAdress == -1)
                DMXOut = Number;
            else
                DMXOut = DMXAdress;
            this.DMXLine = DMXLine;

        }

        public void AddParameter(MLParameter param)
        {
            if(Parameters == null)
            {
                Parameters = new List<MLParameter>();
            }
            if(param.ParNo == -1)
            {
                param.ParNo = (short)Parameters.Count;
            }
            Parameters.Insert(param.ParNo, param);
        }
    }
}
