using System;
using System.Buffers.Binary;

#pragma warning disable CA1707 // Identifiers should not contain underscores

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DataFac.Memory;

/// <summary>
/// Big-endian codec for reading and writing <see cref="PairOfInt64"/> from and to 16-byte spans.
/// </summary>
public sealed class Codec_PairOfInt64_BE : ISpanCodec<PairOfInt64>
{
    public static PairOfInt64 ReadFromSpan(ReadOnlySpan<byte> source)
    {
        Int64 first = BinaryPrimitives.ReadInt64BigEndian(source.Slice(0, 8));
        Int64 second = BinaryPrimitives.ReadInt64BigEndian(source.Slice(8, 8));
        return new PairOfInt64(first, second);
    }

    public static void WriteToSpan(Span<byte> target, in PairOfInt64 input)
    {
        BinaryPrimitives.WriteInt64BigEndian(target.Slice(0, 8), input.A);
        BinaryPrimitives.WriteInt64BigEndian(target.Slice(8, 8), input.B);
    }
}
