using System;

#pragma warning disable CA1707 // Identifiers should not contain underscores

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DataFac.Memory;

public sealed class Codec_Boolean_LE : ISpanCodec<Boolean>
{
    /// <inheritdoc />
    public static Boolean ReadFromSpan(ReadOnlySpan<byte> source) => source[0] != (byte)0;

    /// <inheritdoc />
    public static void WriteToSpan(Span<byte> target, in Boolean input) => target[0] = input ? (byte)1 : (byte)0;
}
