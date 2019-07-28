using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace SharpServer
{
    public static class Extensions
    {
        public static void Write<T>(this byte[] dst, int offset, T value) where T : struct
        {
            Unsafe.As<byte, T>(ref dst[offset]) = value;
        }

        public static void Write(this byte[] dst, int offset, byte[] value)
        {
            Unsafe.CopyBlock(ref dst[offset], ref value[0], (uint)value.Length);
        }

        public static T Read<T>(this byte[] src, int offset) where T : struct
        {
            return Unsafe.As<byte, T>(ref src[offset]);
        }

        public static void Read(this byte[] src, int offset, byte[] value)
        {
            Unsafe.CopyBlock(ref value[0], ref src[offset], (uint)value.Length);
        }
    }
}
