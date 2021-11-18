using Cpu.States;

namespace Cpu.Execution
{
    /// <summary>
    /// Decodes an instruction based on a <see cref="ICpuState"/>
    /// </summary>
    public interface IDecoder
    {
        /// <summary>
        /// Decodes the next instruction
        /// </summary>
        /// <param name="currentState"><see cref="ICpuState"/> to gather information from</param>
        /// <returns>Decoded instruction</returns>
        DecodedInstruction Decode(ICpuState currentState);
    }
}
