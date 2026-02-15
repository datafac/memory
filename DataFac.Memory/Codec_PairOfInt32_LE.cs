using System;
using System.Buffers.Binary;

namespace DataFac.Memory
{
    public sealed class Codec_PairOfInt32_LE : Codec_Base<PairOfInt32>
#if NET7_0_OR_GREATER
    , ISpanCodec<PairOfInt32>
#endif
    {
        private Codec_PairOfInt32_LE() { }
        public static Codec_PairOfInt32_LE Instance { get; } = new Codec_PairOfInt32_LE();

        public override PairOfInt32 OnRead(ReadOnlySpan<byte> source)
        {
            Int32 a = BinaryPrimitives.ReadInt32LittleEndian(source.Slice(0, 4));
            Int32 b = BinaryPrimitives.ReadInt32LittleEndian(source.Slice(4, 4));
            return new PairOfInt32(a, b);
        }

        public override void OnWrite(Span<byte> target, in PairOfInt32 input)
        {
            BinaryPrimitives.WriteInt32LittleEndian(target.Slice(0, 4), input.A);
            BinaryPrimitives.WriteInt32LittleEndian(target.Slice(4, 4), input.B);
        }

        public static PairOfInt32 ReadFromSpan(ReadOnlySpan<byte> source)
        {
            Int32 a = BinaryPrimitives.ReadInt32LittleEndian(source.Slice(0, 4));
            Int32 b = BinaryPrimitives.ReadInt32LittleEndian(source.Slice(4, 4));
            return new PairOfInt32(a, b);
        }

        public static void WriteToSpan(Span<byte> target, in PairOfInt32 input)
        {
            BinaryPrimitives.WriteInt32LittleEndian(target.Slice(0, 4), input.A);
            BinaryPrimitives.WriteInt32LittleEndian(target.Slice(4, 4), input.B);
        }
    }
}
