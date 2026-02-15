using DataFac.Memory;
using System;
using System.Buffers.Binary;

public sealed class Codec_QuadOfInt32_LE : Codec_Base<QuadOfInt32>
#if NET7_0_OR_GREATER
, ISpanCodec<QuadOfInt32>
#endif
{
    private Codec_QuadOfInt32_LE() { }
    public static Codec_QuadOfInt32_LE Instance { get; } = new Codec_QuadOfInt32_LE();

    public override QuadOfInt32 OnRead(ReadOnlySpan<byte> source)
    {
        Int32 a = BinaryPrimitives.ReadInt32LittleEndian(source.Slice(0, 4));
        Int32 b = BinaryPrimitives.ReadInt32LittleEndian(source.Slice(4, 4));
        Int32 c = BinaryPrimitives.ReadInt32LittleEndian(source.Slice(8, 4));
        Int32 d = BinaryPrimitives.ReadInt32LittleEndian(source.Slice(12, 4));
        return new QuadOfInt32(a, b, c, d);
    }

    public override void OnWrite(Span<byte> target, in QuadOfInt32 input)
    {
        BinaryPrimitives.WriteInt32LittleEndian(target.Slice(0, 4), input.A);
        BinaryPrimitives.WriteInt32LittleEndian(target.Slice(4, 4), input.B);
        BinaryPrimitives.WriteInt32LittleEndian(target.Slice(8, 4), input.C);
        BinaryPrimitives.WriteInt32LittleEndian(target.Slice(12, 4), input.D);
    }

    public static QuadOfInt32 ReadFromSpan(ReadOnlySpan<byte> source)
    {
        Int32 a = BinaryPrimitives.ReadInt32LittleEndian(source.Slice(0, 4));
        Int32 b = BinaryPrimitives.ReadInt32LittleEndian(source.Slice(4, 4));
        Int32 c = BinaryPrimitives.ReadInt32LittleEndian(source.Slice(8, 4));
        Int32 d = BinaryPrimitives.ReadInt32LittleEndian(source.Slice(12, 4));
        return new QuadOfInt32(a, b, c, d);
    }

    public static void WriteToSpan(Span<byte> target, in QuadOfInt32 input)
    {
        BinaryPrimitives.WriteInt32LittleEndian(target.Slice(0, 4), input.A);
        BinaryPrimitives.WriteInt32LittleEndian(target.Slice(4, 4), input.B);
        BinaryPrimitives.WriteInt32LittleEndian(target.Slice(8, 4), input.C);
        BinaryPrimitives.WriteInt32LittleEndian(target.Slice(12, 4), input.D);
    }
}
