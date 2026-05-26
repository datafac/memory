using System;
using System.Buffers.Binary;

#pragma warning disable CA1707 // Identifiers should not contain underscores

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DataFac.Memory;

/// <summary>
/// Little-endian codec for reading and writing <see cref="PairOfInt64"/> from and to 16-byte spans.
/// </summary>
public sealed class Codec_PairOfInt64_LE : ISpanCodec<PairOfInt64>
{
    /// <inheritdoc />
    public static PairOfInt64 ReadFromSpan(ReadOnlySpan<byte> source)
    {
        long first = BinaryPrimitives.ReadInt64LittleEndian(source.Slice(0, 8));
        long second = BinaryPrimitives.ReadInt64LittleEndian(source.Slice(8, 8));
        return new PairOfInt64(first, second);
    }

    /// <inheritdoc />
    public static void WriteToSpan(Span<byte> target, in PairOfInt64 input)
    {
        BinaryPrimitives.WriteInt64LittleEndian(target.Slice(0, 8), input.A);
        BinaryPrimitives.WriteInt64LittleEndian(target.Slice(8, 8), input.B);
    }
}
