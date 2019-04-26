namespace SKMNET.Client.Networking.Client
{
    /// <summary>
    /// SKMON an/abmelden
    /// </summary>
    public class SKMSync : CPacket
    {
        public override short Type => 13;

        private readonly Action action;
        private readonly int flags;
        private readonly byte skmType;

        public override byte[] GetDataToSend(LightingConsole console)
        {
            byte[] steckbrief = new byte[10];
            for(int i = 0; i < 10; i++)
            {
                steckbrief[i] = (flags & (1 << i)) != 0 ? (byte)1 : (byte)0;
            }
            steckbrief[0] = skmType;
            return 
                action == Action.BEGIN ? new ByteBuffer().Write((short)action).WriteShort(console.BdstNo).WriteShort(10).Write(steckbrief).ToArray() 
                    : new ByteBuffer().Write((short)action).WriteShort(console.BdstNo).WriteShort(0)/*count = 0 -> no array needed { .Write(steckbrief) } */.ToArray();
        }

        public SKMSync(Action action)
        {
            this.action = action;
            flags = 0;
        }

        public SKMSync(int flags)
        {
            action = Action.BEGIN;
            this.flags = flags;
        }

        public SKMSync(SKMSteckbrief steckbrief, byte skmType)
        {
            action = Action.BEGIN;
            System.Reflection.FieldInfo[] fields = steckbrief.GetType().GetFields();
            for(int i = 0; i < fields.Length; i++)
            {
                flags |= ((bool)fields[i].GetValue(steckbrief) ? 1 : 0) << (i+1);
            }
            this.skmType = skmType;
        }

        public enum Action
        {
            BEGIN = 1,
            END,
            PING
        }
    }

    // nicht alles wird verwendet?!
    // auch wenn Bed = false werden Tastendaten gesendet
    public struct SKMSteckbrief
    {
        public bool Bedientasten;
        public bool BefMeldZeile;
        public bool FuncKeys;
        public bool LKI;
        public bool BlockInfo;
        public bool AZ_Zeilen;
        public bool ExtKeys;
        public bool AktInfo;
        public bool Steller;
    }
}
