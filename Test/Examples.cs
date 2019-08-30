using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using EffectSystem;
using SKMNET;
using SKMNET.Client;
using SKMNET.Client.Networking.Client;
using SKMNET.Client.Networking.Client.Ext;

namespace Test
{
    public static class Examples
    {
        #region Rainbow
        
        /// <summary>
        /// Creates a proper rainbow-effect
        /// </summary>
        /// <param name="console">associated console</param>
        /// <param name="sk">rainbow-sks</param>
        public static void Rainbow(LightingConsole console, params short[] sk)
        {
            Timer t = new Timer(1.0 / 44.0);
            t.Elapsed += (sender, args) =>
            {
                console.Query(
                    new FixParColor(
                        new Dictionary<short, Color>(
                            sk.Select(x =>
                                    {
                                        double hue = (DateTime.Now.Ticks / 100.0) % 360.0;
                                        Console.WriteLine(Math.Round(hue, 2));
                                        HsvToRgb(hue, 1.0, 1.0, out int r, out int g, out int b);
                                        return new KeyValuePair<short, Color>(x, Color.FromArgb(255, r, g, b));
                                    }
                                )
                            )
                        )
                    );
                
            };
            t.Start();
        }
        
        private static void HsvToRgb(double h, double saturation, double value, out int r, out int g, out int b)
        {
            double H = Math.Abs(h % 360.0);
            double R, G, B;
            if (value <= 0)
            {
                R = G = B = 0;
            }
            else if (saturation <= 0)
            {
                R = G = B = value;
            }
            else
            {
                double hf = H / 60.0;
                int i = (int)Math.Floor(hf);
                double f = hf - i;
                double pv = value * (1 - saturation);
                double qv = value * (1 - saturation * f);
                double tv = value * (1 - saturation * (1 - f));
                switch (i)
                {

                    // Red is the dominant color

                    case 0:
                        R = value;
                        G = tv;
                        B = pv;
                        break;

                    // Green is the dominant color

                    case 1:
                        R = qv;
                        G = value;
                        B = pv;
                        break;
                    case 2:
                        R = pv;
                        G = value;
                        B = tv;
                        break;

                    // Blue is the dominant color

                    case 3:
                        R = pv;
                        G = qv;
                        B = value;
                        break;
                    case 4:
                        R = tv;
                        G = pv;
                        B = value;
                        break;

                    // Red is the dominant color

                    case 5:
                        R = value;
                        G = pv;
                        B = qv;
                        break;

                    // Just in case we overshoot on our math by a little, we put these here. Since its a switch it won't slow us down at all to put these here.

                    case 6:
                        R = value;
                        G = tv;
                        B = pv;
                        break;
                    case -1:
                        R = value;
                        G = pv;
                        B = qv;
                        break;

                    // The color is not defined, we should throw an error.

                    default:
                        //LFATAL("i Value error in Pixel conversion, Value is %d", i);
                        R = G = B = value; // Just pretend its black/white
                        break;
                }
            }
            r = Clamp((int)(R * 255.0));
            g = Clamp((int)(G * 255.0));
            b = Clamp((int)(B * 255.0));
        }

        /// <summary>
        /// Clamp a value to 0-255
        /// </summary>
        private static int Clamp(int i) => Math.Min(Math.Max(i, 0), 255);

        #endregion

        #region KeyboardEffects

        public static void StartKeyboardEffect(LightingConsole console, List<EffectInfo> infos, double intervalS = 1.0/44.0)
        {
            EffectManager manager = new EffectManager(infos, intervalS);
            manager.BlockThread(console);
        }

        #endregion

        #region FunctionIntensity

        public static async Task<Enums.FehlerT> FunctionIntensity(LightingConsole console, Func<short, byte> func, params short[] sks)
        {
            return await console.QueryAsync(
                new FixParUnsafe(
                    sks,
                    Enumerable
                        .Range(0, sks.Length)
                        .Select(
                            index => func((short)index))
                        .ToArray()
                    )
                );
        }

        private class FixParUnsafe : SplittableHeader
        {
            public override short Type => 20;
            private readonly IEnumerable<SKInfo> infos;
            public override IEnumerable<byte[]> GetData(LightingConsole console)
            {
                return Make(
                    infos.ToArray(),
                    200,
                    CountShort,
                    (buf, _) => buf.Write((short)console.Bedienstelle).Write((short)FixPar.ValueType.ABS).Write((short)Enums.FixParDst.Current),
                    (par, buf) =>
                    {
                        buf
                            .Write(par.num)
                            .WriteShort(0)
                            .Write((short) (par.val << 8));
                    }
                );
            }

            public FixParUnsafe(IReadOnlyList<short> sks, IReadOnlyList<byte> values)
            {
                infos = Enumerable.Range(0, sks.Count)
                    .Select(index => new SKInfo() {num = sks[index], val = values[index]});
            }
            
            private struct SKInfo
            {
                public short num;
                public byte val;
            }
        }

        #endregion
    }
}