// Copyright (c) 2025 Tiago Ferreira Alves. Licensed under the MIT License.
using Microsoft.Extensions.Options;
using Snowspeck.Metadata;

namespace Snowspeck.Services;

public sealed class SignedSnowflakeService : SnowflakeServiceBase<long>
{
    public SignedSnowflakeService(IOptions<SnowflakeOptions> options) : base(options.Value) { }
    public SignedSnowflakeService(SnowflakeOptions options) : base(options) { }

    protected override long ComposeId(long timestamp, short datacenterId, short workerId, long sequence)
    {
        return (timestamp << TsShift) | ((long)datacenterId << DcShift) 
                                      | ((long)workerId << WorkerShift) 
                                      | sequence;
    }

    protected override SnowflakeMetadata ParseId(long id, long epoch)
    {
        long sequence = id & SequenceMask;
        short workerId = (short)((id >> WorkerShift) & ((1L << WorkerBits) - 1));
        short datacenterId = (short)((id >> DcShift) & ((1L << DatacenterBits) - 1));
        long timestamp = (id >> TsShift) + epoch;

        return new SnowflakeMetadata(timestamp, datacenterId, workerId, sequence);
    }
}
