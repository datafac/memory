using System;

#pragma warning disable CA1707 // Identifiers should not contain underscores

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DataFac.Memory;

public sealed class Codec_Guid_BE : Codec_Base<Guid>
#if NET7_0_OR_GREATER
, ISpanCodec<Guid>
#endif
{
    private Codec_Guid_BE() { }
    public static Codec_Guid_BE Instance { get; } = new Codec_Guid_BE();

    /// <inheritdoc />
    public override Guid OnRead(ReadOnlySpan<byte> source)
    {
#if NET8_0_OR_GREATER
        return new Guid(source, true);
#else
        return GuidHelper.ReadFromSpan(source, true);
#endif
    }

    /// <inheritdoc />
    public override void OnWrite(Span<byte> target, in Guid input)
    {
#if NET8_0_OR_GREATER
        input.TryWriteBytes(target, true, out int _);
#else
        GuidHelper.WriteToSpan(target, true, input);
#endif
    }

    /// <inheritdoc />
    public static Guid ReadFromSpan(ReadOnlySpan<byte> source)
    {
#if NET8_0_OR_GREATER
        return new Guid(source, true);
#else
        return GuidHelper.ReadFromSpan(source, true);
#endif
    }

    /// <inheritdoc />
    public static void WriteToSpan(Span<byte> target, in Guid input)
    {
#if NET8_0_OR_GREATER
        input.TryWriteBytes(target, true, out int _);
#else
        GuidHelper.WriteToSpan(target, true, input);
#endif
    }
}
