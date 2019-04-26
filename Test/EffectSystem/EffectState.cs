using System;
using System.Collections.Generic;
using System.Text;

namespace EffectSystem
{
    public struct EffectState
    {
        public IEnumerable<EffectPar> Parameters;
    }

    public struct EffectPar
    {
        public int Fixture { get; set; }

        public short ParNum { get; set; }

        public byte Value { get; set; }
    }
}
