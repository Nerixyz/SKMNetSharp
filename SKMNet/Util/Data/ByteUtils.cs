using System.Text;

namespace SKMNET
{
    public static class ByteUtils
    {
        public static string ArrayToString(byte[] array)
        {
            StringBuilder hex = new StringBuilder(array.Length * 2);
            foreach (byte b in array)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static byte[] ToByte(short s) => new byte[] { (byte)(s >> 8), (byte)(s & 255) };

        public static byte[] ToByte(ushort s) => new byte[] { (byte)(s >> 8), (byte)(s & 255) };

        public static ushort ToUShort(byte[] arr, int start) => (ushort)(arr[start] << 8 | arr[start + 1]);

        public static short ToShort(byte[] arr, int start) => (short)(arr[start] << 8 | arr[start + 1]);

        public static uint ToUInt(byte[] arr, int start) => (uint)(arr[start] << 24 | arr[start + 1] << 16 | arr[start + 2] << 8 | arr[start + 3]);

        public static string ToString(byte[] arr, int index, int count)
        {
            string stage0 = Encoding.ASCII.GetString(arr, index, count);
            return stage0.Replace("\u0000", string.Empty);
        }

    }
}
