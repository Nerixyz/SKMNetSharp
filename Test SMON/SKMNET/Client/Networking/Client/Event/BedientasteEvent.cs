
namespace SKMNET.Client.Networking.Client
{
    public class BedientasteEvent : Event
    {
        readonly byte bdst;
        readonly byte flanke;
        readonly byte btast;

        // TODO make Enum & TastenImpl
        public BedientasteEvent(byte btast, bool T_STEIGEND, byte bdst = 0)
        {
            this.bdst = bdst;
            this.btast = btast;
            this.flanke = (byte)( T_STEIGEND ? 1 : 0);
        }

        public override int GetEventInteger()
        {
            return 0x04000000 | (bdst << 16) | (flanke << 8) | btast;
        }
    }
}
