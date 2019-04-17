using System;
using System.Collections.Generic;
using System.Text;

namespace EffectSystem
{
    public abstract class Effect
    {
        public abstract double Length { get; }

        public DateTime Start { get; set; }

        public Effect()
        {
            this.Start = DateTime.Now;
        }

        public abstract EffectState Update(double time);

        public static class FixParNum
        {
            public const short DIMMER = 0;
        }
    }
}
