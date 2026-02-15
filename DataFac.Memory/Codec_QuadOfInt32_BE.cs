using System;
using System.Buffers.Binary;

namespace DataFac.Memory;

public sealed class Codec_QuadOfInt32_BE : Codec_Base<QuadOfInt32>
#if NET7_0_OR_GREATER
, ISpanCodec<QuadOfInt32>
#endif
{
    private Codec_QuadOfInt32_BE() { }
    public static Codec_QuadOfInt32_BE Instance { get; } = new Codec_QuadOfInt32_BE();

    public override QuadOfInt32 OnRead(ReadOnlySpan<byte> source)
    {
        Int32 a = BinaryPrimitives.ReadInt32BigEndian(source.Slice(0, 4));
        Int32 b = BinaryPrimitives.ReadInt32BigEndian(source.Slice(4, 4));
        Int32 c = BinaryPrimitives.ReadInt32BigEndian(source.Slice(8, 4));
        Int32 d = BinaryPrimitives.ReadInt32BigEndian(source.Slice(12, 4));
        return new QuadOfInt32(a, b, c, d);
    }

    public override void OnWrite(Span<byte> target, in QuadOfInt32 input)
    {
        BinaryPrimitives.WriteInt32BigEndian(target.Slice(0, 4), input.A);
        BinaryPrimitives.WriteInt32BigEndian(target.Slice(4, 4), input.B);
        BinaryPrimitives.WriteInt32BigEndian(target.Slice(8, 4), input.C);
        BinaryPrimitives.WriteInt32BigEndian(target.Slice(12, 4), input.D);
    }

    public static QuadOfInt32 ReadFromSpan(ReadOnlySpan<byte> source)
    {
        Int32 a = BinaryPrimitives.ReadInt32BigEndian(source.Slice(0, 4));
        Int32 b = BinaryPrimitives.ReadInt32BigEndian(source.Slice(4, 4));
        Int32 c = BinaryPrimitives.ReadInt32BigEndian(source.Slice(8, 4));
        Int32 d = BinaryPrimitives.ReadInt32BigEndian(source.Slice(12, 4));
        return new QuadOfInt32(a, b, c, d);
    }

    public static void WriteToSpan(Span<byte> target, in QuadOfInt32 input)
    {
        BinaryPrimitives.WriteInt32BigEndian(target.Slice(0, 4), input.A);
        BinaryPrimitives.WriteInt32BigEndian(target.Slice(4, 4), input.B);
        BinaryPrimitives.WriteInt32BigEndian(target.Slice(8, 4), input.C);
        BinaryPrimitives.WriteInt32BigEndian(target.Slice(12, 4), input.D);
    }
}
