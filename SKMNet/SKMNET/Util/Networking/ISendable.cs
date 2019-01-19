using SKMNET.Client;

namespace SKMNET.Util
{
    public interface ISendable
    {
        byte[] GetDataToSend(LightingConsole console);
    }
}
