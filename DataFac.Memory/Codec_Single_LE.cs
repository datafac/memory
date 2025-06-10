﻿using System;
using System.Buffers.Binary;

namespace DataFac.Memory
{
    public sealed class Codec_Single_LE : Codec_Base<Single>
#if NET7_0_OR_GREATER
    , ISpanCodec<Single>
#endif
    {
        private Codec_Single_LE() { }
        public static Codec_Single_LE Instance { get; } = new Codec_Single_LE();
        public override Single OnRead(ReadOnlySpan<byte> source)
        {
#if NET6_0_OR_GREATER
            return BinaryPrimitives.ReadSingleLittleEndian(source);
#else
            Int32 value = BinaryPrimitives.ReadInt32LittleEndian(source);
            var overlap = new Convert32(value);
            return overlap.FloatValue;
#endif
        }

        public override void OnWrite(Span<byte> target, in Single input)
        {
#if NET6_0_OR_GREATER
            BinaryPrimitives.WriteSingleLittleEndian(target, input);
#else
            var overlap = new Convert32(input);
            int value = overlap.Int32Value;
            BinaryPrimitives.WriteInt32LittleEndian(target, value);
#endif
        }

        public static Single ReadFromSpan(ReadOnlySpan<byte> source)
        {
#if NET6_0_OR_GREATER
            return BinaryPrimitives.ReadSingleLittleEndian(source);
#else
            Int32 value = BinaryPrimitives.ReadInt32LittleEndian(source);
            var overlap = new Convert32(value);
            return overlap.FloatValue;
#endif
        }

        public static void WriteToSpan(Span<byte> target, in Single input)
        {
#if NET6_0_OR_GREATER
            BinaryPrimitives.WriteSingleLittleEndian(target, input);
#else
            var overlap = new Convert32(input);
            int value = overlap.Int32Value;
            BinaryPrimitives.WriteInt32LittleEndian(target, value);
#endif
        }
    }
}
