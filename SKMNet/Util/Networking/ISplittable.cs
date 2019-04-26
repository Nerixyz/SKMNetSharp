using SKMNET.Client;
using System.Collections.Generic;

namespace SKMNET.Util
{
    public interface ISplittable
    {
        IEnumerable<byte[]> GetData(LightingConsole console);
    }
}
