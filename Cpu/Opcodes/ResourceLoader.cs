using CommunityToolkit.Diagnostics;
using Cpu.Opcodes.Exceptions;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Cpu.Opcodes;

/// <summary>
/// Reads all available resources from the desired source
/// </summary>
public record ResourceLoader // Unsealed because of unit tests
{
    /// <summary>
    /// Reads all available Resources from the Instruction definitions
    /// </summary>
    /// <returns><see cref="ResourceSet"/> referencing all data</returns>
    /// <exception cref="MisconfiguredOpcodeException">Thrown if resources cannot be read</exception>
    /// <seealso cref="InstructionDefinition"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ResourceSet LoadInstructions()
    {
        return this.Load(InstructionDefinition.ResourceManager);
    }

    /// <summary>
    /// Reads all available Resources from the desired manager
    /// </summary>
    /// <param name="manager"><see cref="ResourceManager"/> containing all resource definitions</param>
    /// <returns><see cref="ResourceSet"/> referencing all data</returns>
    /// <exception cref="MisconfiguredOpcodeException">Thrown if resources cannot be read</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public virtual ResourceSet Load(in ResourceManager manager) // Virtual because of unit tests
    {
        Guard.IsNotNull(manager);

        return manager.GetResourceSet(CultureInfo.CurrentUICulture, true, true)
            ?? throw new MisconfiguredOpcodeException("Could not load resources");
    }
}
