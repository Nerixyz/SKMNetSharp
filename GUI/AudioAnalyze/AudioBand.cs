using NAudio.Dsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Util.MathUtils;

namespace AudioAnalyze
{
    public class AudioBand
    {
        private float _value;
        public float Value {
            get {
                if (FadeMode == Fade.Fade)
                {
                    return _value * GetMultiplier();
                }
                else
                {
                    return _value;
                }
            }
            set {
                if (FadeMode == Fade.Fade)
                {
                    if (value > _value)
                    {
                        timer.Reset();
                        _value = value;
                    }
                }
                else
                {
                    _value = value;
                }
            }
        }
        public Fade FadeMode { get; set; }
        public int MIN_FREQUENCY;
        public int MAX_FREQUENCY;
        public TimeSpan fadeSpeed;

        private readonly Timer timer;

        public AudioBand(int minFrequency, int maxFrequency, Fade fade, TimeSpan speed)
        {
            timer = new Timer();
            MIN_FREQUENCY = minFrequency;
            MAX_FREQUENCY = maxFrequency;
            FadeMode = fade;
            fadeSpeed = speed;
        }

        public void OnFFT(Complex[] arr, float scale = 10, int SAMPLE_RATE = 44100)
        {
            int startIndex = (int) ((MIN_FREQUENCY / (float)SAMPLE_RATE) * arr.Length);
            int endIndex = (int)Math.Round(((MAX_FREQUENCY / (float)SAMPLE_RATE) * arr.Length));

            float high = float.MinValue;
            for(int i = startIndex; i < endIndex; i++)
            {
                float var = (float)GetFrequencyIntensity(arr[i].X, arr[i].Y);
                if (var > high)
                    high = var;
            }
            Value = high * scale;
        }

        private float GetMultiplier()
        {
            return Clamp(1f - ( (float)timer.GetElapsed() / (float)fadeSpeed.TotalMilliseconds), 0f, 1f);
        }

        public enum Fade
        {
            Fade,
            Snap
        }

        private static double GetFrequencyIntensity(double re, double im)
        {
            return Math.Sqrt((re * re) + (im * im));
        }
    }
}
