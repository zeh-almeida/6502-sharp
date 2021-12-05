using Cpu.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cpu.Registers
{
    /// <summary>
    /// Implements <see cref="IRegisterManager"/> to manipulate the CPU state
    /// </summary>
    public sealed record RegisterManager : IRegisterManager
    {
        #region Constants
        private const int StateLength = 6;
        #endregion

        #region Properties
        /// <inheritdoc/>
        public ushort ProgramCounter { get; set; }

        /// <inheritdoc/>
        public byte StackPointer { get; set; }

        /// <inheritdoc/>
        public byte Accumulator { get; set; }

        /// <inheritdoc/>
        public byte IndexX { get; set; }

        /// <inheritdoc/>
        public byte IndexY { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Instantiates a new manager
        /// </summary>
        public RegisterManager()
        {
        }
        #endregion

        #region Load/Save
        /// <inheritdoc/>
        public IEnumerable<byte> Save()
        {
            (var lsb, var msb) = this.ProgramCounter.SignificantBits();

            return new byte[]
            {
                lsb,
                msb,
                this.StackPointer,
                this.Accumulator,
                this.IndexX,
                this.IndexY,
            };
        }

        /// <inheritdoc/>
        public void Load(IEnumerable<byte> data)
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (data.Count() != StateLength)
            {
                throw new ArgumentOutOfRangeException(nameof(data), $"Must have a length of {StateLength}");
            }

            var dataArr = data.ToArray();

            this.ProgramCounter = dataArr[0].CombineBytes(dataArr[1]);
            this.StackPointer = dataArr[2];
            this.Accumulator = dataArr[3];
            this.IndexX = dataArr[4];
            this.IndexY = dataArr[5];
        }
        #endregion

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"PC:{this.ProgramCounter.AsHex()};SP:{this.StackPointer.AsHex()};A:{this.Accumulator.AsHex()};X:{this.IndexX.AsHex()};Y:{this.IndexY.AsHex()}";
        }
    }
}
