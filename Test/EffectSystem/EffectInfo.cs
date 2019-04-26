using System;
namespace EffectSystem
{
    public abstract class EffectInfo
    {
        public ConsoleKey Key { get; set; }
        
        /// <summary>
        /// Constructor-Arguments
        /// </summary>
        public object[] Arguments { get; set; } = {};
        
        public abstract Effect Init();
    }
    public class EffectInfo<T> : EffectInfo where T : Effect
    {
        public override Effect Init()
        {
            return (Effect) typeof(T).GetConstructors()[0].Invoke(Arguments);
        }
    }
}
