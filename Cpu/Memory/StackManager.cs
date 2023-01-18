using Cpu.Extensions;
using Cpu.Registers;

namespace Cpu.Memory;

/// <summary>
/// Implements <see cref="IStackManager"/> to manipulate the stack
/// </summary>
public sealed record StackManager : IStackManager
{
    #region Constants
    private const ushort LowestAddress = 0x0100;
    #endregion

    #region Properties
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
        var pointer = this.RegisterManager.StackPointer;
        var address = PadStackPointer(pointer);

        this.MemoryManager.WriteAbsolute(address, value);
        this.RegisterManager.StackPointer = (byte)(pointer - 1);
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
        var pointer = this.RegisterManager.StackPointer;
        var finalPointer = (byte)(pointer + 1);
        this.RegisterManager.StackPointer = finalPointer;

        var address = PadStackPointer(finalPointer);
        return this.MemoryManager.ReadAbsolute(address);
    }

    /// <inheritdoc/>
    public ushort Pull16()
    {
        var lsb = this.Pull();
        var msb = this.Pull();

        return lsb.CombineBytes(msb);
    }

    private static ushort PadStackPointer(byte value)
    {
        return (ushort)(LowestAddress | value);
    }
}
