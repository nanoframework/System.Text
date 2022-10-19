using nanoFramework.Benchmark;
using System;
using System.Diagnostics;
using System.Threading;

namespace nanoFramework.System.Text.Benchmark
{
    public class Program
    {
        public static void Main()
        {
            BenchmarkRunner.Run(typeof(IAssemblyHandler).Assembly);
            Thread.Sleep(Timeout.Infinite);
        }
    }

}
