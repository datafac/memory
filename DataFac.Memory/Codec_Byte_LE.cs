using System;

#pragma warning disable CA1707 // Identifiers should not contain underscores

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DataFac.Memory;

public sealed class Codec_Byte_LE : Codec_Base<Byte>
#if NET7_0_OR_GREATER
, ISpanCodec<Byte>
#endif
{
    private Codec_Byte_LE() { }

    /// <inheritdoc />
    public override Byte OnRead(ReadOnlySpan<byte> source) => (Byte)source[0];

    /// <inheritdoc />
    public override void OnWrite(Span<byte> target, in Byte input) => target[0] = (byte)input;

    /// <inheritdoc />
    public static Byte ReadFromSpan(ReadOnlySpan<byte> source) => (Byte)source[0];

    /// <inheritdoc />
    public static void WriteToSpan(Span<byte> target, in Byte input) => target[0] = (byte)input;
}
