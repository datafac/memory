using System;
using System.Buffers.Binary;

#pragma warning disable CA1707 // Identifiers should not contain underscores

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DataFac.Memory;

public sealed class Codec_Char_LE : ISpanCodec<Char>
{
    /// <inheritdoc />
    public static Char ReadFromSpan(ReadOnlySpan<byte> source) => (Char)BinaryPrimitives.ReadUInt16LittleEndian(source);

    /// <inheritdoc />
    public static void WriteToSpan(Span<byte> target, in Char input) => BinaryPrimitives.WriteUInt16LittleEndian(target, (UInt16)input);
}
