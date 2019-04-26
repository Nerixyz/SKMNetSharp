using SKMNET;
using SKMNET.Client;
using SKMNET.Client.Networking.Client;
using System;
using System.Collections.Generic;
using System.Timers;

namespace EffectSystem
{
    public class EffectManager
    {
        public readonly List<EffectInfo> EffectInfos;
        public List<Effect> ActiveEffects;

        private readonly Timer timer;

        private LightingConsole console;

        public EffectManager(List<EffectInfo> effectInfos, double intervalS = 1.0 / 44.0)
        {
            EffectInfos = effectInfos;
            ActiveEffects = new List<Effect>();
            timer = new Timer(intervalS * 1000.0);
        }

        public void BlockThread(LightingConsole consoleIn)
        {
            console = consoleIn;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
            ConsoleKeyInfo info;
            while((info = Console.ReadKey(true)).Key != ConsoleKey.Escape)
            {
                EffectInfo effect = EffectInfos.Find(x => x.Key == info.Key);
                if (effect != null)
                    ActiveEffects.Add(effect.Init());
            }
        }

        private async void Timer_Elapsed(object sender, ElapsedEventArgs args)
        {
            List<int> toRemove = new List<int>();
            EffectState[] states = new EffectState[ActiveEffects.Count];
            for (int i = 0; i < ActiveEffects.Count; i++)
            {
                Effect e = ActiveEffects[i];
                double timePassed = (DateTime.Now - e.Start).TotalSeconds;
                if (timePassed > e.Length)
                {
                    toRemove.Add(i);
                    continue;
                }
                states[i] = e.Update(timePassed);
            }
            toRemove.ForEach(x => ActiveEffects.RemoveAt(x));

            if (ActiveEffects.Count <= 0) return;
            
            Dictionary<int, Dictionary<short, byte>> mapping = new Dictionary<int, Dictionary<short, byte>>();

            foreach (EffectState state in states)
            {
                if (state.Parameters == null)
                    continue;

                foreach (EffectPar par in state.Parameters)
                {
                    if (!mapping.ContainsKey(par.Fixture))
                        mapping.Add(par.Fixture, new Dictionary<short, byte>());

                    if (!mapping[par.Fixture].ContainsKey(par.ParNum))
                        mapping[par.Fixture].Add(par.ParNum, par.Value);
                    else if (mapping[par.Fixture][par.ParNum] < par.Value)
                        mapping[par.Fixture][par.ParNum] = par.Value;
                }
            }
            Enums.FehlerT fehler = await console.QueryAsync(new CustomFixPar(mapping)).ConfigureAwait(false);
            if (fehler != Enums.FehlerT.FT_OK)
                console.Logger?.Log("[EffectManager] response not FT_OK: " + fehler);
        }

        public sealed class CustomFixPar : SplittableHeader
        {
            public override short Type => 20;

            private readonly List<SkInfo> infos;
            private readonly FixPar.ValueType valueType;
            private readonly Enums.FixParDst dstReg;

            public override IEnumerable<byte[]> GetData(LightingConsole console)
            {
                return Make(
                infos.ToArray(),
                200,
                CountShort,
                (buf, _) => buf.Write((short)console.Bedienstelle).Write((short)valueType).Write((short)dstReg),
                (par, buf) =>
                {
                    foreach((short key, byte value) in par.Parameters)
                    {
                        buf.Write((short)par.Num).WriteShort(key).WriteShort((short)(value << 8));
                    }
                }
            );
            }

            public CustomFixPar(Dictionary<int, Dictionary<short, byte>> mapping, FixPar.ValueType type = FixPar.ValueType.ABS, Enums.FixParDst reg = Enums.FixParDst.Current)
            {
                infos = new List<SkInfo>(mapping.Count);
                foreach ((int key, Dictionary<short, byte> value) in mapping)
                {
                    infos.Add(new SkInfo
                    {
                        Num = key,
                        Parameters = value
                    });
                }
                valueType = type;
                dstReg = reg;
            }

            private struct SkInfo
            {
                public int Num;
                public Dictionary<short, byte> Parameters;
            }
        }
    }
}
