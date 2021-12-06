using Cpu.Execution;
using Cpu.Flags;
using Cpu.Instructions;
using Cpu.Memory;
using Cpu.Registers;
using Cpu.States;
using Microsoft.Extensions.DependencyInjection;

namespace Cpu.DependencyInjection
{
    public static class CpuServiceCollection
    {
        #region Constants
        private static Type InstructionType { get; } = typeof(IInstruction);
        #endregion

        public static IServiceCollection Load()
        {
            var collection = new ServiceCollection();
            var instructions = LoadInstructionTypes();

            _ = collection
                .AddScoped<IMachine, Machine>()
                .AddScoped<IDecoder, Decoder>()
                .AddScoped<IFlagManager, FlagManager>()
                .AddScoped<IMemoryManager, MemoryManager>()
                .AddScoped<IStackManager, StackManager>()
                .AddScoped<IRegisterManager, RegisterManager>()
                .AddScoped<ICpuState, CpuState>();

            foreach (var instruction in instructions)
            {
                _ = collection.AddScoped(InstructionType, instruction);
            }

            return collection;
        }

        private static IEnumerable<Type> LoadInstructionTypes()
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => InstructionType.IsAssignableFrom(t)
                                      && !t.IsInterface
                                      && !t.IsAbstract)
                .ToArray();
        }
    }
}