﻿using Cpu.States;

namespace Cpu.Execution;

/// <summary>
/// Represents a full 6502 machine.
/// Allows the execution of a program, cycle by cycle.
/// </summary>
public interface IMachine
{
    #region Properties
    /// <summary>
    /// Current CPU state
    /// </summary>
    ICpuState State { get; }

    /// <summary>
    /// Status of the last cycle execution
    /// </summary>
    bool HasCycled { get; }
    #endregion

    #region Execution
    /// <summary>
    /// Performs a single cycle of execution in the current program
    /// and allows to validate the state after the cycle
    /// </summary>
    /// <param name="afterCycle">Action to validate <see cref="ICpuState"/></param>
    /// <returns>True if the cycle completed successfully, false otherwise</returns>
    /// <exception cref="Exceptions.ProgramExecutionException"></exception>
    bool Cycle(in Action<ICpuState> afterCycle);

    /// <summary>
    /// Performs a single cycle of execution in the current program
    /// </summary>
    /// <returns>True if the cycle completed successfully, false otherwise</returns>
    /// <exception cref="Exceptions.ProgramExecutionException"></exception>
    bool Cycle();
    #endregion

    #region Save/Load
    /// <summary>
    /// Loads the program data
    /// </summary>
    /// <param name="data">Program data</param>
    /// <exception cref="ArgumentNullException"> thrown if the program supplied is null</exception>
    /// <exception cref="ArgumentOutOfRangeException">thrown if the program supplied has an invalid length</exception>
    /// <see cref="ICpuState.Load(in ReadOnlyMemory{byte})"/>
    void Load(in ReadOnlyMemory<byte> data);

    /// <summary>
    /// Saves the current machine state into bytes
    /// Contains all the registers, flags and memory data
    /// </summary>
    /// <returns>Current state as bytes</returns>
    ReadOnlyMemory<byte> Save();
    #endregion

    #region Interrupts
    /// <summary>
    /// Processes all hardware and software based interrupts when signaled
    /// </summary>
    void ProcessInterrupts();
    #endregion
}
