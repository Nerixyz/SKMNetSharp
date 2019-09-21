using SKMNET.Client.Networking.Server.T98;

namespace SKMNET.Client.Events
{
    public class SkAttrChangedEventArgs : SkmEventArgs<SKRegAttr>
    {
        public SkAttrChangedEventArgs(SKRegAttr packet) : base(packet)
        {
        }

        /// <summary>
        /// Returns the value for a given SK
        /// </summary>
        /// <param name="i">SK Number</param>
        public SkAttrInfo this[int i] => new SkAttrInfo(Packet.Data[Packet.Start + i]);

        public bool IsUpdated(int skNum) => Packet.Start <= skNum && Packet.Start + Packet.Count > skNum;

        public struct SkAttrInfo
        {
            public byte Attrib { get; }
            public bool Anwahl => (Attrib & 0x01) != 0;
            public bool SkuErr => (Attrib & 0x02) != 0;
            public bool Maske => (Attrib & 0x04) != 0;
            public bool Bet => (Attrib & 0x08) != 0;
            public bool Modified => (Attrib & 0x10) != 0;
            public bool Sperr => (Attrib & 0x20) != 0;
            public bool Heller => (Attrib & 0x40) != 0;
            public bool Dunkler => (Attrib & 0x80) != 0;

            public SkAttrInfo(byte attrib) => Attrib = attrib;
        }
    }
}