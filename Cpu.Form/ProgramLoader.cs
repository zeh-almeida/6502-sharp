using Cpu.Execution;

namespace Cpu.Forms
{
    internal static class ProgramLoader
    {
        #region Constants
        private const ushort MemoryStateOffset = 7;

        private const ushort RegisterOffset = 1;

        private const int LoadDataLength = ushort.MaxValue + 1 + MemoryStateOffset;
        #endregion

        public static async Task LoadProgram(IMachine machine, string programPath, CancellationToken token = default)
        {
            var state = new byte[ProgramLoader.LoadDataLength];

            var program = await LoadFile(programPath, token)
                .ConfigureAwait(false);

            program.CopyTo(state, ProgramLoader.MemoryStateOffset);

            state[ProgramLoader.MemoryStateOffset + 0xFFFE] = 0xFF;
            state[ProgramLoader.MemoryStateOffset + 0xFFFF] = 0xFF;

            machine.Load(state);
        }

        public static async Task LoadState(IMachine machine, string programPath, CancellationToken token = default)
        {
            var state = await LoadFile(programPath, token)
                .ConfigureAwait(false);

            machine.Load(state);
        }

        private static Task<byte[]> LoadFile(string programPath, CancellationToken token = default)
        {
            return File.ReadAllBytesAsync(programPath, token);
        }
    }
}
