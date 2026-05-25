using System;
using System.Buffers.Binary;

#pragma warning disable CA1707 // Identifiers should not contain underscores

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DataFac.Memory;

public sealed class Codec_UInt32_BE : Codec_Base<UInt32>
#if NET7_0_OR_GREATER
, ISpanCodec<UInt32>
#endif
{
    private Codec_UInt32_BE() { }
    public static Codec_UInt32_BE Instance { get; } = new Codec_UInt32_BE();

    /// <inheritdoc />
    public override UInt32 OnRead(ReadOnlySpan<byte> source) => BinaryPrimitives.ReadUInt32BigEndian(source);

    /// <inheritdoc />
    public override void OnWrite(Span<byte> target, in UInt32 input) => BinaryPrimitives.WriteUInt32BigEndian(target, input);

    /// <inheritdoc />
    public static UInt32 ReadFromSpan(ReadOnlySpan<byte> source) => BinaryPrimitives.ReadUInt32BigEndian(source);

    /// <inheritdoc />
    public static void WriteToSpan(Span<byte> target, in UInt32 input) => BinaryPrimitives.WriteUInt32BigEndian(target, input);
}
