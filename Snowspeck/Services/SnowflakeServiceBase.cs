// Copyright (c) 2025 Tiago Ferreira Alves. Licensed under the MIT License.
using System.Runtime.CompilerServices;
using Snowspeck.Exceptions;
using Snowspeck.Interfaces;
using Snowspeck.Metadata;

namespace Snowspeck.Services;

public abstract class SnowflakeServiceBase<TId> : ISnowflakeGenerator<TId>
{
    // Protected accessors for extensibility
    protected long Epoch => _epoch;
    protected int MaxClockSkewMs => _maxClockSkewMs;
    protected short DatacenterId => _datacenterId;
    protected short WorkerId => _workerId;

    private long _state;
    
    private readonly long  _epoch;
    private readonly short _datacenterId;
    private readonly short _workerId;
    private readonly int _maxClockSkewMs;

    // Bit layout: [41-bit timestamp offset][5-bit DC][5-bit worker][12-bit seq]
    protected const int SequenceBits = 12;
    protected const int WorkerBits = 5;
    protected const int DatacenterBits = 5;

    protected const long SequenceMask = (1L << SequenceBits) - 1;
    protected const int WorkerShift = SequenceBits;
    protected const int DcShift = SequenceBits + WorkerBits;
    protected const int TsShift = SequenceBits + WorkerBits + DatacenterBits;

    private const long MaxWorkerId = (1L << WorkerBits) - 1;
    private const long MaxDcId = (1L << DatacenterBits) - 1;

    protected SnowflakeServiceBase(SnowflakeOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        _epoch = options.Epoch;
        _datacenterId = options.DatacenterId;
        _workerId = options.WorkerId;
        _maxClockSkewMs = options.MaxClockSkewMs;

        if (_datacenterId < 0 || _datacenterId > MaxDcId)
            throw new ArgumentOutOfRangeException(
                nameof(options.DatacenterId), $"DatacenterId must be between 0 and {MaxDcId}.");
        
        if (_workerId < 0 || _workerId > MaxWorkerId)
            throw new ArgumentOutOfRangeException(
                nameof(options.WorkerId), $"WorkerId must be between 0 and {MaxWorkerId}.");
        
        if (_maxClockSkewMs < 0)
            throw new ArgumentOutOfRangeException(
                nameof(options.MaxClockSkewMs), "MaxClockSkewMs must be >= 0.");
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public TId NextId()
    {
        while (true)
        {
            long nowMs = GetCurrentTimestamp();
            long nowOffset = nowMs - _epoch;

            long oldState = Volatile.Read(ref _state);
            long lastOff = oldState >> SequenceBits;
            long seq = oldState & SequenceMask;

            if (nowOffset < lastOff)
            {
                nowOffset = HandleClockSkew(nowOffset, lastOff);
            }

            long newOff, newSeq;
            if (nowOffset == lastOff)
            {
                newOff = lastOff;
                newSeq = (seq + 1) & SequenceMask;

                if (newSeq == 0)
                {
                    newOff = WaitForNextMillisOffset(lastOff);
                    newSeq = 0;
                }
            }
            else
            {
                newOff = nowOffset;
                newSeq = 0;
            }

            long newState = (newOff << SequenceBits) | newSeq;

            if (Interlocked.CompareExchange(ref _state, newState, oldState) == oldState)
                return ComposeId(newOff, _datacenterId, _workerId, newSeq);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SnowflakeMetadata Decode(TId id) => ParseId(id, _epoch);
    
    protected abstract TId ComposeId(long timestampOffset, short datacenterId, short workerId, long sequence);
    protected abstract SnowflakeMetadata ParseId(TId id, long epoch);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected virtual long GetCurrentTimestamp() => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    protected virtual long WaitForNextMillisOffset(long lastOffset)
    {
        long off;
        SpinWait spinner = new SpinWait();

        do
        {
            off = GetCurrentTimestamp() - _epoch;
            if (off <= lastOffset)
            {
                spinner.SpinOnce();
                if (spinner.Count > 50) Thread.Yield();
            }
        } while (off <= lastOffset);

        return off;
    }
    
    protected virtual long HandleClockSkew(long nowOffset, long lastOffset)
    {
        long drift = lastOffset - nowOffset;
        if (drift <= _maxClockSkewMs) return WaitForNextMillisOffset(lastOffset);
        throw new SnowflakeClockException(
            $"System clock moved backwards by {drift} ms (max tolerated {_maxClockSkewMs} ms).");
    }
}
