using Cpu.States;

namespace Cpu.Forms.Serialization;

internal static class Serializer
{
    public static async Task<ReadOnlyMemory<byte>> LoadProgram(string programPath, CancellationToken token = default)
    {
        var state = new Memory<byte>(new byte[ICpuState.Length]);

        var program = await LoadFile(programPath, token);
        program.CopyTo(state[ICpuState.MemoryStateOffset..]);

        state.Span[ICpuState.MemoryStateOffset + 0xFFFE] = 0xFF;
        state.Span[ICpuState.MemoryStateOffset + 0xFFFF] = 0xFF;

        return state;
    }

    public static Task<EmulatorState> LoadState(string programPath)
    {
        return Task.Run(() =>
        {
            using var stream = File.Open(programPath, FileMode.Open);
            using var reader = new BinaryReader(stream);

            var programName = reader.ReadString();
            var state = reader.ReadBytes(ICpuState.Length);

            return new EmulatorState
            {
                State = state,
                ProgramPath = programName,
            };
        });
    }

    public static Task SaveState(string programName, ReadOnlyMemory<byte> machineState, string destinationPath)
    {
        return Task.Run(() =>
        {
            var fileName = $"{destinationPath}/{DateTime.Now:yyyy-MM-dd_HH-mm-ss}_6502.state";

            using var stream = File.Open(fileName, FileMode.Create);
            using var writer = new BinaryWriter(stream);

            writer.Write(programName);
            writer.Write(machineState.Span);
        });
    }

    private static async Task<ReadOnlyMemory<byte>> LoadFile(string programPath, CancellationToken token = default)
    {
        return await File.ReadAllBytesAsync(programPath, token);
    }
}
