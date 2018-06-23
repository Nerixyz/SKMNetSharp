using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Util
{
    class ByteArrayParser
    {
        List<byte> bytes;

        public ByteArrayParser()
        {
            bytes = new List<byte>();
        }

        public ByteArrayParser(int capacity)
        {
            bytes = new List<byte>();
        }

        public ByteArrayParser(IEnumerable<byte> collection)
        {
            bytes = new List<byte>(collection);
        }

        public ByteArrayParser Add(byte b)
        {
            bytes.Add(b);
            return this;
        }

        public ByteArrayParser Add(int b)
        {
            bytes.AddRange(BitConverter.GetBytes(b).Reverse());
            return this;
        }

        public ByteArrayParser Add(ushort u)
        {
            bytes.AddRange(BitConverter.GetBytes(u).Reverse());
            return this;
        }

        public ByteArrayParser Add(uint u)
        {
            bytes.AddRange(BitConverter.GetBytes(u).Reverse());
            return this;
        }

        public ByteArrayParser Add(short s)
        {
            bytes.AddRange(BitConverter.GetBytes(s).Reverse());
            return this;
        }
        public ByteArrayParser Add(string s)
        {
            bytes.AddRange(Encoding.ASCII.GetBytes(s));
            return this;
        }

        public ByteArrayParser Add(byte[] arr)
        {
            bytes.AddRange(arr);
            return this;
        }

        public ByteArrayParser Add(ushort[] arr)
        {
            foreach(ushort u in arr)
            {
                bytes.AddRange(BitConverter.GetBytes(u).Reverse());
            }
            return this;
        }

        public ByteArrayParser Add(short[] arr)
        {
            foreach (short u in arr)
            {
                bytes.AddRange(BitConverter.GetBytes(u).Reverse());
            }
            return this;
        }

        public byte[] GetArray()
        {
            return bytes.ToArray();
        }

        public List<byte> List()
        {
            return bytes;
        }
    }
}
