using Shouldly;
using System;
using System.Linq;
using Xunit;

namespace DataFac.Memory.Tests
{
#if NET6_0_OR_GREATER
    public class CodecRegressionTests_Half
    {
        private static Half GetTestValue(string input)
        {
            return input switch
            {
                "max" => Half.MaxValue,
                "min" => Half.MinValue,
                "nan" => Half.NaN,
                "eps" => Half.Epsilon,
                "+inf" => Half.PositiveInfinity,
                "-inf" => Half.PositiveInfinity,
#if NET7_0_OR_GREATER
                "e" => Half.E,
                "pi" => Half.Pi,
                "tau" => Half.Tau,
#endif
                _ => Half.Parse(input),
            };
        }

        [Theory]
        [InlineData("1", "3C-00")]
        [InlineData("0", "00-00")]
        [InlineData("-1", "BC-00")]
        [InlineData("max", "7B-FF")]
        [InlineData("min", "FB-FF")]
        [InlineData("eps", "00-01")]
        [InlineData("+inf", "7C-00")]
        [InlineData("-inf", "7C-00")]
        [InlineData("nan", "FE-00")]
#if NET7_0_OR_GREATER
        [InlineData("e", "41-70")]
        [InlineData("pi", "42-48")]
        [InlineData("tau", "46-48")]
#endif
        public void Roundtrip_Half_BE(string valueText, string expectedBytes)
        {
            Half value = GetTestValue(valueText);
            Span<byte> buffer = stackalloc byte[2];
            DataFac.Memory.Codec_Half_BE.WriteToSpan(buffer, value);
            string.Join("-", buffer.ToArray().Select(b => b.ToString("X2"))).ShouldBe(expectedBytes);
            Half copy = DataFac.Memory.Codec_Half_BE.ReadFromSpan(buffer);
            copy.ShouldBe(value);
        }

        [Theory]
        [InlineData("1", "00-3C")]
        [InlineData("0", "00-00")]
        [InlineData("-1", "00-BC")]
        [InlineData("max", "FF-7B")]
        [InlineData("min", "FF-FB")]
        [InlineData("eps", "01-00")]
        [InlineData("+inf", "00-7C")]
        [InlineData("-inf", "00-7C")]
        [InlineData("nan", "00-FE")]
#if NET7_0_OR_GREATER
        [InlineData("e", "70-41")]
        [InlineData("pi", "48-42")]
        [InlineData("tau", "48-46")]
#endif
        public void Roundtrip_Half_LE(string valueText, string expectedBytes)
        {
            Half value = GetTestValue(valueText);
            Span<byte> buffer = stackalloc byte[2];
            DataFac.Memory.Codec_Half_LE.WriteToSpan(buffer, value);
            string.Join("-", buffer.ToArray().Select(b => b.ToString("X2"))).ShouldBe(expectedBytes);
            Half copy = DataFac.Memory.Codec_Half_LE.ReadFromSpan(buffer);
            copy.ShouldBe(value);
        }

    }
#endif
}
