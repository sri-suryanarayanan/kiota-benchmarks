using System.Reflection;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Perfolizer.Horology;

namespace Aveva.Platform.EntityMgmt.Tests.Benchmarks;

/// <summary>
/// Entry point for benchmark tests.
/// </summary>
public static class Program
{
    public static void Main(string[] args)
    {
        var config = ManualConfig.CreateEmpty()
            .AddColumnProvider(DefaultColumnProviders.Instance)
            .AddLogger(ConsoleLogger.Default)
            .AddAnalyser([.. DefaultConfig.Instance.GetAnalysers()])
            .AddValidator([.. DefaultConfig.Instance.GetValidators()])
            .AddJob([.. DefaultConfig.Instance.GetJobs()])
            .AddDiagnoser([.. DefaultConfig.Instance.GetDiagnosers()])
            .AddDiagnoser(new EventPipeProfiler(EventPipeProfile.GcVerbose))
            //.AddExporter(HtmlExporter.Default)
            .AddExporter(MarkdownExporter.Default)
            .WithSummaryStyle(SummaryStyle.Default.WithRatioStyle(RatioStyle.Trend).WithTimeUnit(TimeUnit.Microsecond));
        BenchmarkSwitcher.FromAssembly(Assembly.GetExecutingAssembly()).Run(args, config);
    }
}
