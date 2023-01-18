namespace Cpu.Registers;

/// <summary>
/// Manipulates the CPU registers
/// </summary>
public interface IRegisterManager
{
    #region Constants
    /// <summary>
    /// Amount of bytes it takes to store the register data
    /// </summary>
    public const int RegisterLengthBytes = 6;
    #endregion

    #region Properties
    /// <summary>
    /// 16-bit register, points to the next memory address to read opcodes from
    /// </summary>
    ushort ProgramCounter { get; set; }

    /// <summary>
    /// 8-bit register, points to the next stack address
    /// </summary>
    byte StackPointer { get; set; }

    /// <summary>
    /// 8-bit register, used for arithmetic and data manipulation
    /// </summary>
    byte Accumulator { get; set; }

    /// <summary>
    /// 8-bit register, used for indexing and to auxiliate the accumulator
    /// </summary>
    byte IndexX { get; set; }

    /// <summary>
    /// 8-bit register, used for indexing and to auxiliate the accumulator
    /// </summary>
    byte IndexY { get; set; }
    #endregion

    #region Load/Save
    /// <summary>
    /// Persists the current flag values in a 6-bit number:
    /// <para>
    /// bit 0 = program counter least significant byte
    /// bit 1 = program counter most significant byte
    /// bit 2 = Stack pointer
    /// bit 3 = Accumulator
    /// bit 4 = X Register
    /// bit 5 = Y Register
    /// </para>
    /// The last 2 bits are unused.
    /// </summary>
    /// <returns>6-bit number representing current register values</returns>
    ReadOnlyMemory<byte> Save();

    /// <summary>
    /// Loads the flag values from a 6-bit number:
    /// <para>
    /// bit 0 = program counter least significant byte
    /// bit 1 = program counter most significant byte
    /// bit 2 = Stack pointer
    /// bit 3 = Accumulator
    /// bit 4 = X Register
    /// bit 5 = Y Register
    /// </para>
    /// The last 2 bits are unused.
    /// </summary>
    /// <param name="data">To read values from</param>
    void Load(ReadOnlyMemory<byte> data);
    #endregion
}
