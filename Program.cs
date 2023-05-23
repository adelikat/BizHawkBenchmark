// Assumes BizHawk is cloned in a folder next to this one

using BenchmarkDotNet.Running;

namespace BizHawkBenchmark
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<MovieRunner>();
        }
    }
}



