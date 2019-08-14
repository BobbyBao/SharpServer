using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Benchmark
{
    public unsafe class MemoryTest
    {

        const int COUNT = 1;
        int size = 1;

        byte[][] bytes = new byte[COUNT][];

        byte*[] bytePtrs = new byte*[COUNT];

        [BenchmarkCategory("Alloc New"), Benchmark]
        public void TestNew()
        {
            for (int i = 0; i < COUNT; i++)
            {
                bytes[i] = new byte[size];
            }
        }

        [BenchmarkCategory("Alloc HGlobal"), Benchmark]
        public void TestAlloc()
        {
            for (int i = 0; i < COUNT; i++)
            {
                bytePtrs[i] = (byte*)Marshal.AllocHGlobal(size);
            }
        }
    }
}
