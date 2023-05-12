using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace Test.Performance.Cpu;

public static class Program
{
    public static void Main(string[] args)
    {
        _ = BenchmarkRunner
            .Run<CpuBenchmark>(DefaultConfig.Instance, args);
    }
}