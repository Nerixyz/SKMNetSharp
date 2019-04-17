using System;
using System.Collections.Generic;
using System.Text;

namespace EffectSystem
{
    public abstract class EffectInfo
    {
        public ConsoleKey Key { get; set; }
        public abstract Effect Init();
    }
    public class EffectInfo<T> : EffectInfo where T : Effect
    {
        public override Effect Init()
        {
            return (Effect) typeof(T).GetConstructors()[0].Invoke(new object[] { });
        }
    }
}
