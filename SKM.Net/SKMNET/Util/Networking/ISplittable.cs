using SKMNET.Client;
using System.Collections.Generic;

namespace SKMNET.Util
{
    public interface ISplittable
    {
        List<byte[]> GetData(LightingConsole console);
    }
}
