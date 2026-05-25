using System;
using System.Buffers.Binary;

#pragma warning disable CA1707 // Identifiers should not contain underscores

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DataFac.Memory;

public sealed class Codec_UInt16_LE : Codec_Base<UInt16>
#if NET7_0_OR_GREATER
, ISpanCodec<UInt16>
#endif
{
    private Codec_UInt16_LE() { }
    public static Codec_UInt16_LE Instance { get; } = new Codec_UInt16_LE();

    /// <inheritdoc />
    public override UInt16 OnRead(ReadOnlySpan<byte> source) => BinaryPrimitives.ReadUInt16LittleEndian(source);

    /// <inheritdoc />
    public override void OnWrite(Span<byte> target, in UInt16 input) => BinaryPrimitives.WriteUInt16LittleEndian(target, input);

    /// <inheritdoc />
    public static UInt16 ReadFromSpan(ReadOnlySpan<byte> source) => BinaryPrimitives.ReadUInt16LittleEndian(source);

    /// <inheritdoc />
    public static void WriteToSpan(Span<byte> target, in UInt16 input) => BinaryPrimitives.WriteUInt16LittleEndian(target, input);
}
