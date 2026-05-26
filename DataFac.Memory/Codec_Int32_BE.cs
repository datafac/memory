using System;
using System.Buffers.Binary;

#pragma warning disable CA1707 // Identifiers should not contain underscores

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DataFac.Memory;

public sealed class Codec_Int32_BE : Codec_Base<Int32>
#if NET7_0_OR_GREATER
, ISpanCodec<Int32>
#endif
{
    private Codec_Int32_BE() { }

    /// <inheritdoc />
    public override Int32 OnRead(ReadOnlySpan<byte> source) => BinaryPrimitives.ReadInt32BigEndian(source);

    /// <inheritdoc />
    public override void OnWrite(Span<byte> target, in Int32 input) => BinaryPrimitives.WriteInt32BigEndian(target, input);

    /// <inheritdoc />
    public static Int32 ReadFromSpan(ReadOnlySpan<byte> source) => BinaryPrimitives.ReadInt32BigEndian(source);

    /// <inheritdoc />
    public static void WriteToSpan(Span<byte> target, in Int32 input) => BinaryPrimitives.WriteInt32BigEndian(target, input);
}
