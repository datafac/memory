using System;
using System.Buffers.Binary;

#pragma warning disable CA1707 // Identifiers should not contain underscores

namespace DataFac.Memory;

#if NET8_0_OR_GREATER
/// <summary>
/// Little-endian codec for UInt128 values.
/// </summary>
public sealed class Codec_UInt128_LE : ISpanCodec<UInt128>
{
    /// <inheritdoc />
    public static UInt128 ReadFromSpan(ReadOnlySpan<byte> source) => BinaryPrimitives.ReadUInt128LittleEndian(source);

    /// <inheritdoc />
    public static void WriteToSpan(Span<byte> target, in UInt128 input) => BinaryPrimitives.WriteUInt128LittleEndian(target, input);
}
#endif
