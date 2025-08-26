using System.Collections.Concurrent;
using BenchmarkDotNet.Attributes;
using Snowspeck.Services;

namespace Snowspeck.Benchmarks;

[MemoryDiagnoser]
[SimpleJob]
[HideColumns("Job","LaunchCount","WarmupCount","IterationCount")]
public class MultiWorkerBenchmarks
{
    [Params(1, 2, 4, 8, 16, 32)]
    public int Workers { get; set; }

    [Params(10_000)]
    public int OpsPerWorker { get; set; }

    private SignedSnowflakeService[] _gens = default!;
    private ParallelOptions _po = default!;

    [GlobalSetup]
    public void Setup()
    {
        ThreadPool.GetMinThreads(out var minWorker, out var minIo);
        if (minWorker < Workers) ThreadPool.SetMinThreads(Workers, minIo);

        _po = new ParallelOptions { MaxDegreeOfParallelism = Workers };

        _gens = Enumerable.Range(0, Workers)
            .Select(i => new SignedSnowflakeService(new SnowflakeOptions
            {
                DatacenterId = 0,
                WorkerId = (short)i
            }))
            .ToArray();
    }

    [Benchmark(Baseline = true)]
    public int Guid_NewGuid_Parallel()
    {
        int acc = 0;
        Parallel.For(0, Workers, _po, w =>
        {
            int local = 0;
            for (int i = 0; i < OpsPerWorker; i++)
                local ^= Guid.NewGuid().GetHashCode();

            int init, computed;
            do { init = acc; computed = init ^ local; }
            while (Interlocked.CompareExchange(ref acc, computed, init) != init);
        });
        return acc;
    }

    [Benchmark]
    public long Snowflake_Parallel_ManyWorkers()
    {
        long acc = 0;
        Parallel.For(0, Workers, _po, w =>
        {
            var gen = _gens[w];
            long local = 0;
            for (int i = 0; i < OpsPerWorker; i++)
                local ^= gen.NextId();

            long init, computed;
            do { init = acc; computed = init ^ local; }
            while (Interlocked.CompareExchange(ref acc, computed, init) != init);
        });
        return acc;
    }
}
