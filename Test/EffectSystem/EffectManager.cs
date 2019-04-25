using SKMNET;
using SKMNET.Client;
using SKMNET.Client.Networking.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace EffectSystem
{
    public class EffectManager
    {
        public List<EffectInfo> effectInfos;
        public List<Effect> activeEffects;

        private readonly Timer timer;

        private LightingConsole console;

        public EffectManager(List<EffectInfo> effectInfos, double intervalS = 1.0 / 44.0)
        {
            this.effectInfos = effectInfos;
            activeEffects = new List<Effect>();
            timer = new Timer(intervalS * 1000.0);
        }

        public void BlockThread(LightingConsole console)
        {
            this.console = console;
            this.timer.Elapsed += Timer_Elapsed;
            this.timer.Start();
            ConsoleKeyInfo info;
            while((info = Console.ReadKey(true)).Key != ConsoleKey.Escape)
            {
                var effect = effectInfos.Find((x) => x.Key == info.Key);
                if (effect != null)
                    activeEffects.Add(effect.Init());
            }
        }

        private async void Timer_Elapsed(object sender, ElapsedEventArgs args)
        {
            List<int> toRemove = new List<int>();
            EffectState[] states = new EffectState[activeEffects.Count];
            for (int i = 0; i < activeEffects.Count; i++)
            {
                Effect e = activeEffects[i];
                double timePassed = (DateTime.Now - e.Start).TotalSeconds;
                if (timePassed > e.Length)
                {
                    toRemove.Add(i);
                    continue;
                }
                states[i] = e.Update(timePassed);
            }
            toRemove.ForEach((x) => activeEffects.RemoveAt(x));

            if (activeEffects.Count > 0)
            {
                List<EffectPar> parameters = new List<EffectPar>();

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
                    console.Logger?.Log("[EffectManager] response not FT_OK: " + fehler.ToString());
            }
        }

        public sealed class CustomFixPar : SplittableHeader
        {
            public override short Type => 20;

            private readonly List<SkInfo> infos;
            private readonly FixPar.ValueType valueType;
            private readonly Enums.FixParDst dstReg;

            public override List<byte[]> GetData(LightingConsole console)
            {
                return Make(
                infos.ToArray(),
                200,
                CountShort,
                new Action<ByteBuffer, int>((buf, _) => buf.Write((short)console.Bedienstelle).Write((short)valueType).Write((short)dstReg)),
                new Action<SkInfo, ByteBuffer>((par, buf) =>
                {
                    foreach(KeyValuePair<short, byte> pair in par.parameters)
                    {
                        buf.Write((short)par.num).WriteShort(pair.Key).WriteShort((short)((pair.Value) << 8));
                    }
                })
            );
            }

            public CustomFixPar(Dictionary<int, Dictionary<short, byte>> mapping, FixPar.ValueType type = FixPar.ValueType.ABS, Enums.FixParDst reg = Enums.FixParDst.Current)
            {
                infos = new List<SkInfo>(mapping.Count);
                foreach (KeyValuePair<int, Dictionary<short, byte>> pair in mapping)
                {
                    infos.Add(new SkInfo()
                    {
                        num = pair.Key,
                        parameters = pair.Value
                    });
                }
                this.valueType = type;
                this.dstReg = reg;
            }

            private struct SkInfo
            {
                public int num;
                public Dictionary<short, byte> parameters;
            }
        }
    }
}
