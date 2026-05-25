using System;

#pragma warning disable CA1707 // Identifiers should not contain underscores

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DataFac.Memory;

/// <summary>
/// Little-endian codec for reading and writing a single signed byte (sbyte).
/// </summary>
public sealed class Codec_SByte_LE : Codec_Base<SByte>
#if NET7_0_OR_GREATER
, ISpanCodec<SByte>
#endif
{
    private Codec_SByte_LE() { }
    public static Codec_SByte_LE Instance { get; } = new Codec_SByte_LE();

    /// <inheritdoc />
    public override SByte OnRead(ReadOnlySpan<byte> source) => (SByte)source[0];

    /// <inheritdoc />
    public override void OnWrite(Span<byte> target, in SByte input) => target[0] = (byte)input;

    /// <inheritdoc />
    public static SByte ReadFromSpan(ReadOnlySpan<byte> source) => (SByte)source[0];

    /// <inheritdoc />
    public static void WriteToSpan(Span<byte> target, in SByte input) => target[0] = (byte)input;
}
