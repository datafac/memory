using System;
using System.Buffers.Binary;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DataFac.Memory;

#pragma warning disable CA1707 // Identifiers should not contain underscores

/// <summary>
/// Big-endian codec for reading and writing <see cref="PairOfInt32"/> from and to 8-byte spans.
/// </summary>
public sealed class Codec_PairOfInt32_BE : Codec_Base<PairOfInt32>
#if NET7_0_OR_GREATER
, ISpanCodec<PairOfInt32>
#endif
{
    private Codec_PairOfInt32_BE() { }

    /// <inheritdoc />
    public override PairOfInt32 OnRead(ReadOnlySpan<byte> source)
    {
        Int32 a = BinaryPrimitives.ReadInt32BigEndian(source.Slice(0, 4));
        Int32 b = BinaryPrimitives.ReadInt32BigEndian(source.Slice(4, 4));
        return new PairOfInt32(a, b);
    }

    /// <inheritdoc />
    public override void OnWrite(Span<byte> target, in PairOfInt32 input)
    {
        BinaryPrimitives.WriteInt32BigEndian(target.Slice(0, 4), input.A);
        BinaryPrimitives.WriteInt32BigEndian(target.Slice(4, 4), input.B);
    }

    /// <inheritdoc />
    public static PairOfInt32 ReadFromSpan(ReadOnlySpan<byte> source)
    {
        Int32 a = BinaryPrimitives.ReadInt32BigEndian(source.Slice(0, 4));
        Int32 b = BinaryPrimitives.ReadInt32BigEndian(source.Slice(4, 4));
        return new PairOfInt32(a, b);
    }

    /// <inheritdoc />
    public static void WriteToSpan(Span<byte> target, in PairOfInt32 input)
    {
        BinaryPrimitives.WriteInt32BigEndian(target.Slice(0, 4), input.A);
        BinaryPrimitives.WriteInt32BigEndian(target.Slice(4, 4), input.B);
    }
}
