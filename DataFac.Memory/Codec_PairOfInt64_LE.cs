using System;
using System.Buffers.Binary;

namespace DataFac.Memory
{
    public sealed class Codec_PairOfInt64_LE : Codec_Base<PairOfInt64>
#if NET7_0_OR_GREATER
    , ISpanCodec<PairOfInt64>
#endif
    {
        private Codec_PairOfInt64_LE() { }
        public static Codec_PairOfInt64_LE Instance { get; } = new Codec_PairOfInt64_LE();

        public override PairOfInt64 OnRead(ReadOnlySpan<byte> source)
        {
            long first = BinaryPrimitives.ReadInt64LittleEndian(source.Slice(0, 8));
            long second = BinaryPrimitives.ReadInt64LittleEndian(source.Slice(8, 8));
            return new PairOfInt64(first, second);
        }

        public override void OnWrite(Span<byte> target, in PairOfInt64 input)
        {
            BinaryPrimitives.WriteInt64LittleEndian(target.Slice(0, 8), input.A);
            BinaryPrimitives.WriteInt64LittleEndian(target.Slice(8, 8), input.B);
        }

        public static PairOfInt64 ReadFromSpan(ReadOnlySpan<byte> source)
        {
            long first = BinaryPrimitives.ReadInt64LittleEndian(source.Slice(0, 8));
            long second = BinaryPrimitives.ReadInt64LittleEndian(source.Slice(8, 8));
            return new PairOfInt64(first, second);
        }

        public static void WriteToSpan(Span<byte> target, in PairOfInt64 input)
        {
            BinaryPrimitives.WriteInt64LittleEndian(target.Slice(0, 8), input.A);
            BinaryPrimitives.WriteInt64LittleEndian(target.Slice(8, 8), input.B);
        }
    }
}
