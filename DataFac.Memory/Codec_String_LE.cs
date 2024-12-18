using DataFac.UnsafeHelpers;
using System;

namespace DataFac.Memory
{
    public sealed class Codec_String_LE : ITypedFieldCodec<String>
#if NET7_0_OR_GREATER
    , ISpanCodec<String>
#endif
    {
        private Codec_String_LE() { }
        public static Codec_String_LE Instance { get; } = new Codec_String_LE();

        public String ReadFrom(ReadOnlySpan<byte> source)
        {
            return BlockHelper.GetStringFromSpan(source);
        }

        public void WriteTo(Span<byte> target, in String? input)
        {
            BlockHelper.SetStringIntoSpan(target, input);
        }

        object? IFieldCodec.ReadObject(ReadOnlySpan<byte> source)
        {
            return BlockHelper.GetStringFromSpan(source);
        }

        void IFieldCodec.WriteObject(Span<byte> target, object? input)
        {
            if (input is string value)
                BlockHelper.SetStringIntoSpan(target, value);
            else
                BlockHelper.SetStringIntoSpan(target, null);
        }

        public static String ReadFromSpan(ReadOnlySpan<byte> source)
        {
            return BlockHelper.GetStringFromSpan(source);
        }

        public static void WriteToSpan(Span<byte> target, in String? input)
        {
            BlockHelper.SetStringIntoSpan(target, input);
        }
    }
}
