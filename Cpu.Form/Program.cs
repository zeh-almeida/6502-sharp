using Cpu.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Cpu.Forms
{
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
            return CpuServiceCollection
                .Load()
                .AddLogging(b => b.AddSimpleConsole())
                .AddSingleton<CpuView>()
                .BuildServiceProvider();
        }
    }
}