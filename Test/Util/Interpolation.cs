using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Util
{
    public static class Interpolation
    {
        public static double Linear(double from, double to, double progress)
        {
            return (to - from) * progress + from;
        }
    }
}
