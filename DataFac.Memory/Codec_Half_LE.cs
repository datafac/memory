using System;
using System.Buffers.Binary;

#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DataFac.Memory;

#if NET6_0_OR_GREATER
public sealed class Codec_Half_LE : ISpanCodec<Half>
{
    /// <inheritdoc />
    public static Half ReadFromSpan(ReadOnlySpan<byte> source) => BinaryPrimitives.ReadHalfLittleEndian(source);

    /// <inheritdoc />
    public static void WriteToSpan(Span<byte> target, in Half input) => BinaryPrimitives.WriteHalfLittleEndian(target, input);
}
#endif
