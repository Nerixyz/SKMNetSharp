using SKMNET.Client.Stromkreise.ML;
using System;
using System.Collections.Generic;
using System.Text;

namespace EffectSystem.Effects
{
    public class DimmerFade : Effect
    {
        public override double Length => 1.0;

        public override EffectState Update(double time)
        {
            return new EffectState
            {
                Parameters = new[]
                {
                    new EffectPar
                    {
                       Fixture = 1,
                       ParNum = FixParMap.Active.DIMMER,
                       Value = (byte)((1.0 - time) * 255)
                    }
                }
            };
        }
    }
}
