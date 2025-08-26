// Copyright (c) 2025 Tiago Ferreira Alves. Licensed under the MIT License.
namespace Snowspeck;

/// <summary>
/// Configuration options for the Snowflake ID generator.
/// </summary>
public class SnowflakeOptions
{
    /// <summary>
    /// The custom epoch (in Unix time milliseconds) from which IDs are calculated.  
    /// Default: 1735689600000 = Jan 1, 2025 UTC.  
    /// This reduces the number of bits needed to store the timestamp.  
    /// </summary>
    public long Epoch { get; init; } = 1735689600000L;

    /// <summary>
    /// A unique identifier for the datacenter (0–31 by default in the standard 5-bit scheme).  
    /// Used to distinguish groups of workers across different locations.  
    /// </summary>
    public short DatacenterId { get; init; }
    
    /// <summary>
    /// A unique identifier for the worker (0–31 by default in the standard 5-bit scheme).  
    /// Represents the individual process/machine inside a datacenter.  
    /// Must be unique per datacenter.  
    /// </summary>
    public short WorkerId { get; init; }
    
    /// <summary>
    /// Max tolerated backward clock skew in milliseconds.  
    /// If the clock drifts back by <= this amount, we will wait until the clock catches up.  
    /// If it exceeds this amount, we throw.  
    /// Default: 10 ms.
    /// </summary>
    public int MaxClockSkewMs { get; init; } = 10;
}
