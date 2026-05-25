using System;

namespace DataFac.Memory;

/// <summary>
/// Represents a codec that can read and write values of a specific type TField to and from byte spans.
/// </summary>
/// <typeparam name="TField"></typeparam>
public interface ITypedFieldCodec<TField>
{
    /// <summary>
    /// Reads a value of the given type from the read-only span of byte.
    /// </summary>
    /// <param name="source"></param>
    TField ReadFrom(ReadOnlySpan<byte> source);

    /// <summary>
    /// Writes a value of teh given type to the target span of bytes.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="input"></param>
    void WriteTo(Span<byte> target, in TField input);
}
