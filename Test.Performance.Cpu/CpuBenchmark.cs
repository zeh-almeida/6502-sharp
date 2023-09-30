using BenchmarkDotNet.Attributes;
using Cpu.DependencyInjection;
using Cpu.Execution;
using Cpu.States;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using Test.Performance.Cpu.Files;

namespace Test.Performance.Cpu;

[MemoryDiagnoser]
[JsonExporterAttribute.FullCompressed]
[MinColumn, MaxColumn, MeanColumn, MedianColumn]
[HideColumns("Error", "StdDev", "Median", "RatioSD")]
public class CpuBenchmark
{
    #region Constants
    /// <summary>
    /// Expected iterations for the benchmark
    /// </summary>
    public const int ExpectedIterations = 1_000;

    private const string ProgramName = "count_until";
    #endregion

    #region Properties
    private ReadOnlyMemory<byte> ExecutingProgram { get; set; }

    private IMachine? Machine { get; set; }
    #endregion

    [GlobalSetup]
    public void GlobalSetup()
    {
        var collection = new ServiceCollection();
        var serviceProvider = collection
            .AddLogging()
            .Add6502Cpu()
            .BuildServiceProvider();

        this.ExecutingProgram = ReadProgram(ProgramName);
        this.Machine = serviceProvider.GetRequiredService<IMachine>();
    }

    [GlobalCleanup]
    public void GlobalCleanup()
    {
        this.ExecutingProgram = Array.Empty<byte>();
        this.Machine = null;
    }

    [IterationSetup]
    public void IterationSetup()
    {
        this.Machine?.Load(this.ExecutingProgram);
    }

    [Benchmark]
    public void ExecuteProgram()
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        do
        {
            _ = this.Machine.Cycle();
        } while (this.Machine.HasCycled);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }

    private static ReadOnlyMemory<byte> ReadProgram(string programName)
    {
        ArgumentException.ThrowIfNullOrEmpty(programName, nameof(programName));

        if (Resources.ResourceManager.GetObject(programName, CultureInfo.CurrentCulture) is not byte[] program)
        {
            throw new ArgumentException("Program not found", programName);
        }

        var state = new byte[ICpuState.Length];

        program.CopyTo(state, ICpuState.MemoryStateOffset);

        state[ICpuState.MemoryStateOffset + 0xFFFE] = 0xFF;
        state[ICpuState.MemoryStateOffset + 0xFFFF] = 0xFF;

        return state;
    }
}
