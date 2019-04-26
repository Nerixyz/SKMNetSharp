using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using SKMNET.Client.Stromkreise.ML;

namespace EffectSystem.Effects
{
    public class ColorEffect : Effect
    {
        public override double Length => 0.001;

        private IEnumerable<int> Fixtures;
        private EffectColor Color;

        public ColorEffect(IEnumerable<int> fixtures, EffectColor color)
        {
            Fixtures = fixtures;
            Color = color;
        }

        public override EffectState Update(double time)
        {
            return new EffectState
            {
                Parameters = Fixtures.SelectMany(x => new[]
                {
                    new EffectPar {Fixture = x, ParNum = FixParMap.Active.RED,   Value = Color.Red},
                    new EffectPar {Fixture = x, ParNum = FixParMap.Active.GREEN, Value = Color.Green},
                    new EffectPar {Fixture = x, ParNum = FixParMap.Active.BLUE,  Value = Color.Blue}
                })
            };
        }
    }
}