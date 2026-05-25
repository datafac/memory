using System;
using System.Buffers.Binary;

#pragma warning disable CA1707 // Identifiers should not contain underscores

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DataFac.Memory;

public sealed class Codec_Double_LE : Codec_Base<Double>
#if NET7_0_OR_GREATER
, ISpanCodec<Double>
#endif
{
    private Codec_Double_LE() { }
    public static Codec_Double_LE Instance { get; } = new Codec_Double_LE();

    /// <inheritdoc />
    public override double OnRead(ReadOnlySpan<byte> source)
    {
#if NET6_0_OR_GREATER
        return BinaryPrimitives.ReadDoubleLittleEndian(source);
#else
        return BitConverter.Int64BitsToDouble(BinaryPrimitives.ReadInt64LittleEndian(source));
#endif
    }

    /// <inheritdoc />
    public override void OnWrite(Span<byte> target, in double input)
    {
#if NET6_0_OR_GREATER
        BinaryPrimitives.WriteDoubleLittleEndian(target, input);
#else
        BinaryPrimitives.WriteInt64LittleEndian(target, BitConverter.DoubleToInt64Bits(input));
#endif
    }

    /// <inheritdoc />
    public static Double ReadFromSpan(ReadOnlySpan<byte> source)
    {
#if NET6_0_OR_GREATER
        return BinaryPrimitives.ReadDoubleLittleEndian(source);
#else
        return BitConverter.Int64BitsToDouble(BinaryPrimitives.ReadInt64LittleEndian(source));
#endif
    }

    /// <inheritdoc />
    public static void WriteToSpan(Span<byte> target, in Double input)
    {
#if NET6_0_OR_GREATER
        BinaryPrimitives.WriteDoubleLittleEndian(target, input);
#else
        BinaryPrimitives.WriteInt64LittleEndian(target, BitConverter.DoubleToInt64Bits(input));
#endif
    }
}
