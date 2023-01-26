using Cpu.States;

namespace Cpu.Maui.Utilities;

public static class FileSelector
{
    public static async Task<ReadOnlyMemory<byte>> LoadProgram(PickOptions options)
    {
        var program = new Memory<byte>(new byte[ICpuState.Length]);
        var result = await FilePicker.Default.PickAsync(options);

        if (result is not null)
        {
            using var stream = await result.OpenReadAsync();
            _ = await stream.ReadAsync(program);
        }

        return program;
    }
}
