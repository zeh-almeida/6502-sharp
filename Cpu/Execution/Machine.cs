using Cpu.Execution.Exceptions;
using Cpu.Extensions;
using Cpu.States;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Cpu.Execution
{
    /// <summary>
    /// Implements the <see cref="IMachine"/> interface
    /// </summary>
    public sealed record Machine : IMachine
    {
        #region Properties
        /// <inheritdoc/>
        public ICpuState State { get; }

        private IDecoder Decoder { get; }
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
            if (this.State.CyclesLeft > 0)
            {
                this.State.CountCycle();
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

        #region Interrupts
        /// <inheritdoc/>
        public void ProcessInterrupts()
        {
            this.ProcessHardwareInterrupt();
            this.ProcessSoftwareInterrupt();
        }

        private void ProcessHardwareInterrupt()
        {
            if (this.State.IsHardwareInterrupt)
            {
                this.State.Flags.IsBreakCommand = false;
                var bits = this.State.Flags.Save();

                this.State.Stack.Push(bits);
                this.State.Stack.Push16(this.State.Registers.ProgramCounter);

                // Adds cycles of fetching and pushing values.
                // It is the same amout of cycles used by the 0x00 BRK Instruction,
                // without the single decode cycle
                this.State.SetCycleInterrupt();
                this.State.Flags.IsInterruptDisable = true;

                this.LoadInterruptProgramAddress(0xFFFA);
                this.State.IsHardwareInterrupt = false;
                this.State.IsSoftwareInterrupt = false;
            }
        }

        private void ProcessSoftwareInterrupt()
        {
            if (this.State.IsSoftwareInterrupt && !this.State.Flags.IsInterruptDisable)
            {
                this.State.Flags.IsBreakCommand = true;
                var bits = this.State.Flags.Save();

                this.State.Stack.Push(bits);
                this.State.Stack.Push16(this.State.Registers.ProgramCounter);

                // Adds cycles of fetching and pushing values.
                // It is the same amout of cycles used by the 0x00 BRK Instruction,
                // without the single decode cycle
                this.State.SetCycleInterrupt();
                this.State.Flags.IsInterruptDisable = true;

                this.LoadInterruptProgramAddress(0xFFFE);
                this.State.IsSoftwareInterrupt = false;
            }
        }

        private void LoadInterruptProgramAddress(ushort address)
        {
            var upperAddress = (ushort)(address + 1);

            var msb = this.State.Memory.ReadAbsolute(upperAddress);
            var lsb = this.State.Memory.ReadAbsolute(address);

            this.State.Registers.ProgramCounter = lsb.CombineBytes(msb);
        }
        #endregion

        private bool Execute()
        {
            var result = true;

            try
            {
                this.State.PrepareCycle();
                this.ProcessInterrupts();

                var decoded = this.DecodeStream();

                this.AdvanceProgramCount(decoded);
                this.ExecuteDecoded(decoded);
            }
            catch (ProgramExecutionExeption ex)
            {
                Debug.WriteLine(ex.InnerException.Message);
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
                this.State.SetExecutingInstruction(decoded);
                decoded.Instruction.Execute(this.State, decoded.ValueParameter);
                this.State.CountCycle();
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
    }
}
