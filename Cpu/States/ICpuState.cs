using Cpu.Execution;
using Cpu.Flags;
using Cpu.Memory;
using Cpu.Registers;

namespace Cpu.States
{
    /// <summary>
    /// Represents the CPU current state
    /// </summary>
    public interface ICpuState
    {
        #region Constants
        /// <summary>
        /// Offset for the memory location in state
        /// </summary>
        public const ushort MemoryStateOffset = 9;

        /// <summary>
        /// Offset for the register location in state
        /// </summary>
        public const ushort RegisterOffset = 3;

        /// <summary>
        /// Offset for the flags location in state
        /// </summary>
        public const ushort FlagOffset = 2;

        /// <summary>
        /// Amount of cycles to add because of a triggered interrupt 
        /// </summary>
        public const int InterruptCycleCount = 6;

        /// <summary>
        /// CPU State size when serialized
        /// </summary>
        public const int Length = ushort.MaxValue + MemoryStateOffset + 1;
        #endregion

        #region Properties
        /// <summary>
        /// Cycles left from the current execution
        /// </summary>
        int CyclesLeft { get; }

        /// <summary>
        /// Most recent opcode value
        /// </summary>
        byte ExecutingOpcode { get; }

        /// <summary>
        /// Signalizes the CPU to perform an interrupt based on external activity
        /// </summary>
        bool IsHardwareInterrupt { get; set; }

        /// <summary>
        /// Signalizes the CPU to perform an interrupt based on internal activity
        /// </summary>
        bool IsSoftwareInterrupt { get; set; }

        /// <summary>
        /// Last decoded instruction
        /// </summary>
        DecodedInstruction? DecodedInstruction { get; }

        /// <summary>
        /// Manipulates the CPU registers
        /// </summary>
        /// <see cref="IRegisterManager"/>
        IRegisterManager Registers { get; }

        /// <summary>
        /// Manipulates the CPU flags
        /// </summary>
        /// <see cref="IFlagManager"/>
        IFlagManager Flags { get; }

        /// <summary>
        /// Manipulates the CPU memory
        /// </summary>
        /// <see cref="IMemoryManager"/>
        IMemoryManager Memory { get; }

        /// <summary>
        /// Manipulates the CPU stack
        /// </summary>
        /// <see cref="IStackManager"/>
        IStackManager Stack { get; }
        #endregion

        #region Save/Load
        /// <summary>
        /// Persists the current state in a bit array.
        /// <see cref="Length"/> holds the final length.
        /// <para>
        /// bits 0-6 = CPU Flags
        /// bits 7-12 = CPU Registers
        /// bits 13-forward = CPU Memory
        /// </para>
        /// </summary>
        /// <returns>bit array representing current CPU state</returns>
        ReadOnlyMemory<byte> Save();

        /// <summary>
        /// Loads a bit array and parses its content into the state.
        /// <see cref="Length"/> holds the desired length.
        /// <para>Will overwrite current data</para>
        /// bits 0-6 = CPU Flags
        /// bits 7-12 = CPU Registers
        /// bits 13-forward = CPU Memory
        /// </summary>
        /// <param name="data">To read values from</param>
        /// <exception cref="ArgumentNullException">if <paramref name="data"/> is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">if <paramref name="data"/> is not exactly <see cref="Length"/> bytes long</exception>
        void Load(ReadOnlyMemory<byte> data);
        #endregion

        #region Cycles
        /// <summary>
        /// Prepares a new execution cycle
        /// </summary>
        void PrepareCycle();

        /// <summary>
        /// Decrements the cycle count by one
        /// </summary>
        void DecrementCycle();

        /// <summary>
        /// Increases the remaining cycles as desired.
        /// May happen when crossing memory boundaries
        /// or other special conditions.
        /// </summary>
        /// <param name="amount">Cycles to be added</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the amount is less than zero</exception>
        void IncrementCycles(int amount);

        /// <summary>
        /// Sets cycles for an interrupt execution
        /// </summary>
        void SetCycleInterrupt();

        /// <summary>
        /// Prepares the state according to the decoded instruction
        /// </summary>
        /// <param name="decoded">information of the new instruction</param>
        void SetExecutingInstruction(DecodedInstruction decoded);
        #endregion
    }
}
