using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Markup;
using Cpu.Mvvm.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Cpu.Maui;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseMauiCommunityToolkitMarkup()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services
            .Add6502CpuMvvm()
            .AddSingleton<MainPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}