﻿using Shouldly;
using System;
using System.Linq;
using Xunit;

namespace DataFac.Memory.Tests
{
    public class CodecRegressionTests_Decimal
    {
        private static Decimal GetTestValue(string input)
        {
            return input switch
            {
                "max" => Decimal.MaxValue,
                "min" => Decimal.MinValue,
                _ => Decimal.Parse(input),
            };
        }
        [Theory]
        [InlineData("1", "00-00-00-01-00-00-00-00-00-00-00-00-00-00-00-00")]
        [InlineData("0", "00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00")]
        [InlineData("-1", "00-00-00-01-00-00-00-00-00-00-00-00-80-00-00-00")]
        [InlineData("max", "FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-00-00-00-00")]
        [InlineData("min", "FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-80-00-00-00")]
        public void Roundtrip_Decimal_BE(string input, string expectedBytes)
        {
            Decimal value = GetTestValue(input);
            Span<byte> buffer = stackalloc byte[16];
#if NET7_0_OR_GREATER
            DataFac.Memory.Codec_Decimal_BE.WriteToSpan(buffer, value);
#else
            DataFac.Memory.Codec_Decimal_BE.Instance.WriteTo(buffer, value);
#endif
            string.Join("-", buffer.ToArray().Select(b => b.ToString("X2"))).ShouldBe(expectedBytes);
#if NET7_0_OR_GREATER
            Decimal copy = DataFac.Memory.Codec_Decimal_BE.ReadFromSpan(buffer);
#else
            Decimal copy = DataFac.Memory.Codec_Decimal_BE.Instance.ReadFrom(buffer);
#endif
            copy.ShouldBe(value);
        }

        [Theory]
        [InlineData("1", "01-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00")]
        [InlineData("0", "00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00")]
        [InlineData("-1", "01-00-00-00-00-00-00-00-00-00-00-00-00-00-00-80")]
        [InlineData("max", "FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-00-00-00-00")]
        [InlineData("min", "FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-00-00-00-80")]
        public void Roundtrip_Decimal_LE(string input, string expectedBytes)
        {
            Decimal value = GetTestValue(input);
            Span<byte> buffer = stackalloc byte[16];
#if NET7_0_OR_GREATER
            DataFac.Memory.Codec_Decimal_LE.WriteToSpan(buffer, value);
#else
            DataFac.Memory.Codec_Decimal_LE.Instance.WriteTo(buffer, value);
#endif
            string.Join("-", buffer.ToArray().Select(b => b.ToString("X2"))).ShouldBe(expectedBytes);
#if NET7_0_OR_GREATER
            Decimal copy = DataFac.Memory.Codec_Decimal_LE.ReadFromSpan(buffer);
#else
            Decimal copy = DataFac.Memory.Codec_Decimal_LE.Instance.ReadFrom(buffer);
#endif
            copy.ShouldBe(value);
        }

    }
}
