using System.Collections.Generic;
using SKMNET.Client;

namespace SKMNET.Util.Networking
{
    public interface ISplittable
    {
        IEnumerable<byte[]> GetData(LightingConsole console);
    }
}
