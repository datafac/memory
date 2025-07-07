using System;
using System.Buffers.Binary;

namespace DataFac.Memory
{
    public sealed class Codec_PairOfInt16_LE : Codec_Base<PairOfInt16>
#if NET7_0_OR_GREATER
    , ISpanCodec<PairOfInt16>
#endif
    {
        private Codec_PairOfInt16_LE() { }
        public static Codec_PairOfInt16_LE Instance { get; } = new Codec_PairOfInt16_LE();

        public override PairOfInt16 OnRead(ReadOnlySpan<byte> source)
        {
            Int16 first = BinaryPrimitives.ReadInt16LittleEndian(source.Slice(0, 2));
            Int16 second = BinaryPrimitives.ReadInt16LittleEndian(source.Slice(2, 2));
            return new PairOfInt16(first, second);
        }

        public override void OnWrite(Span<byte> target, in PairOfInt16 input)
        {
            BinaryPrimitives.WriteInt16LittleEndian(target.Slice(0, 2), input.A);
            BinaryPrimitives.WriteInt16LittleEndian(target.Slice(2, 2), input.B);
        }

        public static PairOfInt16 ReadFromSpan(ReadOnlySpan<byte> source)
        {
            Int16 first = BinaryPrimitives.ReadInt16LittleEndian(source.Slice(0, 2));
            Int16 second = BinaryPrimitives.ReadInt16LittleEndian(source.Slice(2, 2));
            return new PairOfInt16(first, second);
        }

        public static void WriteToSpan(Span<byte> target, in PairOfInt16 input)
        {
            BinaryPrimitives.WriteInt16LittleEndian(target.Slice(0, 2), input.A);
            BinaryPrimitives.WriteInt16LittleEndian(target.Slice(2, 2), input.B);
        }
    }
}
