using System;
using System.Collections.Generic;
using System.Text;

namespace EffectSystem
{
    public abstract class Effect
    {
        public abstract double Length { get; }

        public DateTime Start { get; }

        protected Effect()
        {
            Start = DateTime.Now;
        }

        public abstract EffectState Update(double time);
    }
}
