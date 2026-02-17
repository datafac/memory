using System;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;

namespace DataFac.Memory
{
    public sealed class Codec_Single_BE : Codec_Base<Single>
#if NET7_0_OR_GREATER
    , ISpanCodec<Single>
#endif
    {
        private Codec_Single_BE() { }
        public static Codec_Single_BE Instance { get; } = new Codec_Single_BE();
        public override Single OnRead(ReadOnlySpan<byte> source)
        {
            Int32 value = BinaryPrimitives.ReadInt32BigEndian(source);
            return Unsafe.As<int, float>(ref value);
        }

        public override void OnWrite(Span<byte> target, in Single input)
        {
            float input2 = input;
            int value = Unsafe.As<float, int>(ref input2);
            BinaryPrimitives.WriteInt32BigEndian(target, value);
        }

        public static Single ReadFromSpan(ReadOnlySpan<byte> source)
        {
            Int32 value = BinaryPrimitives.ReadInt32BigEndian(source);
            return Unsafe.As<int, float>(ref value);
        }

        public static void WriteToSpan(Span<byte> target, in Single input)
        {
            float input2 = input;
            int value = Unsafe.As<float, int>(ref input2);
            BinaryPrimitives.WriteInt32BigEndian(target, value);
        }
    }
}
