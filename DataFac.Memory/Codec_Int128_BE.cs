using System;
using System.Buffers.Binary;

#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DataFac.Memory;

#if NET8_0_OR_GREATER
public sealed class Codec_Int128_BE : ISpanCodec<Int128>
{
    /// <inheritdoc />
    public static Int128 ReadFromSpan(ReadOnlySpan<byte> source) => BinaryPrimitives.ReadInt128BigEndian(source);

    /// <inheritdoc />
    public static void WriteToSpan(Span<byte> target, in Int128 input) => BinaryPrimitives.WriteInt128BigEndian(target, input);
}
#endif
