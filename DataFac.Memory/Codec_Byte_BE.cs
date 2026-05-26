using System;

#pragma warning disable CA1707 // Identifiers should not contain underscores

namespace DataFac.Memory;

/// <summary>
/// Codec for reading and writing a single byte.
/// </summary>
public sealed class Codec_Byte_BE : Codec_Base<Byte>
#if NET7_0_OR_GREATER
, ISpanCodec<Byte>
#endif
{
    private Codec_Byte_BE() { }

    /// <inheritdoc />
    public override Byte OnRead(ReadOnlySpan<byte> source) => (Byte)source[0];

    /// <inheritdoc />
    public override void OnWrite(Span<byte> target, in Byte input) => target[0] = (byte)input;

    /// <inheritdoc />
    public static Byte ReadFromSpan(ReadOnlySpan<byte> source) => (Byte)source[0];

    /// <inheritdoc />
    public static void WriteToSpan(Span<byte> target, in Byte input) => target[0] = (byte)input;
}
