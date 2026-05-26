using System;
using System.Buffers.Binary;

#pragma warning disable CA1707 // Identifiers should not contain underscores

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DataFac.Memory;

public sealed class Codec_UInt64_BE : Codec_Base<UInt64>
#if NET7_0_OR_GREATER
, ISpanCodec<UInt64>
#endif
{
    private Codec_UInt64_BE() { }

    /// <inheritdoc />
    public override UInt64 OnRead(ReadOnlySpan<byte> source) => BinaryPrimitives.ReadUInt64BigEndian(source);

    /// <inheritdoc />
    public override void OnWrite(Span<byte> target, in UInt64 input) => BinaryPrimitives.WriteUInt64BigEndian(target, input);

    /// <inheritdoc />
    public static UInt64 ReadFromSpan(ReadOnlySpan<byte> source) => BinaryPrimitives.ReadUInt64BigEndian(source);

    /// <inheritdoc />
    public static void WriteToSpan(Span<byte> target, in UInt64 input) => BinaryPrimitives.WriteUInt64BigEndian(target, input);
}
