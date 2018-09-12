namespace SKMNET.Client.Rendering
{
    public struct Color
    {
        public readonly byte Red;
        public readonly byte Green;
        public readonly byte Blue;

        public Color(byte red, byte green, byte blue)
        {
            this.Red = red;
            this.Green = green;
            this.Blue = blue;
        }
    }
}
