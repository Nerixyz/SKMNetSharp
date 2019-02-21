using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SKMNET.Client.Rendering
{
    [Serializable]
    public class Screen
    {
        public MBlock[] Data { get; private set; }
        public bool Init { get; private set; }
        public byte Num { get; private set; }

        const int HORIZONTAL_LINES = 37;
        const int VERTICAL_LINES = 72;

        [NonSerialized]
        private readonly ScreenManager manager;

        public Screen(ScreenManager manager, byte num)
        {
            this.manager = manager;
            this.Num = num;
            this.Init = true;
            Setup();
        }

        public void HandleData(ushort[] data, ushort start, ushort count)
        {
            for(int i = 0; i < count; i++)
            {
                ushort point = data[i];
                MBlock block = new MBlock((byte)((point & 0xff00) >> 8), (char) (point & 0x00ff));
                Data[start + i] = block;
            }
        } 

        public MBlock GetBlock(int x, int y)
        {
            return Data[(x) * VERTICAL_LINES + y];
        }

        private void Setup()
        {
            Data = new MBlock[VERTICAL_LINES * HORIZONTAL_LINES];
        }

        [Serializable]
        public struct MBlock
        {
            public readonly byte farbe;
            public readonly char text;

            public MBlock(byte farbe, char text)
            {
                this.farbe = farbe;
                this.text = text;
            }
        }
    }
}
