using System;

namespace DataFac.Memory;

/// <summary>
/// Represents a codec that can read and write values of a specific type TField to and from byte spans.
/// </summary>
/// <typeparam name="TField"></typeparam>
#pragma warning disable CA1040 // Avoid empty interfaces
public interface ISpanCodec<TField>
#pragma warning restore CA1040 // Avoid empty interfaces
{
#if NET7_0_OR_GREATER
    /// <summary>
    /// Reads a value of the given type from the read-only span of byte.
    /// </summary>
    /// <param name="source"></param>
    static abstract TField ReadFromSpan(ReadOnlySpan<byte> source);

    /// <summary>
    /// Writes a value of the given type to the target span of bytes.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="input"></param>

    static abstract void WriteToSpan(Span<byte> target, in TField input);
#endif
}
