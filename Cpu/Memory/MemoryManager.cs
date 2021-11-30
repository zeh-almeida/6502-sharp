using Cpu.Extensions;
using Cpu.Registers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cpu.Memory
{
    /// <summary>
    /// Implements <see cref="IMemoryManager"/> to manipulate memory
    /// </summary>
    public sealed record MemoryManager : IMemoryManager
    {
        #region Properties
        private IRegisterManager RegisterManager { get; }

        private byte[] MemoryArea { get; }
        #endregion

        #region Constructors
        /// <summary>
        /// Instantiates a new memory manager
        /// </summary>
        /// <param name="registerManager"><see cref="IRegisterManager"/> to read Indexes from</param>
        /// <see cref="IRegisterManager.IndexX"/>
        /// <see cref="IRegisterManager.IndexY"/>
        public MemoryManager(IRegisterManager registerManager)
        {
            this.RegisterManager = registerManager;
            this.MemoryArea = new byte[IMemoryManager.Length];
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
            this.MemoryArea[address] = value;
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
            return this.MemoryArea[address];
        }

        /// <inheritdoc/>
        public byte ReadAbsoluteX(ushort address)
        {
            var addressX = (ushort)(address + this.RegisterManager.IndexX);
            return this.ReadAbsolute(addressX);
        }

        /// <inheritdoc/>
        public byte ReadAbsoluteY(ushort address)
        {
            var addressY = (ushort)(address + this.RegisterManager.IndexY);
            return this.ReadAbsolute(addressY);
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
        public byte ReadIndirectY(ushort address)
        {
            var realAddress = this.ReadWord(address);
            var addressY = (ushort)(realAddress + this.RegisterManager.IndexY);

            return this.ReadAbsolute(addressY);
        }
        #endregion

        #region Save/Load
        /// <inheritdoc/>
        public IEnumerable<byte> Save()
        {
            return this.MemoryArea.Clone() as byte[];
        }

        /// <inheritdoc/>
        public void Load(IEnumerable<byte> data)
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (!IMemoryManager.Length.Equals(data.Count()))
            {
                throw new ArgumentOutOfRangeException(nameof(data), $"Must have a length of {IMemoryManager.Length}");
            }

            var dataArr = data.ToArray();
            Array.Copy(dataArr, this.MemoryArea, IMemoryManager.Length);
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
