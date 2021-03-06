﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SKMNET
{
    public class ByteBuffer
    {
        private readonly MemoryStream memory;

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
            memory.Read(buffer, 0, 4);
            return ByteUtils.ToUInt(buffer, 0);
        }

        public short ReadShort()
        {
            byte[] buffer = new byte[2];
            memory.Read(buffer, 0, 2);
            return ByteUtils.ToShort(buffer, 0);
        }

        public ushort ReadUShort()
        {
            byte[] buffer = new byte[2];
            memory.Read(buffer, 0, 2);
            return ByteUtils.ToUShort(buffer, 0);
        }

        public byte ReadByte()
        {
            byte[] buffer = new byte[1];
            memory.Read(buffer, 0, 1);
            return buffer[0];
        }

        public byte[] ReadByteArray(int length)
        {
            byte[] buffer = new byte[length];
            memory.Read(buffer, 0, length);
            return buffer;
        }

        public string ReadString(int length)
        {
            if (length == 0)
                return string.Empty;
            byte[] buffer = new byte[length];
            memory.Read(buffer, 0, length);
            return ByteUtils.ToString(buffer, 0, length);
        }

        public ByteBuffer Write(ushort value) => WriteUshort(value);

        public ByteBuffer WriteUshort(ushort value)
        {
            byte[] data = BitConverter.GetBytes(value);
            memory.Write(data.TransformToBigEndian(), 0, 2);
            return this;
        }

        public ByteBuffer Write(short value) => WriteShort(value);

        public ByteBuffer WriteShort(short value)
        {
            byte[] data = BitConverter.GetBytes(value);
            memory.Write(data.TransformToBigEndian(), 0, 2);
            return this;
        }

        public ByteBuffer Write(int value)
        {
            byte[] data = BitConverter.GetBytes(value);
            memory.Write(data.TransformToBigEndian(), 0, 4);
            return this;
        }

        public ByteBuffer Write(uint value)
        {
            byte[] data = BitConverter.GetBytes(value);
            memory.Write(data.TransformToBigEndian(), 0, 4);
            return this;
        }

        public ByteBuffer Write(long value)
        {
            byte[] data = BitConverter.GetBytes(value);
            memory.Write(data.TransformToBigEndian(), 0, 8);
            return this;
        }

        public ByteBuffer Write(byte value)
        {
            memory.WriteByte(value);
            return this;
        }

        public ByteBuffer Write(string value, int length)
        {
            memory.Write(Encoding.ASCII.GetBytes(value), 0, Math.Min(length, value.Length));
            if (Math.Min(length, value.Length) >= length) return this;
            
            byte[] fill = new byte[length - value.Length];
            for(int i = 0; i < fill.Length; i++)
            {
                fill[i] = 0;
            }
            memory.Write(fill, 0, fill.Length);
            return this;
        }

        public ByteBuffer Write(ulong value)
        {
            byte[] data = BitConverter.GetBytes(value);
            memory.Write(data.TransformToBigEndian(), 0, 8);
            return this;
        }

        public ByteBuffer Write(IEnumerable<ushort> arr)
        {
            foreach (ushort value in arr)
            {
                byte[] data = BitConverter.GetBytes(value);
                memory.Write(data.TransformToBigEndian(), 0, 2);
            }
            return this;
        }

        public ByteBuffer Write(IEnumerable<short> arr)
        {
            foreach (short value in arr)
            {
                byte[] data = BitConverter.GetBytes(value);
                memory.Write(data.TransformToBigEndian(), 0, 2);
            }
            return this;
        }

        public ByteBuffer Write(byte[] arr)
        {
            memory.Write(arr, 0, arr.Length);
            return this;
        }

        public int Length => (int) memory.Length;

        public void Forward(int forward) => memory.Position += forward;

        public byte[] ToArray() => memory.ToArray();

        public long Position => memory.Position;
    }

    public static class ByteArrayExtensions
    {
        public static byte[] TransformToBigEndian(this byte[] instance)
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(instance);

            return instance;
        }

        public static byte[] TransformBigToLocalEndian(this byte[] instance)
        {
            if(BitConverter.IsLittleEndian)
                Array.Reverse(instance);

            return instance;
        }
    }
}
