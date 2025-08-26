using BenchmarkDotNet.Running;

namespace Snowspeck.Benchmarks;

internal abstract class Program
{
    private static void Main()
    {
        BenchmarkRunner.Run([
            typeof(SingleWorkerBenchmarks),
            typeof(MultiWorkerBenchmarks)
        ]);
    }
}
