using System;
using System.Buffers.Binary;

#pragma warning disable CA1707 // Identifiers should not contain underscores

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DataFac.Memory;

public sealed class Codec_Int16_BE : ISpanCodec<Int16>
{
    /// <inheritdoc />
    public static Int16 ReadFromSpan(ReadOnlySpan<byte> source) => BinaryPrimitives.ReadInt16BigEndian(source);

    /// <inheritdoc />
    public static void WriteToSpan(Span<byte> target, in Int16 input) => BinaryPrimitives.WriteInt16BigEndian(target, input);
}
