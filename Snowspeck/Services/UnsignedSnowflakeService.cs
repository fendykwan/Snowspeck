// Copyright (c) 2025 Tiago Ferreira Alves. Licensed under the MIT License.
using Microsoft.Extensions.Options;
using Snowspeck.Metadata;

namespace Snowspeck.Services;

public sealed class UnsignedSnowflakeService : SnowflakeServiceBase<ulong>
{
    public UnsignedSnowflakeService(IOptions<SnowflakeOptions> options)
        : base(options.Value) { }

    public UnsignedSnowflakeService(SnowflakeOptions options)
        : base(options) { }

    protected override ulong ComposeId(long timestamp, short datacenterId, short workerId, long sequence)
    {
        return ((ulong)timestamp << TsShift) | ((ulong)datacenterId << DcShift) 
                                             | ((ulong)workerId << WorkerShift) 
                                             | (ulong)sequence;
    }

    protected override SnowflakeMetadata ParseId(ulong id, long epoch)
    {
        long sequence = (long)(id & SequenceMask);
        short workerId = (short)((id >> WorkerShift) & ((1UL << WorkerBits) - 1));
        short datacenterId = (short)((id >> DcShift) & ((1UL << DatacenterBits) - 1));
        long timestamp = (long)(id >> TsShift) + epoch;

        return new SnowflakeMetadata(timestamp, datacenterId, workerId, sequence);
    }
}
