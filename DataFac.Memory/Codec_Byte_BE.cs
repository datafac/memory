using System;

#pragma warning disable CA1707 // Identifiers should not contain underscores

namespace DataFac.Memory;

/// <summary>
/// Codec for reading and writing a single byte.
/// </summary>
public sealed class Codec_Byte_BE : ISpanCodec<Byte>
{
    /// <inheritdoc />
    public static Byte ReadFromSpan(ReadOnlySpan<byte> source) => (Byte)source[0];

    /// <inheritdoc />
    public static void WriteToSpan(Span<byte> target, in Byte input) => target[0] = (byte)input;
}
