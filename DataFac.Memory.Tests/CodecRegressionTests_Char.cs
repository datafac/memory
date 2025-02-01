using Shouldly;
using System;
using System.Linq;
using Xunit;

namespace DataFac.Memory.Tests
{

    public class CodecRegressionTests_Char
    {
        [Theory]
        [InlineData(Char.MinValue, "00-00")]
        [InlineData((Char)32, "00-20")]
        [InlineData(Char.MaxValue, "FF-FF")]
        public void Roundtrip_Char_BE(in Char value, string expectedBytes)
        {
            Span<byte> buffer = stackalloc byte[2];
#if NET7_0_OR_GREATER
            DataFac.Memory.Codec_Char_BE.WriteToSpan(buffer, value);
#else
            DataFac.Memory.Codec_Char_BE.Instance.WriteTo(buffer, value);
#endif
            string.Join("-", buffer.ToArray().Select(b => b.ToString("X2"))).ShouldBe(expectedBytes);
#if NET7_0_OR_GREATER
            Char copy = DataFac.Memory.Codec_Char_BE.ReadFromSpan(buffer);
#else
            Char copy = DataFac.Memory.Codec_Char_BE.Instance.ReadFrom(buffer);
#endif
            copy.ShouldBe(value);
        }

        [Theory]
        [InlineData(Char.MinValue, "00-00")]
        [InlineData((Char)32, "20-00")]
        [InlineData(Char.MaxValue, "FF-FF")]
        public void Roundtrip_Char_LE(in Char value, string expectedBytes)
        {
            Span<byte> buffer = stackalloc byte[2];
#if NET7_0_OR_GREATER
            DataFac.Memory.Codec_Char_LE.WriteToSpan(buffer, value);
#else
            DataFac.Memory.Codec_Char_LE.Instance.WriteTo(buffer, value);
#endif
            string.Join("-", buffer.ToArray().Select(b => b.ToString("X2"))).ShouldBe(expectedBytes);
#if NET7_0_OR_GREATER
            Char copy = DataFac.Memory.Codec_Char_LE.ReadFromSpan(buffer);
#else
            Char copy = DataFac.Memory.Codec_Char_LE.Instance.ReadFrom(buffer);
#endif
            copy.ShouldBe(value);
        }

    }
}
