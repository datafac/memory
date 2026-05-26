using System;

#pragma warning disable CA1707 // Identifiers should not contain underscores

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DataFac.Memory;

public sealed class Codec_Guid_BE : ISpanCodec<Guid>
{
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
