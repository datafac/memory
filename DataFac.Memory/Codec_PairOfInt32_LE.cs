using System;
using System.Buffers.Binary;

#pragma warning disable CA1707 // Identifiers should not contain underscores

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DataFac.Memory;

/// <summary>
/// Little-endian codec for reading and writing <see cref="PairOfInt32"/> from and to 8-byte spans.
/// </summary>
public sealed class Codec_PairOfInt32_LE : Codec_Base<PairOfInt32>
#if NET7_0_OR_GREATER
, ISpanCodec<PairOfInt32>
#endif
{
    private Codec_PairOfInt32_LE() { }

    public override PairOfInt32 OnRead(ReadOnlySpan<byte> source)
    {
        Int32 a = BinaryPrimitives.ReadInt32LittleEndian(source.Slice(0, 4));
        Int32 b = BinaryPrimitives.ReadInt32LittleEndian(source.Slice(4, 4));
        return new PairOfInt32(a, b);
    }

    public override void OnWrite(Span<byte> target, in PairOfInt32 input)
    {
        BinaryPrimitives.WriteInt32LittleEndian(target.Slice(0, 4), input.A);
        BinaryPrimitives.WriteInt32LittleEndian(target.Slice(4, 4), input.B);
    }

    public static PairOfInt32 ReadFromSpan(ReadOnlySpan<byte> source)
    {
        Int32 a = BinaryPrimitives.ReadInt32LittleEndian(source.Slice(0, 4));
        Int32 b = BinaryPrimitives.ReadInt32LittleEndian(source.Slice(4, 4));
        return new PairOfInt32(a, b);
    }

    public static void WriteToSpan(Span<byte> target, in PairOfInt32 input)
    {
        BinaryPrimitives.WriteInt32LittleEndian(target.Slice(0, 4), input.A);
        BinaryPrimitives.WriteInt32LittleEndian(target.Slice(4, 4), input.B);
    }
}
