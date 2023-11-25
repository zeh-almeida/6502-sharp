using CommunityToolkit.Diagnostics;
using Cpu.Extensions;
using Cpu.Registers;
using Microsoft.Extensions.Logging;

namespace Cpu.Memory;

/// <summary>
/// Implements <see cref="IMemoryManager"/> to manipulate memory
/// </summary>
public sealed record MemoryManager : IMemoryManager
{
    #region Properties
    private ILogger<MemoryManager> Logger { get; }

    private IRegisterManager RegisterManager { get; }

    private Memory<byte> MemoryArea { get; }
    #endregion

    #region Constructors
    /// <summary>
    /// Instantiates a new memory manager
    /// </summary>
    /// <param name="logger"><see cref="ILogger{TCategoryName}"/> to report reads and writes</param>
    /// <param name="registerManager"><see cref="IRegisterManager"/> to read Indexes from</param>
    /// <see cref="IRegisterManager.IndexX"/>
    /// <see cref="IRegisterManager.IndexY"/>
    public MemoryManager(
        ILogger<MemoryManager> logger,
        IRegisterManager registerManager)
    {
        this.Logger = logger;
        this.RegisterManager = registerManager;

        this.MemoryArea = new Memory<byte>(new byte[IMemoryManager.Length]);
    }
    #endregion

    #region Write
    /// <inheritdoc/>
    public void WriteZeroPage(in ushort address, in byte value)
    {
        var finalAddressZero = WrapZeroPageAddress(address);
        this.WriteAbsolute(finalAddressZero, value);
    }

    /// <inheritdoc/>
    public void WriteZeroPageX(in ushort address, in byte value)
    {
        var addressZero = address + this.RegisterManager.IndexX;
        var finalAddressZero = WrapZeroPageAddress((ushort)addressZero);

        this.WriteAbsolute(finalAddressZero, value);
    }

    /// <inheritdoc/>
    public void WriteZeroPageY(in ushort address, in byte value)
    {
        var addressZero = address + this.RegisterManager.IndexY;
        var finalAddressZero = WrapZeroPageAddress((ushort)addressZero);

        this.WriteAbsolute(finalAddressZero, value);
    }

    /// <inheritdoc/>
    public void WriteAbsolute(in ushort address, in byte value)
    {
        this.Logger.LogAction(MemoryEvents.WriteAction, default, value, address);
        this.MemoryArea.Span[address] = value;
    }

    /// <inheritdoc/>
    public void WriteAbsoluteX(in ushort address, in byte value)
    {
        var addressX = address + this.RegisterManager.IndexX;
        this.WriteAbsolute((ushort)addressX, value);
    }

    /// <inheritdoc/>
    public void WriteAbsoluteY(in ushort address, in byte value)
    {
        var addressY = address + this.RegisterManager.IndexY;
        this.WriteAbsolute((ushort)addressY, value);
    }

    /// <inheritdoc/>
    public void WriteIndirect(in ushort address, in byte value)
    {
        var realAddress = this.ReadWord(address);
        this.WriteAbsolute(realAddress, value);
    }

    /// <inheritdoc/>
    public void WriteIndirectX(in ushort address, in byte value)
    {
        var addressX = address + this.RegisterManager.IndexX;
        var realAddress = this.ReadWord((ushort)addressX);

        this.WriteAbsolute(realAddress, value);
    }

    /// <inheritdoc/>
    public void WriteIndirectY(in ushort address, in byte value)
    {
        var realAddress = this.ReadWord(address);
        var addressY = realAddress + this.RegisterManager.IndexY;

        this.WriteAbsolute((ushort)addressY, value);
    }
    #endregion

    #region Read
    /// <inheritdoc/>
    public byte ReadZeroPage(in ushort address)
    {
        var finalAddressZero = WrapZeroPageAddress(address);
        return this.ReadAbsolute(finalAddressZero);
    }

    /// <inheritdoc/>
    public byte ReadZeroPageX(in ushort address)
    {
        var addressZero = address + this.RegisterManager.IndexX;
        var finalAddressZero = WrapZeroPageAddress((ushort)addressZero);

        return this.ReadAbsolute(finalAddressZero);
    }

    /// <inheritdoc/>
    public byte ReadZeroPageY(in ushort address)
    {
        var addressZero = address + this.RegisterManager.IndexY;
        var finalAddressZero = WrapZeroPageAddress((ushort)addressZero);

        return this.ReadAbsolute(finalAddressZero);
    }

    /// <inheritdoc/>
    public byte ReadAbsolute(in ushort address)
    {
        var value = this.MemoryArea.Span[address];
        this.Logger.LogAction(MemoryEvents.ReadAction, default, value, address);

        return value;
    }

    /// <inheritdoc/>
    public (bool, byte) ReadAbsoluteX(in ushort address)
    {
        var finalAddress = address + this.RegisterManager.IndexX;
        var unboxedAddress = (ushort)finalAddress;

        var pageCrossed = address.CheckPageCrossed(unboxedAddress);
        return (pageCrossed, this.ReadAbsolute(unboxedAddress));
    }

    /// <inheritdoc/>
    public (bool, byte) ReadAbsoluteY(in ushort address)
    {
        var finalAddress = address + this.RegisterManager.IndexY;
        var unboxedAddress = (ushort)finalAddress;

        var pageCrossed = address.CheckPageCrossed(unboxedAddress);
        return (pageCrossed, this.ReadAbsolute(unboxedAddress));
    }

    /// <inheritdoc/>
    public byte ReadIndirect(in ushort address)
    {
        var realAddress = this.ReadAbsolute(address);
        return this.ReadAbsolute(realAddress);
    }

    /// <inheritdoc/>
    public byte ReadIndirectX(in ushort address)
    {
        var addressX = address + this.RegisterManager.IndexX;
        var realAddress = this.ReadWord((ushort)addressX);

        return this.ReadAbsolute(realAddress);
    }

    /// <inheritdoc/>
    public (bool, byte) ReadIndirectY(in ushort address)
    {
        var realAddress = this.ReadWord(address);
        var finalAddress = realAddress + this.RegisterManager.IndexY;
        var unboxedAddress = (ushort)finalAddress;

        var pageCrossed = realAddress.CheckPageCrossed(unboxedAddress);
        return (pageCrossed, this.ReadAbsolute(unboxedAddress));
    }
    #endregion

    #region Save/Load
    /// <inheritdoc/>
    public ReadOnlyMemory<byte> Save()
    {
        return this.MemoryArea;
    }

    /// <inheritdoc/>
    public void Load(in ReadOnlyMemory<byte> data)
    {
        Guard.IsEqualTo(data.Length, IMemoryManager.Length, nameof(data));
        data.CopyTo(this.MemoryArea);
    }
    #endregion

    private ushort ReadWord(in ushort address)
    {
        var addressLsb = this.ReadAbsolute(address);
        var addressMsb = this.ReadAbsolute((ushort)(address + 1));

        return addressLsb.CombineBytes(addressMsb);
    }

    private static ushort WrapZeroPageAddress(in ushort address)
    {
        return (ushort)(address & 0xFF);
    }
}
