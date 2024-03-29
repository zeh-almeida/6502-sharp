﻿using Cpu.Execution;
using Cpu.Flags;
using Cpu.Instructions;
using Cpu.Memory;
using Cpu.Opcodes;
using Cpu.Registers;
using Cpu.States;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

namespace Cpu.DependencyInjection;

/// <summary>
/// Extends <see cref="IServiceCollection"/> to add CPU dependency injection mechanisms
/// </summary>
public static class CpuServiceCollectionExtensions
{
    #region Constants
    private static Type InstructionType { get; } = typeof(IInstruction);
    #endregion

    /// <summary>
    /// Registers all necessary components for the CPU
    /// </summary>
    /// <param name="collection"><see cref="IServiceCollection"/>`to register to</param>
    /// <returns>Populated registry</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IServiceCollection Add6502Cpu(this IServiceCollection collection)
    {
        var instructions = LoadInstructionTypes();
        var opcodes = LoadOpcodes();

        _ = collection
            .AddScoped<IMachine, Machine>()
            .AddScoped<IDecoder, Decoder>()
            .AddScoped<ICpuState, CpuState>()
            .AddScoped<IFlagManager, FlagManager>()
            .AddScoped<IStackManager, StackManager>()
            .AddScoped<IMemoryManager, MemoryManager>()
            .AddScoped<IRegisterManager, RegisterManager>();

        foreach (var instruction in instructions)
        {
            _ = collection.AddScoped(InstructionType, instruction);
        }

        _ = collection.AddScoped(_ => opcodes);

        return collection;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Type[] LoadInstructionTypes()
    {
        return AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => InstructionType.IsAssignableFrom(t)
                                  && !t.IsInterface
                                  && !t.IsAbstract)
            .ToArray();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static IEnumerable<IOpcodeInformation> LoadOpcodes()
    {
        var loader = new OpcodeLoader();
        loader.LoadAsync().Wait();

        return loader.Opcodes;
    }
}