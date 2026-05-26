using System;
using System.Buffers.Binary;

#pragma warning disable CA1707 // Identifiers should not contain underscores

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DataFac.Memory;

public sealed class Codec_UInt64_LE : Codec_Base<UInt64>, ISpanCodec<UInt64>
{
    private Codec_UInt64_LE() { }

    /// <inheritdoc />
    public override UInt64 OnRead(ReadOnlySpan<byte> source) => BinaryPrimitives.ReadUInt64LittleEndian(source);

    /// <inheritdoc />
    public override void OnWrite(Span<byte> target, in UInt64 input) => BinaryPrimitives.WriteUInt64LittleEndian(target, input);

    /// <inheritdoc />
    public static UInt64 ReadFromSpan(ReadOnlySpan<byte> source) => BinaryPrimitives.ReadUInt64LittleEndian(source);

    /// <inheritdoc />
    public static void WriteToSpan(Span<byte> target, in UInt64 input) => BinaryPrimitives.WriteUInt64LittleEndian(target, input);
}
