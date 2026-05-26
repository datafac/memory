using System;

#pragma warning disable CA1707 // Identifiers should not contain underscores

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DataFac.Memory;

public sealed class Codec_Byte_LE : ISpanCodec<Byte>
{
    /// <inheritdoc />
    public static Byte ReadFromSpan(ReadOnlySpan<byte> source) => (Byte)source[0];

    /// <inheritdoc />
    public static void WriteToSpan(Span<byte> target, in Byte input) => target[0] = (byte)input;
}
