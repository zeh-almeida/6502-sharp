using Microsoft.Extensions.Logging;
using Cpu.DependencyInjection;
using Cpu.Maui.Models;

namespace Cpu.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            _ = builder.UseMauiApp<App>();

            _ = builder.Logging.AddDebug();
            _ = builder.Logging.AddSimpleConsole();

            _ = builder.Services
                .Add6502Cpu()
                .AddSingleton<MainPage>();

            _=builder.Services.AddTransient<FlagModel>();

            return builder.Build();
        }
    }
}