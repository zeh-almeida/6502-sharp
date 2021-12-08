using Cpu.Execution;

namespace Cpu.Forms.Serialization
{
    internal static class Serializer
    {
        #region Constants
        private const ushort MemoryStateOffset = 7;

        private const int LoadDataLength = ushort.MaxValue + 1 + MemoryStateOffset;
        #endregion

        public static async Task LoadProgram(IMachine machine, string programPath, CancellationToken token = default)
        {
            var state = new byte[Serializer.LoadDataLength];

            var program = await LoadFile(programPath, token)
                .ConfigureAwait(false);

            program.CopyTo(state, Serializer.MemoryStateOffset);

            state[Serializer.MemoryStateOffset + 0xFFFE] = 0xFF;
            state[Serializer.MemoryStateOffset + 0xFFFF] = 0xFF;

            machine.Load(state);
        }

        public static Task<EmulatorState> LoadState(string programPath)
        {
            return Task.Run(() =>
            {
                using var stream = File.Open(programPath, FileMode.Open);
                using var reader = new BinaryReader(stream);

                var programName = reader.ReadString();
                var state = reader.ReadBytes(LoadDataLength);

                return new EmulatorState
                {
                    State = state,
                    ProgramPath = programName,
                };
            });
        }

        public static Task SaveState(string programName, IMachine machine, string destinationPath)
        {
            return Task.Run(() =>
            {
                var fileName = $"{destinationPath}/{DateTime.Now:yyyy-MM-dd_HH-mm-ss}_6502.state";

                var machineState = machine
                    .Save()
                    .ToArray();

                using var stream = File.Open(fileName, FileMode.Create);
                using var writer = new BinaryWriter(stream);

                writer.Write(programName);
                writer.Write(machineState);
            });
        }

        private static Task<byte[]> LoadFile(string programPath, CancellationToken token = default)
        {
            return File.ReadAllBytesAsync(programPath, token);
        }
    }
}
