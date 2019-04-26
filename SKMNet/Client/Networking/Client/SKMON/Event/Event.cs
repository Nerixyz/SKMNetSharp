using System;

namespace SKMNET.Client.Networking.Client
{
    public abstract class Event : CPacket
    {
        public override short Type => 14;

        public override byte[] GetDataToSend(LightingConsole console)
        {
            //no BdstNo ?!
            return new ByteBuffer()
                .Write((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds) // Time
                .Write((short)1) // Count
                .Write((short)1) // Flags
                .Write(GetEventInteger(console)) // Event Int
                .ToArray();
        }

        public abstract int GetEventInteger(LightingConsole console);


        public static CPacket Chain(params Event[] events)
        {
            return new ChainedEvents(events);
        }

        private class ChainedEvents : CPacket
        {
            public override short Type => 14;

            private readonly Event[] events;

            public override byte[] GetDataToSend(LightingConsole console)
            {
                ByteBuffer buffer = new ByteBuffer()
                    .Write((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds)
                    .Write((short)events.Length).Write((short)1); // Flags (context)

                foreach(Event e in events)
                {
                    buffer.Write(e.GetEventInteger(console));
                }
                return buffer.ToArray();
            }

            public ChainedEvents(Event[] events)
            {
                this.events = events;
            }
        }
    }
}
