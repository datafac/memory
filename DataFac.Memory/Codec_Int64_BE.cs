using System;
using System.Buffers.Binary;

#pragma warning disable CA1707 // Identifiers should not contain underscores

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DataFac.Memory;

public sealed class Codec_Int64_BE : ISpanCodec<Int64>
{
    /// <inheritdoc />
    public static Int64 ReadFromSpan(ReadOnlySpan<byte> source) => BinaryPrimitives.ReadInt64BigEndian(source);

    /// <inheritdoc />
    public static void WriteToSpan(Span<byte> target, in Int64 input) => BinaryPrimitives.WriteInt64BigEndian(target, input);
}
