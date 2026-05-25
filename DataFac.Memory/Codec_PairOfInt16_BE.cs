using System;
using System.Buffers.Binary;

#pragma warning disable CA1707 // Identifiers should not contain underscores

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DataFac.Memory;

/// <summary>
/// Big-endian codec for reading and writing <see cref="PairOfInt16"/> from and to 4-byte spans.
/// </summary>
public sealed class Codec_PairOfInt16_BE : Codec_Base<PairOfInt16>
#if NET7_0_OR_GREATER
, ISpanCodec<PairOfInt16>
#endif
{
    private Codec_PairOfInt16_BE() { }
    public static Codec_PairOfInt16_BE Instance { get; } = new Codec_PairOfInt16_BE();

    /// <inheritdoc />
    public override PairOfInt16 OnRead(ReadOnlySpan<byte> source)
    {
        Int16 first = BinaryPrimitives.ReadInt16BigEndian(source.Slice(0, 2));
        Int16 second = BinaryPrimitives.ReadInt16BigEndian(source.Slice(2, 2));
        return new PairOfInt16(first, second);
    }

    /// <inheritdoc />
    public override void OnWrite(Span<byte> target, in PairOfInt16 input)
    {
        BinaryPrimitives.WriteInt16BigEndian(target.Slice(0, 2), input.A);
        BinaryPrimitives.WriteInt16BigEndian(target.Slice(2, 2), input.B);
    }

    /// <inheritdoc />
    public static PairOfInt16 ReadFromSpan(ReadOnlySpan<byte> source)
    {
        Int16 first = BinaryPrimitives.ReadInt16BigEndian(source.Slice(0, 2));
        Int16 second = BinaryPrimitives.ReadInt16BigEndian(source.Slice(2, 2));
        return new PairOfInt16(first, second);
    }

    /// <inheritdoc />
    public static void WriteToSpan(Span<byte> target, in PairOfInt16 input)
    {
        BinaryPrimitives.WriteInt16BigEndian(target.Slice(0, 2), input.A);
        BinaryPrimitives.WriteInt16BigEndian(target.Slice(2, 2), input.B);
    }
}
