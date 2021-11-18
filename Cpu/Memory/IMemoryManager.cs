using System.Collections.Generic;

namespace Cpu.Memory
{
    /// <summary>
    /// Allows manipulation of the CPU Memory
    /// </summary>
    public interface IMemoryManager
    {
        #region Constants
        /// <summary>
        /// Available system memory length in bytes
        /// </summary>
        public const int Length = ushort.MaxValue + 1;
        #endregion

        #region Write
        /// <summary>
        /// <para>Writes an 8-bit value using an 8-bit address fot the zero page space</para>
        /// <para>The Zero Page is made of the first 256 addresses of RAM, from 0x00 to 0xFF</para>
        /// <para>Because the address is a 16-bit value, if it exceeds <c>0xFF</c> it will wrap around</para>
        /// </summary>
        /// <param name="address">8-bit address</param>
        /// <param name="value">8-bit value</param>
        void WriteZeroPage(ushort address, byte value);

        /// <summary>
        /// <para>Writes an 8-bit value using an 8-bit address fot the zero page space</para>
        /// <para>The Zero Page is made of the first 256 addresses of RAM, from 0x00 to 0xFF</para>
        /// <para>Because the address is a 16-bit value, if it exceeds <c>0xFF</c> it will wrap around</para>
        /// <para>The provided address will have the X Register value added to it</para>
        /// <see cref="Registers.IRegisterManager.IndexX"/>
        /// </summary>
        /// <param name="address">8-bit address</param>
        /// <param name="value">8-bit value</param>
        void WriteZeroPageX(ushort address, byte value);

        /// <summary>
        /// <para>Writes an 8-bit value using an 8-bit address fot the zero page space</para>
        /// <para>The Zero Page is made of the first 256 addresses of RAM, from 0x00 to 0xFF</para>
        /// <para>Because the address is a 16-bit value, if it exceeds <c>0xFF</c> it will wrap around</para>
        /// <para>The provided address will have the Y Register value added to it</para>
        /// <see cref="Registers.IRegisterManager.IndexY"/>
        /// </summary>
        /// <param name="address">8-bit address</param>
        /// <param name="value">8-bit value</param>
        void WriteZeroPageY(ushort address, byte value);

        /// <summary>
        /// Writes an 8-bit value using an 16-bit address which must point to any address in the 16-bit space
        /// </summary>
        /// <param name="address">16-bit address</param>
        /// <param name="value">8-bit value</param>
        void WriteAbsolute(ushort address, byte value);

        /// <summary>
        /// <para>Writes an 8-bit value using an 16-bit address which must point to any address in the 16-bit space</para>
        /// <para>The provided address will have the X Register value added to it</para>
        /// <see cref="Registers.IRegisterManager.IndexX"/>
        /// </summary>
        /// <param name="address">16-bit address</param>
        /// <param name="value">8-bit value</param>
        void WriteAbsoluteX(ushort address, byte value);

        /// <summary>
        /// <para>Writes an 8-bit value using an 16-bit address which must point to any address in the 16-bit space</para>
        /// <para>The provided address will have the Y Register value added to it</para>
        /// <see cref="Registers.IRegisterManager.IndexY"/>
        /// </summary>
        /// <param name="address">16-bit address</param>
        /// <param name="value">8-bit value</param>
        void WriteAbsoluteY(ushort address, byte value);

        /// <summary>
        /// <para>Based on the supplied address, reads the zero page for the real address</para>
        /// <para>The supplied address reads the LSB and the address increases by one for the MSB</para>
        /// <para>Writes an 8-bit value using the calculated address</para>
        /// </summary>
        /// <param name="address">16-bit address pointing to the zero page</param>
        /// <param name="value">8-bit value</param>
        void WriteIndirect(ushort address, byte value);

        /// <summary>
        /// <para>Based on the supplied address and the X Register, reads the zero page for the real address</para>
        /// <para>The supplied address reads the LSB and the address increases by one for the MSB</para>
        /// <para>Writes an 8-bit value using the calculated address</para>
        /// <see cref="Registers.IRegisterManager.IndexX"/>
        /// </summary>
        /// <param name="address">16-bit address pointing to the zero page</param>
        /// <param name="value">8-bit value</param>
        void WriteIndirectX(ushort address, byte value);

        /// <summary>
        /// <para>Based on the supplied address and the Y Register, reads the zero page for the real address</para>
        /// <para>The supplied address reads the LSB and the address increases by one for the MSB
        /// then the contents of the Y Register are added</para>
        /// <para>Writes an 8-bit value using the calculated address</para>
        /// <see cref="Registers.IRegisterManager.IndexX"/>
        /// </summary>
        /// <param name="address">16-bit address pointing to the zero page</param>
        /// <param name="value">8-bit value</param>
        void WriteIndirectY(ushort address, byte value);
        #endregion

        #region Read
        /// <summary>
        /// <para>Reads an 8-bit value using an 8-bit address fot the zero page space</para>
        /// <para>The Zero Page is made of the first 256 addresses of RAM, from 0x00 to 0xFF</para>
        /// <para>Because the address is a 16-bit value, if it exceeds <c>0xFF</c> it will wrap around</para>
        /// </summary>
        /// <param name="address">8-bit address</param>
        byte ReadZeroPage(ushort address);

        /// <summary>
        /// <para>Reads an 8-bit value using an 8-bit address fot the zero page space</para>
        /// <para>The Zero Page is made of the first 256 addresses of RAM, from 0x00 to 0xFF</para>
        /// <para>Because the address is a 16-bit value, if it exceeds <c>0xFF</c> it will wrap around</para>
        /// <para>The provided address will have the X Register value added to it</para>
        /// <see cref="Registers.IRegisterManager.IndexX"/>
        /// </summary>
        /// <param name="address">8-bit address</param>
        byte ReadZeroPageX(ushort address);

        /// <summary>
        /// <para>Reads an 8-bit value using an 8-bit address fot the zero page space</para>
        /// <para>The Zero Page is made of the first 256 addresses of RAM, from 0x00 to 0xFF</para>
        /// <para>Because the address is a 16-bit value, if it exceeds <c>0xFF</c> it will wrap around</para>
        /// <para>The provided address will have the Y Register value added to it</para>
        /// <see cref="Registers.IRegisterManager.IndexY"/>
        /// </summary>
        /// <param name="address">8-bit address</param>
        byte ReadZeroPageY(ushort address);

        /// <summary>
        /// Writes an 8-bit value using an 16-bit address which must point to any address in the 16-bit space
        /// </summary>
        /// <param name="address">16-bit address</param>
        byte ReadAbsolute(ushort address);

        /// <summary>
        /// <para>Reads an 8-bit value using an 16-bit address which must point to any address in the 16-bit space</para>
        /// <para>The provided address will have the X Register value added to it</para>
        /// <see cref="Registers.IRegisterManager.IndexX"/>
        /// </summary>
        /// <param name="address">16-bit address</param>
        byte ReadAbsoluteX(ushort address);

        /// <summary>
        /// <para>Reads an 8-bit value using an 16-bit address which must point to any address in the 16-bit space</para>
        /// <para>The provided address will have the Y Register value added to it</para>
        /// <see cref="Registers.IRegisterManager.IndexY"/>
        /// </summary>
        /// <param name="address">16-bit address</param>
        byte ReadAbsoluteY(ushort address);

        /// <summary>
        /// <para>Based on the supplied address, reads the zero page for the real address</para>
        /// <para>The supplied address reads the LSB and the address increases by one for the MSB</para>
        /// <para>Reads an 8-bit value from the calculated address</para>
        /// </summary>
        /// <param name="address">16-bit address pointing to the zero page</param>
        byte ReadIndirect(ushort address);

        /// <summary>
        /// <para>Based on the supplied address and the Y Register, reads the zero page for the real address</para>
        /// <para>The supplied address reads the LSB and the address increases by one for the MSB
        /// then the contents of the Y Register are added</para>
        /// <para>Reads an 8-bit value from the calculated address</para>
        /// <see cref="Registers.IRegisterManager.IndexX"/>
        /// </summary>
        /// <param name="address">16-bit address pointing to the zero page</param>
        byte ReadIndirectX(ushort address);

        /// <summary>
        /// <para>Based on the supplied address and the Y Register, reads the zero page for the real address</para>
        /// <para>The supplied address reads the LSB and the address increases by one for the MSB
        /// then the contents of the Y Register are added</para>
        /// <para>Reads an 8-bit value from the calculated address</para>
        /// <see cref="Registers.IRegisterManager.IndexX"/>
        /// </summary>
        /// <param name="address">16-bit address pointing to the zero page</param>
        byte ReadIndirectY(ushort address);
        #endregion

        /// <summary>
        /// Serializes the current memory resulting in a byte array.
        /// <see cref="Length"/> holds the final length.
        /// </summary>
        /// <returns>Current memory content</returns>
        IEnumerable<byte> Save();

        /// <summary>
        /// Loads a <see cref="Length"/>-long byte array into memory
        /// <para>Will overwrite current data</para>
        /// </summary>
        /// <param name="data">To read values from</param>
        /// <exception cref="System.ArgumentNullException">if <paramref name="data"/> is null</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">if <paramref name="data"/> is not exactly <see cref="Length"/> bytes long</exception>
        void Load(IEnumerable<byte> data);
    }
}
