using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioAnalyze
{
    class Timer
    {
        private double lastMS;

        public double GetCurrentMS()
        {
            return TimeSpan.FromTicks(DateTime.Now.Ticks).TotalMilliseconds;
        }

        public static double GetCurrent()
        {
            return TimeSpan.FromTicks(DateTime.Now.Ticks).TotalMilliseconds;
        }

        public double GetLastMS()
        {
            return this.lastMS;
        }

        public bool HasReached(long milliseconds)
        {
            return GetElapsed() >= milliseconds;
        }

        public bool HasReached(float milliseconds)
        {
            return GetElapsed() >= milliseconds;
        }

        public bool HasReached(double milliseconds)
        {
            return GetElapsed() >= milliseconds;
        }

        public void Reset()
        {
            this.lastMS = this.GetCurrentMS();
        }

        public void SetLastMS(double currentMS)
        {
            this.lastMS = currentMS;
        }

        public double GetElapsed()
        {
            return this.GetCurrentMS() - this.lastMS;
        }
    }
}
