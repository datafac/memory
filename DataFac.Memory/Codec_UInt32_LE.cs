using System;
using System.Buffers.Binary;

#pragma warning disable CA1707 // Identifiers should not contain underscores

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DataFac.Memory;

public sealed class Codec_UInt32_LE : ISpanCodec<UInt32>
{
    /// <inheritdoc />
    public static UInt32 ReadFromSpan(ReadOnlySpan<byte> source) => BinaryPrimitives.ReadUInt32LittleEndian(source);

    /// <inheritdoc />
    public static void WriteToSpan(Span<byte> target, in UInt32 input) => BinaryPrimitives.WriteUInt32LittleEndian(target, input);
}
