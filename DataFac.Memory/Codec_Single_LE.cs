using System;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;

#pragma warning disable CA1707 // Identifiers should not contain underscores

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DataFac.Memory;

/// <summary>
/// Little-endian codec for Single (float) values.
/// </summary>
public sealed class Codec_Single_LE : Codec_Base<Single>
#if NET7_0_OR_GREATER
, ISpanCodec<Single>
#endif
{
    private Codec_Single_LE() { }

    /// <inheritdoc />
    public override Single OnRead(ReadOnlySpan<byte> source)
    {
        Int32 value = BinaryPrimitives.ReadInt32LittleEndian(source);
        return Unsafe.As<int, float>(ref value);
    }


    /// <inheritdoc />
    public override void OnWrite(Span<byte> target, in Single input)
    {
        var input2 = input;
        int value = Unsafe.As<float, int>(ref input2);
        BinaryPrimitives.WriteInt32LittleEndian(target, value);
    }


    /// <inheritdoc />
    public static Single ReadFromSpan(ReadOnlySpan<byte> source)
    {
        Int32 value = BinaryPrimitives.ReadInt32LittleEndian(source);
        return Unsafe.As<int, float>(ref value);
    }


    /// <inheritdoc />
    public static void WriteToSpan(Span<byte> target, in Single input)
    {
        var input2 = input;
        int value = Unsafe.As<float, int>(ref input2);
        BinaryPrimitives.WriteInt32LittleEndian(target, value);
    }
}
