using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace DataFac.Memory.Tests
{
    public class CodecRegressionTests_String
    {
        [Theory]
        [InlineData("", "00-00-00-00-00-00-00-00")]
        [InlineData("a", "01-61-00-00-00-00-00-00")]
        [InlineData("abcdefg", "07-61-62-63-64-65-66-67")]
        public void Roundtrip_String_Small(in String value, string expectedBytes)
        {
            Span<byte> buffer = stackalloc byte[8];
#if NET7_0_OR_GREATER
            DataFac.Memory.Codec_String_LE.WriteToSpan(buffer, value);
#else
            DataFac.Memory.Codec_String_LE.Instance.WriteTo(buffer, value);
#endif
            string.Join("-", buffer.ToArray().Select(b => b.ToString("X2"))).Should().Be(expectedBytes);
#if NET7_0_OR_GREATER
            String copy = DataFac.Memory.Codec_String_LE.ReadFromSpan(buffer);
#else
            String copy = DataFac.Memory.Codec_String_LE.Instance.ReadFrom(buffer);
#endif
            copy.Should().Be(value);
        }

        [Theory]
        [InlineData(0, "00-00-00-00-")]
        [InlineData(255, "FF-00-61-61-")]
        [InlineData(256, "00-01-61-61-")]
        [InlineData(1022, "FE-03-61-61-")]
        public void Roundtrip_String_Large(int length, string expectedStartsWith)
        {
            Span<byte> buffer = new byte[1024];
            string value = new string('a', length);
#if NET7_0_OR_GREATER
            DataFac.Memory.Codec_String_LE.WriteToSpan(buffer, value);
#else
            DataFac.Memory.Codec_String_LE.Instance.WriteTo(buffer, value);
#endif
            string.Join("-", buffer.ToArray().Select(b => b.ToString("X2"))).Should().StartWith(expectedStartsWith);
#if NET7_0_OR_GREATER
            String copy = DataFac.Memory.Codec_String_LE.ReadFromSpan(buffer);
#else
            String copy = DataFac.Memory.Codec_String_LE.Instance.ReadFrom(buffer);
#endif
            copy.Should().Be(value);
        }

    }
}
