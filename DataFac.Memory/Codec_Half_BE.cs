using System;
using System.Buffers.Binary;

#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DataFac.Memory;

#if NET6_0_OR_GREATER
public sealed class Codec_Half_BE : ISpanCodec<Half>
{
    public static Half ReadFromSpan(ReadOnlySpan<byte> source) => BinaryPrimitives.ReadHalfBigEndian(source);
    public static void WriteToSpan(Span<byte> target, in Half input) => BinaryPrimitives.WriteHalfBigEndian(target, input);
}
#endif
