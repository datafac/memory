using DataFac.Memory;
using System;
using System.Buffers.Binary;

#pragma warning disable CA1707 // Identifiers should not contain underscores

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DataFac.Memory;

/// <summary>
/// Little-endian codec for reading and writing <see cref="QuadOfInt32"/> from and to 16-byte spans.
/// </summary>
public sealed class Codec_QuadOfInt32_LE : ISpanCodec<QuadOfInt32>
{
    /// <inheritdoc />
    public static QuadOfInt32 ReadFromSpan(ReadOnlySpan<byte> source)
    {
        Int32 a = BinaryPrimitives.ReadInt32LittleEndian(source.Slice(0, 4));
        Int32 b = BinaryPrimitives.ReadInt32LittleEndian(source.Slice(4, 4));
        Int32 c = BinaryPrimitives.ReadInt32LittleEndian(source.Slice(8, 4));
        Int32 d = BinaryPrimitives.ReadInt32LittleEndian(source.Slice(12, 4));
        return new QuadOfInt32(a, b, c, d);
    }

    /// <inheritdoc />
    public static void WriteToSpan(Span<byte> target, in QuadOfInt32 input)
    {
        BinaryPrimitives.WriteInt32LittleEndian(target.Slice(0, 4), input.A);
        BinaryPrimitives.WriteInt32LittleEndian(target.Slice(4, 4), input.B);
        BinaryPrimitives.WriteInt32LittleEndian(target.Slice(8, 4), input.C);
        BinaryPrimitives.WriteInt32LittleEndian(target.Slice(12, 4), input.D);
    }
}
