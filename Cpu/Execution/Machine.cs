using Cpu.Execution.Exceptions;
using Cpu.States;
using System;
using System.Collections.Generic;

namespace Cpu.Execution
{
    /// <summary>
    /// Implements the <see cref="IMachine"/> interface
    /// </summary>
    public sealed record Machine : IMachine
    {
        #region Properties
        private ICpuState State { get; }

        private IDecoder Decoder { get; }

        /// <inheritdoc/>
        public int CyclesLeft { get; private set; }

        /// <inheritdoc/>
        public bool IsHardwareInterrupt { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Instantiates a new machine
        /// </summary>
        /// <param name="state"><see cref="ICpuState"/> to maintain the execution state</param>
        /// <param name="decoder"><see cref="IDecoder"/> to decode instructions</param>
        public Machine(
            ICpuState state,
            IDecoder decoder)
        {
            this.State = state;
            this.Decoder = decoder;
        }
        #endregion

        /// <inheritdoc/>
        public bool Cycle(Action<ICpuState> afterCycle)
        {
            var result = this.Cycle();
            afterCycle(this.State);

            return result;
        }

        /// <inheritdoc/>
        public bool Cycle()
        {
            if (this.CyclesLeft > 0)
            {
                this.CyclesLeft--;
                return true;
            }
            else
            {
                return this.IsProgramRunning()
                    && this.Execute();
            }
        }

        /// <inheritdoc/>
        public void Load(IEnumerable<byte> data)
        {
            this.State.Load(data);
        }

        /// <inheritdoc/>
        public IEnumerable<byte> Save()
        {
            return this.State.Save();
        }

        private bool Execute()
        {
            var result = true;

            try
            {
                this.CyclesLeft = 0;
                this.ProcessHardwareInterrupt();

                var decoded = this.DecodeStream();

                this.AdvanceProgramCount(decoded);
                this.ExecuteDecoded(decoded);
            }
            catch (ProgramExecutionExeption)
            {
                result = false;
            }

            return result;
        }

        private DecodedInstruction DecodeStream()
        {
            try
            {
                return this.Decoder.Decode(this.State);
            }
            catch (Exception ex)
            {
                throw new ProgramExecutionExeption("Failed to decode stream", ex);
            }
        }

        private void ExecuteDecoded(DecodedInstruction decoded)
        {
            try
            {
                this.State.ExecutingOpcode = decoded.Opcode;
                this.CyclesLeft += decoded.Cycles - 1;

                decoded.Instruction.Execute(this.State, decoded.ValueParameter);
                this.CyclesLeft--;
            }
            catch (Exception ex)
            {
                throw new ProgramExecutionExeption("Failed to execute instruction", ex);
            }
        }

        private void AdvanceProgramCount(DecodedInstruction decoded)
        {
            this.State.Registers.ProgramCounter += decoded.Bytes;
        }

        private bool IsProgramRunning()
        {
            return !ushort.MaxValue.Equals(this.State.Registers.ProgramCounter);
        }

        private void ProcessHardwareInterrupt()
        {
            if (this.IsHardwareInterrupt)
            {
                var value = (ushort)(this.State.Registers.ProgramCounter + 2);
                this.State.Stack.Push16(value);

                this.State.Flags.IsBreakCommand = false;
                var bits = this.State.Flags.Save();
                this.State.Stack.Push(bits);

                // Adds cycles of fetching and pushing values.
                // It is the same amout of cycles used by the 0x00 BRK Instruction,
                // without the single decode cycle
                this.CyclesLeft += 6;
                this.IsHardwareInterrupt = false;
            }
        }
    }
}
