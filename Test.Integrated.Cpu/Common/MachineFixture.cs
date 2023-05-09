using Cpu.Execution;
using Cpu.Extensions;
using Cpu.Flags;
using Cpu.Instructions;
using Cpu.Memory;
using Cpu.Opcodes;
using Cpu.Registers;
using Cpu.States;
using Microsoft.Extensions.Logging;
using System.Globalization;
using Test.Integrated.Cpu.Files;

namespace Test.Integrated.Cpu.Common;

public sealed record MachineFixture : IDisposable, IAsyncDisposable
{
    #region Properties
    public Machine Subject { get; }

    private ILoggerFactory LogFactory { get; }

    private bool IsDisposed { get; set; }
    #endregion

    #region Constructors
    public MachineFixture()
    {
        this.LogFactory = BuildLogFactory();

        var opcodes = LoadOpcodes() as IEnumerable<IOpcodeInformation>;
        var instructions = LoadInstructions() as IEnumerable<IInstruction>;

        var machineLogger = this.LogFactory.CreateLogger<Machine>();
        var memoryLogger = this.LogFactory.CreateLogger<MemoryManager>();

        var flagManager = new FlagManager();
        var registerManager = new RegisterManager();

        var memoryManager = new MemoryManager(memoryLogger, registerManager);
        var stackManager = new StackManager(memoryManager, registerManager);

        var state = new CpuState(flagManager, stackManager, memoryManager, registerManager);

        var decoder = new Decoder(opcodes, instructions);
        this.Subject = new Machine(machineLogger, state, decoder);
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

            this.LogFactory.Dispose();
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

    private static IEnumerable<IOpcodeInformation?> LoadOpcodes()
    {
        var loader = new OpcodeLoader();
        loader.LoadAsync().Wait();

        return loader.Opcodes;
    }

    private static IEnumerable<IInstruction?> LoadInstructions()
    {
        var instructionType = typeof(IInstruction);

        return AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => instructionType.IsAssignableFrom(t)
                                  && !t.IsInterface
                                  && !t.IsAbstract)
            .Select(t => Activator.CreateInstance(t) as IInstruction)
            .ToArray();
    }

    private static ILoggerFactory BuildLogFactory()
    {
        return LoggerFactory.Create(builder =>
        {
            _ = builder
                .AddFilter("Microsoft", LogLevel.Warning)
                .AddFilter("System", LogLevel.Warning)
                .AddFilter("Cpu", LogLevel.Debug);

            _ = builder.AddSimpleConsole(options =>
              {
                  options.SingleLine = true;
                  options.IncludeScopes = true;
                  options.UseUtcTimestamp = true;

                  options.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
              });
        });
    }
}
