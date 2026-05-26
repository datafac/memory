using System;
using System.Buffers.Binary;

#pragma warning disable CA1707 // Identifiers should not contain underscores

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DataFac.Memory;

public sealed class Codec_Int16_LE : Codec_Base<Int16>
#if NET7_0_OR_GREATER
, ISpanCodec<Int16>
#endif
{
    private Codec_Int16_LE() { }

    /// <inheritdoc />
    public override Int16 OnRead(ReadOnlySpan<byte> source) => BinaryPrimitives.ReadInt16LittleEndian(source);

    /// <inheritdoc />
    public override void OnWrite(Span<byte> target, in Int16 input) => BinaryPrimitives.WriteInt16LittleEndian(target, input);

    /// <inheritdoc />
    public static Int16 ReadFromSpan(ReadOnlySpan<byte> source) => BinaryPrimitives.ReadInt16LittleEndian(source);

    /// <inheritdoc />
    public static void WriteToSpan(Span<byte> target, in Int16 input) => BinaryPrimitives.WriteInt16LittleEndian(target, input);
}
