using Shouldly;
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
            DataFac.Memory.Codec_Boolean_BE.WriteToSpan(buffer, value);
            string.Join("-", buffer.ToArray().Select(b => b.ToString("X2"))).ShouldBe(expectedBytes);
            Boolean copy = DataFac.Memory.Codec_Boolean_BE.ReadFromSpan(buffer);
            copy.ShouldBe(value);
        }

        [Theory]
        [InlineData(false, "00")]
        [InlineData(true, "01")]
        public void Roundtrip_Boolean_LE(in Boolean value, string expectedBytes)
        {
            Span<byte> buffer = stackalloc byte[1];
            DataFac.Memory.Codec_Boolean_LE.WriteToSpan(buffer, value);
            string.Join("-", buffer.ToArray().Select(b => b.ToString("X2"))).ShouldBe(expectedBytes);
            Boolean copy = DataFac.Memory.Codec_Boolean_LE.ReadFromSpan(buffer);
            copy.ShouldBe(value);
        }
    }
}
