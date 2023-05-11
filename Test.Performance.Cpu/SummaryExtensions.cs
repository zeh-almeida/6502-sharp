using BenchmarkDotNet.Reports;
using System.Diagnostics.CodeAnalysis;

namespace Test.Performance.Cpu;

public static class SummaryExtensions
{
    public static int ToExitCode([NotNull] this IEnumerable<Summary> summaries)
    {
        // an empty summary means that initial filtering and validation did not allow to run
        if (!summaries.Any())
        {
            return 1;
        }

        // if anything has failed, it's an error
        return summaries.Any(summary => summary.HasAnyErrors()) ? 1 : 0;
    }

    public static bool HasAnyErrors([NotNull] this Summary summary)
    {
        return summary.HasCriticalValidationErrors || summary.Reports.Any(report => report.HasAnyErrors());
    }

    public static bool HasAnyErrors([NotNull] this BenchmarkReport report)
    {
        return !report.BuildResult.IsBuildSuccess || !report.AllMeasurements.Any();
    }
}
