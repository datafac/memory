using System;
using System.Buffers.Binary;

#pragma warning disable CA1707 // Identifiers should not contain underscores

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DataFac.Memory;

public sealed class Codec_Double_BE : ISpanCodec<Double>
{
    /// <inheritdoc />
    public static Double ReadFromSpan(ReadOnlySpan<byte> source)
    {
#if NET6_0_OR_GREATER
        return BinaryPrimitives.ReadDoubleBigEndian(source);
#else
        return BitConverter.Int64BitsToDouble(BinaryPrimitives.ReadInt64BigEndian(source));
#endif
    }

    /// <inheritdoc />
    public static void WriteToSpan(Span<byte> target, in Double input)
    {
#if NET6_0_OR_GREATER
        BinaryPrimitives.WriteDoubleBigEndian(target, input);
#else
        BinaryPrimitives.WriteInt64BigEndian(target, BitConverter.DoubleToInt64Bits(input));
#endif
    }
}
