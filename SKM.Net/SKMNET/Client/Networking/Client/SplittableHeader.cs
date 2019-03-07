using SKMNET.Util;
using System;
using System.Collections.Generic;

namespace SKMNET.Client.Networking.Client
{
    public abstract class SplittableHeader : ISplittable
    {
        public abstract short Type { get; }

        public abstract List<byte[]> GetData(LightingConsole console);

        protected static List<byte[]> Make<T>(T[] entries, int maxEntries, WriteCount writeCount, Action<ByteBuffer, int> addHeader_func, Action<T, ByteBuffer> addData_func)
        {
            List<byte[]> allPackets = new List<byte[]>();

            ByteBuffer currentBuffer = new ByteBuffer();

            bool addHeader = true;
            int entriesDone = 0;
            int totalEntries = 0;
            foreach(T entry in entries)
            {
                if (addHeader)
                {
                    addHeader_func(currentBuffer, totalEntries);
                    writeCount?.Invoke(currentBuffer, maxEntries, totalEntries, entries.Length);
                    addHeader = false;
                }
                addData_func(entry, currentBuffer);

                entriesDone++;
                if(entriesDone >= maxEntries)
                {
                    entriesDone = 0;
                    addHeader = true;
                    allPackets.Add(currentBuffer.ToArray());
                    currentBuffer = new ByteBuffer();
                    totalEntries += maxEntries;
                }

            }
            allPackets.Add(currentBuffer.ToArray());
            return allPackets;

        }

        protected delegate void WriteCount(ByteBuffer buf, int maxEntries, int totalDone, int listCount);

        protected static void CountShort(ByteBuffer buf, int maxEntries, int totalDone, int listCount)
        {
            buf.WriteShort((short)Math.Min(maxEntries, listCount - totalDone));
        }

        protected static void CountUShort(ByteBuffer buf, int maxEntries, int totalDone, int listCount)
        {
            buf.WriteUshort((ushort)Math.Min(maxEntries, listCount - totalDone));
        }
    }
}
