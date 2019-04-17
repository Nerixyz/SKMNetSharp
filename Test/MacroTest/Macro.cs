using SKMNET;
using SKMNET.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SKMNET.Enums;

namespace MacroTest
{
    public class Macro
    {
        public List<MacroStep> Steps { get; set; }
        public ConsoleKey Key { get; set; }

        public async Task<FehlerT[]> ExecuteAsync(LightingConsole console)
        {
            FehlerT[] fehler = new FehlerT[Steps.Count];
            for(int i = 0; i < Steps.Count; i++)
            {
                fehler[i] = await Steps[i].ExecuteAsync(console);
            }
            return fehler;
        }

        public void Execute(LightingConsole console, bool wait = false)
        {
            foreach(MacroStep step in Steps)
            {
                if (wait)
                    step.ExecuteAsync(console).Wait();
                else
                    step.ExecuteAsync(console).ConfigureAwait(false);
            }
        }
    }

    public abstract class MacroStep
    {
        public abstract Task<FehlerT> ExecuteAsync(LightingConsole console);
    }

    public class MacroStepKey : MacroStep
    {
        public List<EnumTaste> Keys { get; set; }
        public override async Task<FehlerT> ExecuteAsync(LightingConsole console) => await console.PushKeys(Keys.ToArray());
    }

    public class MacroStepHoldKey : MacroStep
    {
        public EnumTaste Key { get; set; }
        public override async Task<FehlerT> ExecuteAsync(LightingConsole console) => await console.HoldKey(Key);
    }

    public class MacroStepReleaseKey : MacroStep
    {
        public EnumTaste Key { get; set; }
        public override async Task<FehlerT> ExecuteAsync(LightingConsole console) => await console.ReleaseKey(Key);
    }
}
