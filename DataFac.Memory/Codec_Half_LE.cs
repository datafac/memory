using System;
using System.Buffers.Binary;

#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DataFac.Memory;

#if NET6_0_OR_GREATER
public sealed class Codec_Half_LE : Codec_Base<Half>
#if NET7_0_OR_GREATER
, ISpanCodec<Half>
#endif
{
    private Codec_Half_LE() { }
    public static Codec_Half_LE Instance { get; } = new Codec_Half_LE();

    /// <inheritdoc />
    public override Half OnRead(ReadOnlySpan<byte> source)
    {
        return BinaryPrimitives.ReadHalfLittleEndian(source);
    }


    /// <inheritdoc />
    public override void OnWrite(Span<byte> target, in Half input)
    {
        BinaryPrimitives.WriteHalfLittleEndian(target, input);
    }

    /// <inheritdoc />
    public static Half ReadFromSpan(ReadOnlySpan<byte> source)
    {
        return BinaryPrimitives.ReadHalfLittleEndian(source);
    }


    /// <inheritdoc />
    public static void WriteToSpan(Span<byte> target, in Half input)
    {
        BinaryPrimitives.WriteHalfLittleEndian(target, input);
    }
}
#endif
