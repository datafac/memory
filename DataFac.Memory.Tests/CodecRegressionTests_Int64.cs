﻿using Shouldly;
using System;
using System.Linq;
using Xunit;

namespace DataFac.Memory.Tests
{

    public class CodecRegressionTests_Int64
    {
        [Theory]
        [InlineData(1L, "00-00-00-00-00-00-00-01")]
        [InlineData(0L, "00-00-00-00-00-00-00-00")]
        [InlineData(-1L, "FF-FF-FF-FF-FF-FF-FF-FF")]
        [InlineData(Int64.MaxValue, "7F-FF-FF-FF-FF-FF-FF-FF")]
        [InlineData(Int64.MinValue, "80-00-00-00-00-00-00-00")]
        public void Roundtrip_Int64_BE(in Int64 value, string expectedBytes)
        {
            Span<byte> buffer = stackalloc byte[8];
            Codec_Int64_BE.WriteToSpan(buffer, value);
            string.Join("-", buffer.ToArray().Select(b => b.ToString("X2"))).ShouldBe(expectedBytes);
            Int64 copy = Codec_Int64_BE.ReadFromSpan(buffer);
            copy.ShouldBe(value);
        }

        [Theory]
        [InlineData(1L, "01-00-00-00-00-00-00-00")]
        [InlineData(0L, "00-00-00-00-00-00-00-00")]
        [InlineData(-1L, "FF-FF-FF-FF-FF-FF-FF-FF")]
        [InlineData(Int64.MaxValue, "FF-FF-FF-FF-FF-FF-FF-7F")]
        [InlineData(Int64.MinValue, "00-00-00-00-00-00-00-80")]
        public void Roundtrip_Int64_LE(in Int64 value, string expectedBytes)
        {
            Span<byte> buffer = stackalloc byte[8];
            Codec_Int64_LE.WriteToSpan(buffer, value);
            string.Join("-", buffer.ToArray().Select(b => b.ToString("X2"))).ShouldBe(expectedBytes);
            Int64 copy = Codec_Int64_LE.ReadFromSpan(buffer);
            copy.ShouldBe(value);
        }

        [Theory]
        [InlineData(UInt64.MinValue, "00-00-00-00-00-00-00-00")]
        [InlineData(1UL, "00-00-00-00-00-00-00-01")]
        [InlineData(UInt64.MaxValue, "FF-FF-FF-FF-FF-FF-FF-FF")]
        public void Roundtrip_UInt64_BE(in UInt64 value, string expectedBytes)
        {
            Span<byte> buffer = stackalloc byte[8];
#if NET7_0_OR_GREATER
            DataFac.Memory.Codec_UInt64_BE.WriteToSpan(buffer, value);
#else
            DataFac.Memory.Codec_UInt64_BE.Instance.WriteTo(buffer, value);
#endif
            string.Join("-", buffer.ToArray().Select(b => b.ToString("X2"))).ShouldBe(expectedBytes);
#if NET7_0_OR_GREATER
            UInt64 copy = DataFac.Memory.Codec_UInt64_BE.ReadFromSpan(buffer);
#else
            UInt64 copy = DataFac.Memory.Codec_UInt64_BE.Instance.ReadFrom(buffer);
#endif
            copy.ShouldBe(value);
        }

        [Theory]
        [InlineData(UInt64.MinValue, "00-00-00-00-00-00-00-00")]
        [InlineData(1UL, "01-00-00-00-00-00-00-00")]
        [InlineData(UInt64.MaxValue, "FF-FF-FF-FF-FF-FF-FF-FF")]
        public void Roundtrip_UInt64_LE(in UInt64 value, string expectedBytes)
        {
            Span<byte> buffer = stackalloc byte[8];
#if NET7_0_OR_GREATER
            DataFac.Memory.Codec_UInt64_LE.WriteToSpan(buffer, value);
#else
            DataFac.Memory.Codec_UInt64_LE.Instance.WriteTo(buffer, value);
#endif
            string.Join("-", buffer.ToArray().Select(b => b.ToString("X2"))).ShouldBe(expectedBytes);
#if NET7_0_OR_GREATER
            UInt64 copy = DataFac.Memory.Codec_UInt64_LE.ReadFromSpan(buffer);
#else
            UInt64 copy = DataFac.Memory.Codec_UInt64_LE.Instance.ReadFrom(buffer);
#endif
            copy.ShouldBe(value);
        }
    }
}
