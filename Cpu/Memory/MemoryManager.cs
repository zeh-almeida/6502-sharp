using Cpu.Extensions;
using Cpu.Registers;
using Microsoft.Extensions.Logging;

namespace Cpu.Memory
{
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
        public void WriteZeroPage(ushort address, byte value)
        {
            var finalAddressZero = WrapZeroPageAddress(address);
            this.WriteAbsolute(finalAddressZero, value);
        }

        /// <inheritdoc/>
        public void WriteZeroPageX(ushort address, byte value)
        {
            var addressZero = (ushort)(address + this.RegisterManager.IndexX);
            var finalAddressZero = WrapZeroPageAddress(addressZero);

            this.WriteAbsolute(finalAddressZero, value);
        }

        /// <inheritdoc/>
        public void WriteZeroPageY(ushort address, byte value)
        {
            var addressZero = (ushort)(address + this.RegisterManager.IndexY);
            var finalAddressZero = WrapZeroPageAddress(addressZero);

            this.WriteAbsolute(finalAddressZero, value);
        }

        /// <inheritdoc/>
        public void WriteAbsolute(ushort address, byte value)
        {
            this.Logger.LogInformation(MemoryEvents.OnWrite, "{value:X2} @ {address:X4}", value, address);
            this.MemoryArea.Span[address] = value;
        }

        /// <inheritdoc/>
        public void WriteAbsoluteX(ushort address, byte value)
        {
            var addressX = (ushort)(address + this.RegisterManager.IndexX);
            this.WriteAbsolute(addressX, value);
        }

        /// <inheritdoc/>
        public void WriteAbsoluteY(ushort address, byte value)
        {
            var addressY = (ushort)(address + this.RegisterManager.IndexY);
            this.WriteAbsolute(addressY, value);
        }

        /// <inheritdoc/>
        public void WriteIndirect(ushort address, byte value)
        {
            var realAddress = this.ReadWord(address);
            this.WriteAbsolute(realAddress, value);
        }

        /// <inheritdoc/>
        public void WriteIndirectX(ushort address, byte value)
        {
            var addressX = (ushort)(address + this.RegisterManager.IndexX);
            var realAddress = this.ReadWord(addressX);

            this.WriteAbsolute(realAddress, value);
        }

        /// <inheritdoc/>
        public void WriteIndirectY(ushort address, byte value)
        {
            var realAddress = this.ReadWord(address);
            var addressY = (ushort)(realAddress + this.RegisterManager.IndexY);

            this.WriteAbsolute(addressY, value);
        }
        #endregion

        #region Read
        /// <inheritdoc/>
        public byte ReadZeroPage(ushort address)
        {
            var finalAddressZero = WrapZeroPageAddress(address);
            return this.ReadAbsolute(finalAddressZero);
        }

        /// <inheritdoc/>
        public byte ReadZeroPageX(ushort address)
        {
            var addressZero = (ushort)(address + this.RegisterManager.IndexX);
            var finalAddressZero = WrapZeroPageAddress(addressZero);

            return this.ReadAbsolute(finalAddressZero);
        }

        /// <inheritdoc/>
        public byte ReadZeroPageY(ushort address)
        {
            var addressZero = (ushort)(address + this.RegisterManager.IndexY);
            var finalAddressZero = WrapZeroPageAddress(addressZero);

            return this.ReadAbsolute(finalAddressZero);
        }

        /// <inheritdoc/>
        public byte ReadAbsolute(ushort address)
        {
            var value = this.MemoryArea.Span[address];
            this.Logger.LogInformation(MemoryEvents.OnRead, "{value:X2} @ {address:X4}", value, address);

            return value;
        }

        /// <inheritdoc/>
        public (bool, byte) ReadAbsoluteX(ushort address)
        {
            var finalAddress = (ushort)(address + this.RegisterManager.IndexX);

            var pageCrossed = address.CheckPageCrossed(finalAddress);
            return (pageCrossed, this.ReadAbsolute(finalAddress));
        }

        /// <inheritdoc/>
        public (bool, byte) ReadAbsoluteY(ushort address)
        {
            var finalAddress = (ushort)(address + this.RegisterManager.IndexY);

            var pageCrossed = address.CheckPageCrossed(finalAddress);
            return (pageCrossed, this.ReadAbsolute(finalAddress));
        }

        /// <inheritdoc/>
        public byte ReadIndirect(ushort address)
        {
            var realAddress = this.ReadAbsolute(address);
            return this.ReadAbsolute(realAddress);
        }

        /// <inheritdoc/>
        public byte ReadIndirectX(ushort address)
        {
            var addressX = (ushort)(address + this.RegisterManager.IndexX);
            var realAddress = this.ReadWord(addressX);

            return this.ReadAbsolute(realAddress);
        }

        /// <inheritdoc/>
        public (bool, byte) ReadIndirectY(ushort address)
        {
            var realAddress = this.ReadWord(address);
            var finalAddress = (ushort)(realAddress + this.RegisterManager.IndexY);

            var pageCrossed = realAddress.CheckPageCrossed(finalAddress);
            return (pageCrossed, this.ReadAbsolute(finalAddress));
        }
        #endregion

        #region Save/Load
        /// <inheritdoc/>
        public ReadOnlyMemory<byte> Save()
        {
            return this.MemoryArea;
        }

        /// <inheritdoc/>
        public void Load(ReadOnlyMemory<byte> data)
        {
            if (data.IsEmpty)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (!IMemoryManager.Length.Equals(data.Length))
            {
                throw new ArgumentOutOfRangeException(nameof(data), $"Must have a length of {IMemoryManager.Length}");
            }

            data.CopyTo(this.MemoryArea);
        }
        #endregion

        private ushort ReadWord(ushort address)
        {
            var addressLsb = this.ReadAbsolute(address);
            var addressMsb = this.ReadAbsolute((ushort)(address + 1));

            return addressLsb.CombineBytes(addressMsb);
        }

        private static ushort WrapZeroPageAddress(ushort address)
        {
            return (ushort)(address & 0xFF);
        }
    }
}
