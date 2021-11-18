using Cpu.Extensions;
using Cpu.Registers;

namespace Cpu.Memory
{
    /// <summary>
    /// Implements <see cref="IStackManager"/> to manipulate the stack
    /// </summary>
    public sealed record StackManager : IStackManager
    {
        #region Constants
        private const ushort LowestAddress = 0x0100;
        #endregion

        #region Properties
        /// <inheritdoc/>
        public byte StackPointer { get; private set; }

        private IMemoryManager MemoryManager { get; }

        private IRegisterManager RegisterManager { get; }
        #endregion

        #region Constructors
        /// <inheritdoc/>
        public StackManager(
            IMemoryManager memoryManager,
            IRegisterManager registerManager)
        {
            this.MemoryManager = memoryManager;
            this.RegisterManager = registerManager;
        }
        #endregion

        /// <inheritdoc/>
        public void Push(byte value)
        {
            this.StackPointer = this.RegisterManager.StackPointer;
            var memoryPointer = this.PadStackPointer();

            this.StackPointer++;
            this.RegisterManager.StackPointer = (this.StackPointer);

            this.MemoryManager.WriteAbsolute(memoryPointer, value);
        }

        /// <inheritdoc/>
        public void Push16(ushort value)
        {
            (var lsb, var msb) = value.SignificantBits();

            this.Push(msb);
            this.Push(lsb);
        }

        /// <inheritdoc/>
        public byte Pull()
        {
            this.StackPointer = this.RegisterManager.StackPointer;
            this.StackPointer--;

            this.RegisterManager.StackPointer = (this.StackPointer);

            var memoryPointer = this.PadStackPointer();
            return this.MemoryManager.ReadAbsolute(memoryPointer);
        }

        /// <inheritdoc/>
        public ushort Pull16()
        {
            var lsb = this.Pull();
            var msb = this.Pull();

            return lsb.CombineBytes(msb);
        }

        private ushort PadStackPointer()
        {
            return (ushort)(LowestAddress + this.StackPointer);
        }
    }
}
