﻿using Cpu.Extensions;
using Cpu.Instructions.Exceptions;
using Cpu.Opcodes;
using Cpu.States;

namespace Cpu.Instructions.Illegal
{
    /// <summary>
    /// <para>Rotate Right and ADD Accumulator instruction (RRA)</para>
    /// <para>Illegal, shift right one bit in memory, then add to accumulator</para>
    /// <para>
    /// Executes the following opcodes:
    /// <c>0x67</c>,
    /// <c>0x77</c>,
    /// <c>0x6F</c>,
    /// <c>0x7F</c>,
    /// <c>0x7B</c>,
    /// <c>0x63</c>,
    /// <c>0x73</c>
    /// </para>
    /// </summary>
    /// <see href="https://masswerk.at/6502/6502_instruction_set.html#RRA"/>
    /// <seealso cref="Shifts.RotateRight"/>
    /// <seealso cref="Arithmetic.AddWithCarry"/>
    public sealed class RotateRightAdd : BaseInstruction
    {
        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="RotateRightAdd"/>
        /// </summary>
        public RotateRightAdd()
            : base(
                  new OpcodeInformation(0x67, 5, 2),
                            new OpcodeInformation(0x77, 6, 2),
                            new OpcodeInformation(0x6F, 6, 3),
                            new OpcodeInformation(0x7F, 7, 3),
                            new OpcodeInformation(0x7B, 7, 3),
                            new OpcodeInformation(0x63, 8, 2),
                            new OpcodeInformation(0x73, 8, 2))
        { }
        #endregion

        /// <inheritdoc/>
        public override void Execute(ICpuState currentState, ushort value)
        {
            var loadValue = Load(currentState, value);
            var accumulator = currentState.Registers.Accumulator;

            var oldCarry = currentState.Flags.IsCarry;
            var rotateCarry = (byte)(loadValue.IsFirstBitSet() ? 1 : 0);

            var shifted = loadValue.RotateRight(oldCarry);
            var operation = (ushort)(accumulator + shifted + rotateCarry);

            currentState.Flags.IsCarry = (operation.IsBitSet(8));
            currentState.Flags.IsOverflow = (accumulator >> 7 != operation >> 7);

            currentState.Flags.IsZero = (operation.IsZero());
            currentState.Flags.IsNegative = (operation.IsSeventhBitSet());

            currentState.Registers.Accumulator = ((byte)operation);
            Write(currentState, value, (byte)operation);
        }

        private static byte Load(ICpuState currentState, ushort address)
        {
            return currentState.ExecutingOpcode switch
            {
                0x67 => currentState.Memory.ReadZeroPage(address),
                0x77 => currentState.Memory.ReadZeroPageX(address),
                0x6F => currentState.Memory.ReadAbsolute(address),
                0x7F => currentState.Memory.ReadAbsoluteX(address),
                0x7B => currentState.Memory.ReadAbsoluteY(address),
                0x63 => currentState.Memory.ReadIndirectX(address),
                0x73 => currentState.Memory.ReadIndirectY(address),
                _ => throw new UnknownOpcodeException(currentState.ExecutingOpcode),
            };
        }

        private static void Write(ICpuState currentState, ushort address, byte value)
        {
            switch (currentState.ExecutingOpcode)
            {
                case 0x67:
                    currentState.Memory.WriteZeroPage(address, value);
                    break;

                case 0x77:
                    currentState.Memory.WriteZeroPageX(address, value);
                    break;

                case 0x6F:
                    currentState.Memory.WriteAbsolute(address, value);
                    break;

                case 0x7F:
                    currentState.Memory.WriteAbsoluteX(address, value);
                    break;

                case 0x7B:
                    currentState.Memory.WriteAbsoluteY(address, value);
                    break;

                case 0x63:
                    currentState.Memory.WriteIndirectX(address, value);
                    break;

                case 0x73:
                default:
                    currentState.Memory.WriteIndirectY(address, value);
                    break;
            }
        }
    }
}
