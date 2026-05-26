using System;
using System.Buffers.Binary;

#pragma warning disable CA1707 // Identifiers should not contain underscores

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DataFac.Memory;

public sealed class Codec_Int32_BE : ISpanCodec<Int32>
{
    /// <inheritdoc />
    public static Int32 ReadFromSpan(ReadOnlySpan<byte> source) => BinaryPrimitives.ReadInt32BigEndian(source);

    /// <inheritdoc />
    public static void WriteToSpan(Span<byte> target, in Int32 input) => BinaryPrimitives.WriteInt32BigEndian(target, input);
}
