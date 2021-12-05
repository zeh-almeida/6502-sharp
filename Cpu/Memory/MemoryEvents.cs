using Microsoft.Extensions.Logging;

namespace Cpu.Memory
{
    /// <summary>
    /// Defines the <see cref="EventId"/> used to log <see cref="IMemoryManager"/> operations
    /// </summary>
    internal static class MemoryEvents
    {
        /// <summary>
        /// Signifies data being read from memory
        /// </summary>
        public static EventId OnRead { get; } = new EventId(2000, "Read memory");

        /// <summary>
        /// Signifies data being written from memory
        /// </summary>
        public static EventId OnWrite { get; } = new EventId(2100, "Write memory");
    }
}
