using Cpu.Execution;
using Cpu.Flags;
using Cpu.Memory;
using Cpu.Registers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cpu.States
{
    /// <summary>
    /// Implements <see cref="ICpuState"/> to keep track of the CPU state
    /// </summary>
    public sealed record CpuState : ICpuState
    {
        #region Properties
        /// <inheritdoc/>
        public int CyclesLeft { get; private set; }

        /// <inheritdoc/>
        public byte ExecutingOpcode { get; set; }

        /// <inheritdoc/>
        public bool IsHardwareInterrupt { get; set; }

        /// <inheritdoc/>
        public bool IsSoftwareInterrupt { get; set; }

        /// <inheritdoc/>
        public DecodedInstruction DecodedInstruction { get; private set; }

        /// <inheritdoc/>
        public IRegisterManager Registers { get; }

        /// <inheritdoc/>
        public IFlagManager Flags { get; }

        /// <inheritdoc/>
        public IMemoryManager Memory { get; }

        /// <inheritdoc/>
        public IStackManager Stack { get; }
        #endregion

        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="CpuState"/> manager
        /// </summary>
        /// <param name="flagManager">Allows manipulating the CPU flags</param>
        /// <param name="stackManager">Allows manipulating the CPU stack</param>
        /// <param name="memoryManager">Allows manipulating the CPU memory</param>
        /// <param name="registerManager">Allows manipulating the CPU registers</param>
        public CpuState(
            IFlagManager flagManager,
            IStackManager stackManager,
            IMemoryManager memoryManager,
            IRegisterManager registerManager)
        {
            this.Flags = flagManager;
            this.Stack = stackManager;
            this.Memory = memoryManager;
            this.Registers = registerManager;
        }
        #endregion

        #region Save/Load
        /// <inheritdoc/>
        public IEnumerable<byte> Save()
        {
            var registerState = this.Registers.Save();
            var memoryState = this.Memory.Save();

            var flagState = new byte[]
            {
                this.Flags.Save()
            };

            var currentState = new byte[]
            {
                (byte)this.CyclesLeft,
                this.ExecutingOpcode,
            };

            return currentState
                .Concat(flagState)
                .Concat(registerState)
                .Concat(memoryState)
                .ToArray();
        }

        /// <inheritdoc/>
        public void Load(IEnumerable<byte> data)
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var dataLength = data.Count();

            if (!ICpuState.Length.Equals(dataLength))
            {
                throw new ArgumentOutOfRangeException(nameof(data), $"Must have a length of {ICpuState.Length}, was {dataLength}");
            }

            var dataArr = data.ToArray();

            var registerState = data
                .Skip(ICpuState.RegisterOffset)
                .Take(6)
                .ToArray();

            var flagState = data
                .Skip(ICpuState.FlagOffset)
                .FirstOrDefault();

            var memoryState = data
                .Skip(ICpuState.MemoryStateOffset)
                .ToArray();

            this.Flags.Load(flagState);
            this.Memory.Load(memoryState);
            this.Registers.Load(registerState);

            this.CyclesLeft = dataArr[0];
            this.ExecutingOpcode = dataArr[1];

            this.IsHardwareInterrupt = false;
            this.IsSoftwareInterrupt = false;

            this.DecodedInstruction = null;
        }
        #endregion

        #region Cycles
        /// <inheritdoc/>
        public void PrepareCycle()
        {
            this.ExecutingOpcode = 0;
            this.CyclesLeft = 0;
        }

        /// <inheritdoc/>
        public void CountCycle()
        {
            this.CyclesLeft--;
        }

        /// <inheritdoc/>
        public void SetCycleInterrupt()
        {
            this.CyclesLeft += 6;
        }

        /// <inheritdoc/>
        public void SetExecutingInstruction(DecodedInstruction decoded)
        {
            this.DecodedInstruction = decoded;

            this.ExecutingOpcode = decoded.Opcode;
            this.CyclesLeft += decoded.Cycles;

            this.CountCycle();
        }
        #endregion
    }
}
