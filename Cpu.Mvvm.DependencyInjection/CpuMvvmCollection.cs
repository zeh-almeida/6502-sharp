using CommunityToolkit.Mvvm.Messaging;
using Cpu.DependencyInjection;
using Cpu.MVVM;
using Microsoft.Extensions.DependencyInjection;

namespace Cpu.Mvvm.DependencyInjection;

/// <summary>
/// Extends <see cref="IServiceCollection"/> to add CPU MVVM dependency injection mechanisms
/// </summary>
public static class CpuMvvmCollection
{
    /// <summary>
    /// Registers all necessary components for the CPU MVVM
    /// </summary>
    /// <param name="collection"><see cref="IServiceCollection"/>`to register to</param>
    /// <returns>Populated registry</returns>
    public static IServiceCollection Add6502CpuMvvm(this IServiceCollection collection)
    {
        _ = collection
            .Add6502Cpu()
            .AddSingleton<CpuModel>()
            .AddSingleton<MachineModel>()
            .AddSingleton<StateModel>()
            .AddSingleton<FlagModel>()
            .AddSingleton<RegisterModel>()
            .AddSingleton<RunningProgramModel>()
            .AddSingleton<IMessenger>(_ => WeakReferenceMessenger.Default);

        return collection;
    }

}
