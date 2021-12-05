using Cpu.Execution;
using Cpu.Extensions;
using Cpu.Flags;
using Cpu.Instructions;
using Cpu.Memory;
using Cpu.Registers;
using Cpu.States;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Integrated.Cpu.Files;

namespace Test.Integrated.Cpu.Common
{
    public sealed record MachineFixture : IDisposable, IAsyncDisposable
    {
        #region Constants
        public const ushort MemoryStateOffset = 7;

        public const ushort RegisterOffset = 1;

        public const int LoadDataLength = ushort.MaxValue + 1 + MemoryStateOffset;
        #endregion

        #region Properties
        public Machine Subject { get; }

        private ILoggerFactory LogFactory { get; }

        private bool IsDisposed { get; set; }
        #endregion

        #region Constructors
        public MachineFixture()
        {
            this.LogFactory = BuildLogFactory();
            var instructions = LoadInstructions();

            var machineLogger = this.LogFactory.CreateLogger<Machine>();
            var memoryLogger = this.LogFactory.CreateLogger<MemoryManager>();

            var flagManager = new FlagManager();
            var registerManager = new RegisterManager();

            var memoryManager = new MemoryManager(memoryLogger, registerManager);
            var stackManager = new StackManager(memoryManager, registerManager);

            var state = new CpuState(flagManager, stackManager, memoryManager, registerManager);
            var decoder = new Decoder(instructions);

            this.Subject = new Machine(machineLogger, state, decoder);
        }
        #endregion

        public byte[] Compute(IEnumerable<byte> data)
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

        public byte[] Compute(IEnumerable<byte> data, Action<ICpuState> cycleAction)
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
            return new ValueTask(Task.Run(() => this.Dispose()));
        }

        private static IEnumerable<byte> BuildProgramStream(string programName)
        {
            var state = new byte[MachineFixture.LoadDataLength];

            var program = Resources.ResourceManager.GetObject(programName) as byte[];
            program.CopyTo(state, MachineFixture.MemoryStateOffset);

            state[MachineFixture.MemoryStateOffset + 0xFFFE] = 0xFF;
            state[MachineFixture.MemoryStateOffset + 0xFFFF] = 0xFF;

            return state;
        }

        private static IEnumerable<byte> BuildProgramStream(string programName, ushort offset)
        {
            var state = new byte[MachineFixture.LoadDataLength];

            (var programLsb, var programMsb) = offset.SignificantBits();
            state[MachineFixture.RegisterOffset + 0] = programLsb;
            state[MachineFixture.RegisterOffset + 1] = programMsb;

            var program = Resources.ResourceManager.GetObject(programName) as byte[];
            program.CopyTo(state, MachineFixture.MemoryStateOffset + offset);

            return state;
        }

        private static IEnumerable<IInstruction> LoadInstructions()
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
}
