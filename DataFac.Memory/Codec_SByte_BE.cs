using System;

#pragma warning disable CA1707 // Identifiers should not contain underscores

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DataFac.Memory;

/// <summary>
/// Big-endian codec for reading and writing a signed byte (sbyte).
/// </summary>
public sealed class Codec_SByte_BE : ISpanCodec<SByte>
{
    /// <inheritdoc />
    public static SByte ReadFromSpan(ReadOnlySpan<byte> source) => (SByte)source[0];

    /// <inheritdoc />
    public static void WriteToSpan(Span<byte> target, in SByte input) => target[0] = (byte)input;
}
