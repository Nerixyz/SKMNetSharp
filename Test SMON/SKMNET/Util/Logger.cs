using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET
{
    class Logger
    {
        public static void Log(object toLog)
        {
#if DEBUG
            Console.WriteLine(toLog);
#endif
        }
    }
}
