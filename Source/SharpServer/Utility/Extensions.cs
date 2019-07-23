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

        public static void Read<T>(this byte[] dst, int offset, out T value) where T : struct
        {
            value = Unsafe.As<byte, T>(ref dst[offset]);
        }
    }
}
