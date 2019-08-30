using SKMNET.Client;

namespace SKMNET.Util.Networking
{
    public interface ISendable
    {
        byte[] GetDataToSend(LightingConsole console);
    }
}
