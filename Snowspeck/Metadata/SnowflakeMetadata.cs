// Copyright (c) 2025 Tiago Ferreira Alves. Licensed under the MIT License.
namespace Snowspeck.Metadata;

/// <summary>
/// Represents decoded information extracted from a generated Snowflake ID.
/// </summary>
/// <param name="Timestamp">The raw Unix timestamp in milliseconds when the ID was generated.</param>
/// <param name="DatacenterId">The datacenter identifier extracted from the ID.</param>
/// <param name="WorkerId">The worker/machine identifier extracted from the ID.</param>
/// <param name="Sequence">The per-millisecond sequence counter value.</param>
[System.Diagnostics.DebuggerDisplay("{ToFriendlyString()}")]
public readonly record struct SnowflakeMetadata(long Timestamp, short DatacenterId, short WorkerId, long Sequence)
{
    /// <summary>
    /// Returns a compact string representation of the metadata.
    /// </summary>
    public override string ToString() =>
        $"[Timestamp: {Timestamp}, DC: {DatacenterId}, Worker: {WorkerId}, Sequence: {Sequence}]";

    /// <summary>
    /// Returns a user-friendly string including the UTC datetime.
    /// </summary>
    public string ToFriendlyString() =>
        $"[{ToDateTimeOffset():u}, DC: {DatacenterId}, Worker: {WorkerId}, Sequence: {Sequence}]";

    /// <summary>
    /// Converts the raw Unix timestamp (in milliseconds) to a <see cref="DateTimeOffset"/>.
    /// </summary>
    public DateTimeOffset ToDateTimeOffset() =>
        DateTimeOffset.FromUnixTimeMilliseconds(Timestamp);
    
    /// <summary>
    /// Gets the UTC <see cref="DateTime"/> representation of the timestamp.
    /// </summary>
    public DateTime UtcDateTime => ToDateTimeOffset().UtcDateTime;

    /// <summary>
    /// Gets the Unix timestamp in whole seconds (instead of milliseconds).
    /// </summary>
    public long UnixSeconds => Timestamp / 1000;

    /// <summary>
    /// Compares two metadata instances by timestamp.
    /// </summary>
    public static bool operator <(SnowflakeMetadata left, SnowflakeMetadata right) =>
        left.Timestamp < right.Timestamp;

    /// <summary>
    /// Compares two metadata instances by timestamp.
    /// </summary>
    public static bool operator >(SnowflakeMetadata left, SnowflakeMetadata right) =>
        left.Timestamp > right.Timestamp;

    /// <summary>
    /// Compares two metadata instances by timestamp.
    /// </summary>
    public static bool operator <=(SnowflakeMetadata left, SnowflakeMetadata right) =>
        left.Timestamp <= right.Timestamp;

    /// <summary>
    /// Compares two metadata instances by timestamp.
    /// </summary>
    public static bool operator >=(SnowflakeMetadata left, SnowflakeMetadata right) =>
        left.Timestamp >= right.Timestamp;
}
