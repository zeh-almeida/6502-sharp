using Cpu.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Cpu.Forms;

public static class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    public static void Main()
    {
        ApplicationConfiguration.Initialize();

        var sp = BuildProvider();
        using var view = sp.GetRequiredService<CpuView>();

        Application.Run(view);
    }

    private static IServiceProvider BuildProvider()
    {
        var collection = new ServiceCollection();

        return collection
            .AddLogging(b => b.AddSimpleConsole())
            .Add6502Cpu()
            .AddSingleton<CpuView>()
            .BuildServiceProvider();
    }
}