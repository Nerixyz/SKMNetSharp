using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET
{
    class ByteUtils
    {
        public static string ArrayToString(byte[] array)
        {
            StringBuilder hex = new StringBuilder(array.Length * 2);
            foreach (byte b in array)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static byte[] ToByte(short s)
        {
            return new byte[] { (byte)(s >> 8) , (byte)(s & 255) };
        }

        public static byte[] ToByte(ushort s)
        {
            return new byte[] { (byte)(s >> 8), (byte)(s & 255) };
        }

        public static ushort ToUShort(byte[] arr, int start)
        {
            return (ushort)(arr[start] << 8 | arr[start + 1]);
        }

        public static short ToShort(byte[] arr, int start)
        {
            return (short)(arr[start] << 8 | arr[start + 1]);
        }
    }
}
