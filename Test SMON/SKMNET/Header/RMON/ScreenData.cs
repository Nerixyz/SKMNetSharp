using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Header
{
    /// <summary>
    /// SKMON_SCREEN_DATA - Bildschirmdaten.
    /// Es werden die geänderten Bildschirmdaten gesendet.
    /// Dabei handelt es sich immer nur um einen Ausschnitt
    /// aus dem Monitorbild. Die Daten werden zeichenweise,
    /// von oben nach und und von links nach rechts übertragen.
    /// </summary>
    class ScreenData : Header
    {

        public override int HeaderLength => 4;

        /// <summary>
        /// Maximale Anzahl von Bildschirmdaten (legacy)
        /// </summary>
        public static readonly int MAX_DATA = 733;
        /// <summary>
        /// Position im Bildschirm. Links oben entspricht Position 0.
        /// </summary>
        public ushort start;
        /// <summary>
        /// Anzahl der folgenden Bildschirm Daten length. (legacy)
        /// </summary>
        public ushort count;
        /// <summary>
        /// Bildschirmdaten (Bit 15..8 Attribut, Bit 7..0 Zeichen.)
        /// </summary>
        public ushort[] data;

        public override byte[] ParseData()
        {
            List<byte> bytes = new List<byte>(HeaderLength + data.Length * 2);
            bytes.AddRange(BitConverter.GetBytes(start));
            bytes.AddRange(BitConverter.GetBytes(count));
            foreach(ushort us in data)
            {
                bytes.AddRange(BitConverter.GetBytes(us));
            }
            return bytes.ToArray();
        }

        public override Header ParseHeader(byte[] data)
        {
            this.start = BitConverter.ToUInt16(data, 0);
            this.count = BitConverter.ToUInt16(data, 2);
            this.data = new ushort[count * 2];
            for(int i = 0; i < count; i++)
            {
                this.data[i] = data[i * 2 + 4];
            }
            return this;
        }
    }
}
