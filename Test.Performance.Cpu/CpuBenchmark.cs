using BenchmarkDotNet.Attributes;
using Cpu.DependencyInjection;
using Cpu.Execution;
using Cpu.States;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using Test.Performance.Cpu.Files;

namespace Test.Performance.Cpu;

[GcServer(true)]
[MemoryDiagnoser]
[JsonExporterAttribute.FullCompressed]
[MinColumn, MaxColumn, MeanColumn, MedianColumn]
[SimpleJob(
    invocationCount: ExpectedIterations,
    iterationCount: ExpectedIterations)]
public class CpuBenchmark
{
    #region Constants
    /// <summary>
    /// Expected iterations for the benchmark
    /// </summary>
    public const int ExpectedIterations = 100;

    private const string ProgramName = "count_until";
    #endregion

    #region Properties
    private ReadOnlyMemory<byte> ExecutingProgram { get; set; }

    private IServiceProvider ServiceProvider { get; set; }

    private IMachine? Machine { get; set; }
    #endregion

    [GlobalSetup(Targets = new[] { nameof(ExecuteProgram) })]
    public void GlobalSetup()
    {
        var collection = new ServiceCollection();
        this.ServiceProvider = collection
            .AddLogging()
            .Add6502Cpu()
            .BuildServiceProvider();

        this.ExecutingProgram = ReadProgram(ProgramName);
    }

    [GlobalCleanup(Targets = new[] { nameof(ExecuteProgram) })]
    public void GlobalCleanup()
    {
        this.ExecutingProgram = Array.Empty<byte>();
        this.ServiceProvider = null;
    }

    [IterationSetup]
    public void IterationSetup()
    {
        this.Machine = this.ServiceProvider.GetRequiredService<IMachine>();
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        this.Machine = null;
    }

    [Benchmark]
    public void ExecuteProgram()
    {
        this.Machine.Load(this.ExecutingProgram);

        bool cycling;

        do
        {
            cycling = this.Machine.Cycle();
        } while (cycling);
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
