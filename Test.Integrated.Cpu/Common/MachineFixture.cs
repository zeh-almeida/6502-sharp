using Cpu.Execution;
using Cpu.Extensions;
using Cpu.States;
using System.Globalization;
using Test.Integrated.Cpu.Files;

namespace Test.Integrated.Cpu.Common;

public sealed record MachineFixture : IDisposable, IAsyncDisposable
{
    #region Properties
    public IMachine Subject { get; }

    private bool IsDisposed { get; set; }
    #endregion

    #region Constructors
    public MachineFixture(IMachine machine)
    {
        this.Subject = machine;
    }
    #endregion

    public byte[] Compute(ReadOnlyMemory<byte> data)
    {
        this.Subject.Load(data);

        bool cycling;

        do
        {
            cycling = this.Subject.Cycle();
        } while (cycling);

        return this.Subject
            .Save()
            .ToArray();
    }

    public byte[] Compute(ReadOnlyMemory<byte> data, Action<ICpuState> cycleAction)
    {
        this.Subject.Load(data);

        bool cycling;

        do
        {
            cycling = this.Subject.Cycle(cycleAction);
        } while (cycling);

        return this.Subject
            .Save()
            .ToArray();
    }

    public byte[] Compute(string programName)
    {
        var program = BuildProgramStream(programName);
        return this.Compute(program);
    }

    public byte[] Compute(string programName, Action<ICpuState> cycleAction)
    {
        var program = BuildProgramStream(programName);
        return this.Compute(program, cycleAction);
    }

    public byte[] Compute(string programName, ushort offset, Action<ICpuState> cycleAction)
    {
        var program = BuildProgramStream(programName, offset);
        return this.Compute(program, cycleAction);
    }

    public void Dispose()
    {
        if (!this.IsDisposed)
        {
            this.IsDisposed = true;
            GC.SuppressFinalize(this);
        }
    }

    public ValueTask DisposeAsync()
    {
        return new ValueTask(Task.Run(this.Dispose));
    }

    private static ReadOnlyMemory<byte> BuildProgramStream(string programName)
    {
        if (Resources.ResourceManager.GetObject(programName, CultureInfo.CurrentCulture) is not byte[] program)
        {
            throw new ArgumentException("Program not found", nameof(programName));
        }

        var state = new byte[ICpuState.Length];

        program.CopyTo(state, ICpuState.MemoryStateOffset);

        state[ICpuState.MemoryStateOffset + 0xFFFE] = 0xFF;
        state[ICpuState.MemoryStateOffset + 0xFFFF] = 0xFF;

        return state;
    }

    private static ReadOnlyMemory<byte> BuildProgramStream(string programName, ushort offset)
    {
        if (Resources.ResourceManager.GetObject(programName, CultureInfo.CurrentCulture) is not byte[] program)
        {
            throw new ArgumentException("Program not found", nameof(programName));
        }

        var state = new byte[ICpuState.Length];

        (var programLsb, var programMsb) = offset.SignificantBits();
        state[ICpuState.RegisterOffset + 0] = programLsb;
        state[ICpuState.RegisterOffset + 1] = programMsb;

        program.CopyTo(state, ICpuState.MemoryStateOffset + offset);

        return state;
    }
}
