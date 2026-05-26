using System;
using System.Buffers.Binary;

#pragma warning disable CA1707 // Identifiers should not contain underscores

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DataFac.Memory;

/// <summary>
/// Little-endian codec for reading and writing <see cref="PairOfInt16"/> from and to 4-byte spans.
/// </summary>
public sealed class Codec_PairOfInt16_LE : ISpanCodec<PairOfInt16>
{
    /// <inheritdoc />
    public static PairOfInt16 ReadFromSpan(ReadOnlySpan<byte> source)
    {
        Int16 first = BinaryPrimitives.ReadInt16LittleEndian(source.Slice(0, 2));
        Int16 second = BinaryPrimitives.ReadInt16LittleEndian(source.Slice(2, 2));
        return new PairOfInt16(first, second);
    }

    /// <inheritdoc />
    public static void WriteToSpan(Span<byte> target, in PairOfInt16 input)
    {
        BinaryPrimitives.WriteInt16LittleEndian(target.Slice(0, 2), input.A);
        BinaryPrimitives.WriteInt16LittleEndian(target.Slice(2, 2), input.B);
    }
}
