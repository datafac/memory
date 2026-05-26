using System;
using System.Buffers.Binary;

#pragma warning disable CA1707 // Identifiers should not contain underscores

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DataFac.Memory;

public sealed class Codec_Decimal_BE : ISpanCodec<Decimal>
{
    /// <inheritdoc />
    public static Decimal ReadFromSpan(ReadOnlySpan<byte> source)
    {
#if NET6_0_OR_GREATER
        Span<int> data = stackalloc int[4];
#else
        int[] data = new int[4];
#endif
        data[0] = BinaryPrimitives.ReadInt32BigEndian(source.Slice(0));
        data[1] = BinaryPrimitives.ReadInt32BigEndian(source.Slice(4));
        data[2] = BinaryPrimitives.ReadInt32BigEndian(source.Slice(8));
        data[3] = BinaryPrimitives.ReadInt32BigEndian(source.Slice(12));
        return new Decimal(data);
    }

    /// <inheritdoc />
    public static void WriteToSpan(Span<byte> target, in Decimal input)
    {
#if NET6_0_OR_GREATER
        Span<int> data = stackalloc int[4];
        Decimal.TryGetBits(input, data, out int _);
#else
        int[] data = Decimal.GetBits(input);
#endif
        BinaryPrimitives.WriteInt32BigEndian(target.Slice(0), data[0]);
        BinaryPrimitives.WriteInt32BigEndian(target.Slice(4), data[1]);
        BinaryPrimitives.WriteInt32BigEndian(target.Slice(8), data[2]);
        BinaryPrimitives.WriteInt32BigEndian(target.Slice(12), data[3]);
    }
}
public sealed class Codec_Decimal_LE : ISpanCodec<Decimal>
{
    /// <inheritdoc />
    public static Decimal ReadFromSpan(ReadOnlySpan<byte> source)
    {
#if NET6_0_OR_GREATER
        Span<int> data = stackalloc int[4];
#else
        int[] data = new int[4];
#endif
        data[0] = BinaryPrimitives.ReadInt32LittleEndian(source.Slice(0));
        data[1] = BinaryPrimitives.ReadInt32LittleEndian(source.Slice(4));
        data[2] = BinaryPrimitives.ReadInt32LittleEndian(source.Slice(8));
        data[3] = BinaryPrimitives.ReadInt32LittleEndian(source.Slice(12));
        return new Decimal(data);
    }

    /// <inheritdoc />
    public static void WriteToSpan(Span<byte> target, in Decimal input)
    {
#if NET6_0_OR_GREATER
        Span<int> data = stackalloc int[4];
        Decimal.TryGetBits(input, data, out int _);
#else
        int[] data = Decimal.GetBits(input);
#endif
        BinaryPrimitives.WriteInt32LittleEndian(target.Slice(0), data[0]);
        BinaryPrimitives.WriteInt32LittleEndian(target.Slice(4), data[1]);
        BinaryPrimitives.WriteInt32LittleEndian(target.Slice(8), data[2]);
        BinaryPrimitives.WriteInt32LittleEndian(target.Slice(12), data[3]);
    }
}
