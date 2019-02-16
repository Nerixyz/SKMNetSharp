using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Logging
{
    public interface ILogger
    {
        void Log(object toLog);
    }
}
