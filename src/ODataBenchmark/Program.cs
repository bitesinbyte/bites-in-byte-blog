using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Running;

namespace ODataBenchmark;

public class Program
{
    public static void Main(string[] s)
    {

        var config = ManualConfig
            .CreateMinimumViable()
            .AddExporter(HtmlExporter.Default);

        BenchmarkRunner.Run(new[]{
            BenchmarkConverter.TypeToBenchmarks( typeof(BenchmarkODataClient), config),
            BenchmarkConverter.TypeToBenchmarks( typeof(BenchmarkOdataHttp), config),
            BenchmarkConverter.TypeToBenchmarks( typeof(BenchmarkSimpleODataClient), config)
            });
    }
}
