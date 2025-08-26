using BenchmarkDotNet.Attributes;
using Snowspeck.Interfaces;
using Snowspeck.Metadata;
using Snowspeck.Services;

namespace Snowspeck.Benchmarks;

[MemoryDiagnoser]
[SimpleJob]
[HideColumns("Job", "LaunchCount", "WarmupCount", "IterationCount", "Threads")]
public class SingleWorkerBenchmarks
{
    private readonly ISnowflakeGenerator<long> _signed;
    private readonly ISnowflakeGenerator<ulong> _unsigned;

    public SingleWorkerBenchmarks()
    {
        var options = new SnowflakeOptions { DatacenterId = 0, WorkerId = 0 };
        _signed = new SignedSnowflakeService(options);
        _unsigned = new UnsignedSnowflakeService(options);
    }

    [Benchmark(Baseline = true)]
    public Guid Guid_NewGuid() => Guid.NewGuid();

    [Benchmark]
    public long Snowflake_Signed() => _signed.NextId();

    [Benchmark]
    public ulong Snowflake_Unsigned() => _unsigned.NextId();
    
    [Benchmark]
    public SnowflakeMetadata Snowflake_Signed_Decode()
    {
        var id = _signed.NextId();
        return _signed.Decode(id);
    }
}
