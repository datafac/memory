using System;

#pragma warning disable CA1707 // Identifiers should not contain underscores

namespace DataFac.Memory;

/// <summary>
/// Base type for codecs.
/// </summary>
/// <typeparam name="TField"></typeparam>
public abstract class Codec_Base<TField> : ITypedFieldCodec<TField>
    where TField : struct
{
    /// <summary>
    /// Reads a value from a read-only span of byte in a specific endian encoding.
    /// </summary>
    /// <param name="source">The readonly span containing the bytes to decode the value.
    /// </param>
    /// <returns>The decoded value of type TField.</returns>
    public abstract TField OnRead(ReadOnlySpan<byte> source);

    /// <summary>
    /// Writes the value to the target span in a specific endian encoding.
    /// </summary>
    /// <param name="target">Target span to receive the encoded bytes.</param>
    /// <param name="input">Field value to encode and write into target.</param>
    public abstract void OnWrite(Span<byte> target, in TField input);

    /// <summary>
    /// Reads a TField value from the provided read-only span of bytes.
    /// </summary>
    /// <remarks>Delegates the parsing operation to OnRead().</remarks>
    /// <param name="source">The read-only span that contains the serialized representation of the value.</param>
    /// <returns>The TField value read from the span.</returns>
    public TField ReadFrom(ReadOnlySpan<byte> source) => OnRead(source);

    /// <summary>
    /// Writes a TField value to the target span of bytes.
    /// </summary>
    /// <param name="target">The span to write the encoded bytes to.</param>
    /// <param name="input">The value to encode and write.</param>
    public void WriteTo(Span<byte> target, in TField input) => OnWrite(target, in input);
}
