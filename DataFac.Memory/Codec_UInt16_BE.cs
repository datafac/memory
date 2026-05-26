using System;
using System.Buffers.Binary;

#pragma warning disable CA1707 // Identifiers should not contain underscores

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DataFac.Memory;

public sealed class Codec_UInt16_BE : ISpanCodec<UInt16>
{
    /// <inheritdoc />
    public static UInt16 ReadFromSpan(ReadOnlySpan<byte> source) => BinaryPrimitives.ReadUInt16BigEndian(source);

    /// <inheritdoc />
    public static void WriteToSpan(Span<byte> target, in UInt16 input) => BinaryPrimitives.WriteUInt16BigEndian(target, input);
}
