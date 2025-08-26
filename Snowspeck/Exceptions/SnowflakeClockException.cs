// Copyright (c) 2025 Tiago Ferreira Alves. Licensed under the MIT License.
namespace Snowspeck.Exceptions;

/// <summary>
/// Exception thrown when the system clock moves backwards relative to the last generated ID.
/// </summary>
public sealed class SnowflakeClockException : InvalidOperationException
{
    public SnowflakeClockException(string message) : base(message) { }
}
