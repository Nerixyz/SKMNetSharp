using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Server
{
    /// <summary>
    /// SKMON_SCREEN_DATA - Bildschirmdaten.
    /// Es werden die geänderten Bildschirmdaten gesendet.
    /// Dabei handelt es sich immer nur um einen Ausschnitt
    /// aus dem Monitorbild. Die Daten werden zeichenweise,
    /// von oben nach und und von links nach rechts übertragen.
    /// </summary>
    class ScreenData : SPacket
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

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            this.start = buffer.ReadUShort();
            this.count = buffer.ReadUShort();
            this.data = new ushort[count];
            for (int i = 0; i < count; i++)
            {
                this.data[i] = buffer.ReadUShort();
            }
            return this;
        }
    }
}
