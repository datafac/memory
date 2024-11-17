using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace DataFac.Memory.Tests
{
    public class CodecRegressionTests_Boolean
    {
        [Theory]
        [InlineData(false, "00")]
        [InlineData(true, "01")]
        public void Roundtrip_Boolean_BE(in Boolean value, string expectedBytes)
        {
            Span<byte> buffer = stackalloc byte[1];
#if NET7_0_OR_GREATER
            DataFac.Memory.Codec_Boolean_BE.WriteToSpan(buffer, value);
#else
            DataFac.Memory.Codec_Boolean_BE.Instance.WriteTo(buffer, value);
#endif
            string.Join("-", buffer.ToArray().Select(b => b.ToString("X2"))).Should().Be(expectedBytes);
#if NET7_0_OR_GREATER
            Boolean copy = DataFac.Memory.Codec_Boolean_BE.ReadFromSpan(buffer);
#else
            Boolean copy = DataFac.Memory.Codec_Boolean_BE.Instance.ReadFrom(buffer);
#endif
            copy.Should().Be(value);
        }

        [Theory]
        [InlineData(false, "00")]
        [InlineData(true, "01")]
        public void Roundtrip_Boolean_LE(in Boolean value, string expectedBytes)
        {
            Span<byte> buffer = stackalloc byte[1];
#if NET7_0_OR_GREATER
            DataFac.Memory.Codec_Boolean_LE.WriteToSpan(buffer, value);
#else
            DataFac.Memory.Codec_Boolean_LE.Instance.WriteTo(buffer, value);
#endif
            string.Join("-", buffer.ToArray().Select(b => b.ToString("X2"))).Should().Be(expectedBytes);
#if NET7_0_OR_GREATER
            Boolean copy = DataFac.Memory.Codec_Boolean_LE.ReadFromSpan(buffer);
#else
            Boolean copy = DataFac.Memory.Codec_Boolean_LE.Instance.ReadFrom(buffer);
#endif
            copy.Should().Be(value);
        }
    }
}
