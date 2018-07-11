using System;
using System.IO;
using System.Text;

namespace SKMNET
{
    public class ByteBuffer
    {
        MemoryStream memory;

        public ByteBuffer()
        {
            memory = new MemoryStream();
        }

        public ByteBuffer(byte[] buffer, bool writable = false)
        {
            memory = new MemoryStream(buffer, writable);
        }

        public uint ReadUInt()
        {
            byte[] buffer = new byte[4];
            memory.Read(buffer, (int)memory.Position, 4);
            return ByteUtils.ToUInt(buffer, 0);
        }

        public short ReadShort()
        {
            byte[] buffer = new byte[2];
            memory.Read(buffer, (int)memory.Position, 2);
            return ByteUtils.ToShort(buffer, 0);
        }

        public ushort ReadUShort()
        {
            byte[] buffer = new byte[2];
            memory.Read(buffer, (int)memory.Position, 2);
            return ByteUtils.ToUShort(buffer, 0);
        }

        public byte ReadByte()
        {
            byte[] buffer = new byte[1];
            memory.Read(buffer, (int)memory.Position, 1);
            return buffer[0];
        }

        public byte[] ReadByteArray(int length)
        {
            byte[] buffer = new byte[length];
            memory.Read(buffer, (int)memory.Position, length);
            return buffer;
        }

        public string ReadString(int length)
        {
            if (length == 0)
                return string.Empty;
            byte[] buffer = new byte[length];
            memory.Read(buffer, (int)memory.Position, length);
            return ByteUtils.ToString(buffer, 0, length);
        }

        public void Write(ushort value)
        {
            byte[] data = BitConverter.GetBytes(value);
            Array.Reverse(data);
            memory.Write(data, (int)memory.Position, 2);
        }

        public void Write(short value)
        {
            byte[] data = BitConverter.GetBytes(value);
            Array.Reverse(data);
            memory.Write(data, (int)memory.Position, 2);
        }

        public void Write(int value)
        {
            byte[] data = BitConverter.GetBytes(value);
            Array.Reverse(data);
            memory.Write(data, (int)memory.Position, 4);
        }

        public void Write(uint value)
        {
            byte[] data = BitConverter.GetBytes(value);
            Array.Reverse(data);
            memory.Write(data, (int)memory.Position, 4);
        }

        public void Write(long value)
        {
            byte[] data = BitConverter.GetBytes(value);
            Array.Reverse(data);
            memory.Write(data, (int)memory.Position, 8);
        }

        public void Write(byte value)
        {
            memory.Write(new byte[] { value }, (int)memory.Position, 1);
        }

        public void Write(ulong value)
        {
            byte[] data = BitConverter.GetBytes(value);
            Array.Reverse(data);
            memory.Write(data, (int)memory.Position, 8);
        }

        public void Write(ushort[] arr)
        {
            foreach (ushort value in arr)
            {
                byte[] data = BitConverter.GetBytes(value);
                Array.Reverse(data);
                memory.Write(data, (int)memory.Position, 2);
            }
        }

        public void Write(short[] arr)
        {
            foreach (short value in arr)
            {
                byte[] data = BitConverter.GetBytes(value);
                Array.Reverse(data);
                memory.Write(data, (int)memory.Position, 2);
            }
        }

        public void Write(byte[] arr)
        {
            memory.Write(arr, (int)memory.Position, arr.Length);
        }

        public int Length { get {
                return (int) memory.Length;
            } }

        public void Forward(int forward)
        {
            memory.Position += forward;
        }

        public byte[] ToArray()
        {
            return memory.ToArray();
        }
    }
}
