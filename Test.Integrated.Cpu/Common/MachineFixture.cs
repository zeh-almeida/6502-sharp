using Cpu.Execution;
using Cpu.Extensions;
using Cpu.Flags;
using Cpu.Instructions;
using Cpu.Memory;
using Cpu.Registers;
using Cpu.States;
using System;
using System.Collections.Generic;
using System.Linq;
using Test.Integrated.Cpu.Files;

namespace Test.Integrated.Cpu.Common
{
    public sealed record MachineFixture
    {
        #region Constants
        public const ushort MemoryStateOffset = 7;

        public const ushort RegisterOffset = 1;

        public const int LoadDataLength = ushort.MaxValue + 1 + MemoryStateOffset;
        #endregion

        #region Properties
        public Machine Subject { get; }
        #endregion

        #region Constructors
        public MachineFixture()
        {
            var instructions = LoadInstructions();

            var flagManager = new FlagManager();
            var registerManager = new RegisterManager();

            var memoryManager = new MemoryManager(registerManager);
            var stackManager = new StackManager(memoryManager, registerManager);

            var state = new CpuState(flagManager, stackManager, memoryManager, registerManager);
            var decoder = new Decoder(instructions);

            this.Subject = new Machine(state, decoder);
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

        public static IEnumerable<byte> BuildProgramStream(string programName, ushort offset = 0)
        {
            var state = new byte[MachineFixture.LoadDataLength];

            (var programLsb, var programMsb) = offset.SignificantBits();
            state[MachineFixture.RegisterOffset + 0] = programLsb;
            state[MachineFixture.RegisterOffset + 1] = programMsb;

            var program = Resources.ResourceManager.GetObject(programName) as byte[];
            program.CopyTo(state, MachineFixture.MemoryStateOffset + offset);

            if (0.Equals(offset))
            {
                state[MachineFixture.MemoryStateOffset + 0xFFFE] = 0xFF;
                state[MachineFixture.MemoryStateOffset + 0xFFFF] = 0xFF;
            }

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
    }
}
