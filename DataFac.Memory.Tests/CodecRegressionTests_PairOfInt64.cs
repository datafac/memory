using Shouldly;
using System;
using System.Linq;
using Xunit;

namespace DataFac.Memory.Tests
{
    public class CodecRegressionTests_PairOfInt64
    {
        [Theory]
        [InlineData(1L, 2L,  "00-00-00-00-00-00-00-01-00-00-00-00-00-00-00-02")]
        [InlineData(0L, 1L,  "00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-01")]
        [InlineData(-1L, 0L, "FF-FF-FF-FF-FF-FF-FF-FF-00-00-00-00-00-00-00-00")]
        [InlineData(Int64.MaxValue-1,Int64.MaxValue, "7F-FF-FF-FF-FF-FF-FF-FE-7F-FF-FF-FF-FF-FF-FF-FF")]
        [InlineData(Int64.MinValue, Int64.MinValue+1, "80-00-00-00-00-00-00-00-80-00-00-00-00-00-00-01")]
        public void Roundtrip_PairOfInt64_BE(in Int64 a, in Int64 b, string expectedBytes)
        {
            PairOfInt64 orig = new PairOfInt64(a, b);
            Span<byte> buffer = stackalloc byte[16];
            Codec_PairOfInt64_BE.WriteToSpan(buffer, orig);
            string.Join("-", buffer.ToArray().Select(b => b.ToString("X2"))).ShouldBe(expectedBytes);
            PairOfInt64 copy1 = Codec_PairOfInt64_BE.ReadFromSpan(buffer);
            copy1.ShouldBe(orig);
            BlockB016 block = new BlockB016();
            block.PairOfInt64BE = orig;
            PairOfInt64 copy2 = block.PairOfInt64BE;
            copy2.ShouldBe(orig);
            block.TryWrite(buffer).ShouldBeTrue();
            string.Join("-", buffer.ToArray().Select(b => b.ToString("X2"))).ShouldBe(expectedBytes);
            block.TryRead(buffer).ShouldBeTrue();
            PairOfInt64 copy3 = block.PairOfInt64BE;
            copy3.ShouldBe(orig);
        }

        [Theory]
        [InlineData(1L, 2L, "01-00-00-00-00-00-00-00-02-00-00-00-00-00-00-00")]
        [InlineData(0L, 1L, "00-00-00-00-00-00-00-00-01-00-00-00-00-00-00-00")]
        [InlineData(-1L, 0L, "FF-FF-FF-FF-FF-FF-FF-FF-00-00-00-00-00-00-00-00")]
        [InlineData(Int64.MaxValue - 1, Int64.MaxValue, "FE-FF-FF-FF-FF-FF-FF-7F-FF-FF-FF-FF-FF-FF-FF-7F")]
        [InlineData(Int64.MinValue, Int64.MinValue + 1, "00-00-00-00-00-00-00-80-01-00-00-00-00-00-00-80")]
        public void Roundtrip_PairOfInt64_LE(in Int64 a, in Int64 b, string expectedBytes)
        {
            PairOfInt64 orig = new PairOfInt64(a, b);
            Span<byte> buffer = stackalloc byte[16];
            Codec_PairOfInt64_LE.WriteToSpan(buffer, orig);
            string.Join("-", buffer.ToArray().Select(b => b.ToString("X2"))).ShouldBe(expectedBytes);
            PairOfInt64 copy1 = Codec_PairOfInt64_LE.ReadFromSpan(buffer);
            copy1.ShouldBe(orig);
            BlockB016 block = new BlockB016();
            block.PairOfInt64LE = orig;
            PairOfInt64 copy2 = block.PairOfInt64LE;
            copy2.ShouldBe(orig);
            block.TryWrite(buffer).ShouldBeTrue();
            string.Join("-", buffer.ToArray().Select(b => b.ToString("X2"))).ShouldBe(expectedBytes);
            block.TryRead(buffer).ShouldBeTrue();
            PairOfInt64 copy3 = block.PairOfInt64LE;
            copy3.ShouldBe(orig);
        }
    }
}
