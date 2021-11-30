namespace Cpu.Memory
{
    /// <summary>
    /// Allows manipulation of the CPU stack
    /// </summary>
    public interface IStackManager
    {
        /// <summary>
        /// Pushes a new 8-bit value to the stack and increments the pointer
        /// </summary>
        /// <param name="value">Value to push</param>
        void Push(byte value);

        /// <summary>
        /// Pushes a new 16-bit value to the stack and increments the pointer
        /// </summary>
        /// <param name="value">Value to push</param>
        void Push16(ushort value);

        /// <summary>
        /// Pulls an existing 8-bit value from the stack and decrements the pointer
        /// </summary>
        /// <returns>Value from stack</returns>
        byte Pull();

        /// <summary>
        /// Pulls an existing 16-bit value from the stack and decrements the pointer
        /// </summary>
        /// <returns>Value from stack</returns>
        ushort Pull16();
    }
}
