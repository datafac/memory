using System;

#pragma warning disable CA1707 // Identifiers should not contain underscores

namespace DataFac.Memory;

/// <summary>
/// A codec for encoding and decoding Boolean values as a single byte (0 = false, 1 = true).
/// </summary>
public sealed class Codec_Boolean_BE : ISpanCodec<Boolean>
{
    /// <inheritdoc />
    public static Boolean ReadFromSpan(ReadOnlySpan<byte> source) => source[0] != (byte)0;

    /// <inheritdoc />
    public static void WriteToSpan(Span<byte> target, in Boolean input) => target[0] = input ? (byte)1 : (byte)0;
}
