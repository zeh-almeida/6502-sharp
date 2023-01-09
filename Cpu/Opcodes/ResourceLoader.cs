﻿using System.Globalization;
using System.Resources;

namespace Cpu.Opcodes
{
    /// <summary>
    /// Reads all available resources from the desired source
    /// </summary>
    public class ResourceLoader
    {
        /// <summary>
        /// Reads all available Resources from the Instruction definitions
        /// </summary>
        /// <returns><see cref="ResourceSet"/> referencing all data</returns>
        /// <exception cref="Exception">Thrown if resources cannot be read</exception>
        /// <seealso cref="InstructionDefinition"/>
        public ResourceSet LoadInstructions()
        {
            return this.Load(InstructionDefinition.ResourceManager);
        }

        /// <summary>
        /// Reads all available Resources from the desired manager
        /// </summary>
        /// <param name="manager"><see cref="ResourceManager"/> containing all resource definitions</param>
        /// <returns><see cref="ResourceSet"/> referencing all data</returns>
        /// <exception cref="Exception">Thrown if resources cannot be read</exception>
        public virtual ResourceSet Load(ResourceManager manager)
        {
            return manager.GetResourceSet(CultureInfo.CurrentUICulture, true, true)
                    ?? throw new Exception("Could not load resources");
        }
    }
}
