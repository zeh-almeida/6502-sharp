using Cpu.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Test.Integrated.Cpu;

public sealed record Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        _ = services
            .Add6502Cpu()
            .AddLogging(builder =>
            {
                _ = builder
                   .AddFilter("Microsoft", LogLevel.Warning)
                   .AddFilter("System", LogLevel.Warning)
                   .AddFilter("Cpu", LogLevel.Trace);

                _ = builder.AddSimpleConsole(options =>
                {
                    options.SingleLine = true;
                    options.IncludeScopes = true;
                    options.UseUtcTimestamp = true;

                    options.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
                });
            });
    }
}
