namespace Cpu.Flags
{
    /// <summary>
    /// Manipulates the flags of the CPU.
    /// </summary>
    public interface IFlagManager
    {
        #region Properties
        /// <summary>
        /// Carry flag. Set in most arithmetic operations
        /// </summary>
        bool IsCarry { get; set; }

        /// <summary>
        /// Zero flag. Set in most arithmetic operations
        /// </summary>
        bool IsZero { get; set; }

        /// <summary>
        /// Disable interrupt flag
        /// </summary>
        bool IsInterruptDisable { get; set; }

        /// <summary>
        /// Decimal Mode flag. When enabled, some instructions behave differently
        /// </summary>
        bool IsDecimalMode { get; set; }

        /// <summary>
        /// Break flag. Signalizes the ssytem to execute a break
        /// </summary>
        bool IsBreakCommand { get; set; }

        /// <summary>
        /// Overflow flag. Set in most arithmetic operations
        /// </summary>
        bool IsOverflow { get; set; }

        /// <summary>
        /// Overflow flag. Set in most arithmetic operations
        /// </summary>
        bool IsNegative { get; set; }
        #endregion

        #region Load/Save
        /// <summary>
        /// Persists the current flag values in a 7-bit number:
        /// <para>
        /// bit 0 = Carry flag
        /// bit 1 = Zero flag
        /// bit 2 = Interrupt Disable flag
        /// bit 3 = Decimal Mode flag
        /// bit 4 = Break Command flag
        /// bit 5 = Overflow flag
        /// bit 6 = Negative flag
        /// </para>
        /// The last bit is unused.
        /// </summary>
        /// <returns>7-bit number representing current flag values</returns>
        byte Save();

        /// <summary>
        /// Loads the flag values from a 7-bit number:
        /// <para>
        /// bit 0 = Carry flag
        /// bit 1 = Zero flag
        /// bit 2 = Interrupt Disable flag
        /// bit 3 = Decimal Mode flag
        /// bit 4 = Break Command flag
        /// bit 5 = Overflow flag
        /// bit 6 = Negative flag
        /// </para>
        /// The last bit is unused.
        /// </summary>
        /// <param name="value">To read values from</param>
        void Load(byte value);
        #endregion
    }
}