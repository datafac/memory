using Shouldly;
using System;
using System.Linq;
using Xunit;

namespace DataFac.Memory.Tests
{
    public class CodecRegressionTests_PairOfInt16
    {
        [Theory]
        [InlineData((short)1, (short)2, "00-01-00-02")]
        [InlineData((short)0, (short)1, "00-00-00-01")]
        [InlineData((short)(-1), (short)0, "FF-FF-00-00")]
        [InlineData((short)(Int16.MaxValue - 1), Int16.MaxValue, "7F-FE-7F-FF")]
        [InlineData(Int16.MinValue, (short)(Int16.MinValue + 1), "80-00-80-01")]
        public void Roundtrip_PairOfInt16_BE(in Int16 a, in Int16 b, string expectedBytes)
        {
            PairOfInt16 orig = new PairOfInt16(a, b);
            Span<byte> buffer = stackalloc byte[4];
            Codec_PairOfInt16_BE.WriteToSpan(buffer, orig);
            string.Join("-", buffer.ToArray().Select(b => b.ToString("X2"))).ShouldBe(expectedBytes);
            PairOfInt16 copy1 = Codec_PairOfInt16_BE.ReadFromSpan(buffer);
            copy1.ShouldBe(orig);
            BlockB004 block = new BlockB004();
            block.PairOfInt16BE = orig;
            PairOfInt16 copy2 = block.PairOfInt16BE;
            copy2.ShouldBe(orig);
            block.TryWrite(buffer).ShouldBeTrue();
            string.Join("-", buffer.ToArray().Select(b => b.ToString("X2"))).ShouldBe(expectedBytes);
            block.TryRead(buffer).ShouldBeTrue();
            PairOfInt16 copy3 = block.PairOfInt16BE;
            copy3.ShouldBe(orig);
        }

        [Theory]
        [InlineData((short)1, (short)2, "01-00-02-00")]
        [InlineData((short)0, (short)1, "00-00-01-00")]
        [InlineData((short)(-1), (short)0, "FF-FF-00-00")]
        [InlineData((short)(Int16.MaxValue - 1), Int16.MaxValue, "FE-7F-FF-7F")]
        [InlineData(Int16.MinValue, (short)(Int16.MinValue + 1), "00-80-01-80")]
        public void Roundtrip_PairOfInt16_LE(in Int16 a, in Int16 b, string expectedBytes)
        {
            PairOfInt16 orig = new PairOfInt16(a, b);
            Span<byte> buffer = stackalloc byte[4];
            Codec_PairOfInt16_LE.WriteToSpan(buffer, orig);
            string.Join("-", buffer.ToArray().Select(b => b.ToString("X2"))).ShouldBe(expectedBytes);
            PairOfInt16 copy1 = Codec_PairOfInt16_LE.ReadFromSpan(buffer);
            copy1.ShouldBe(orig);
            BlockB004 block = new BlockB004();
            block.PairOfInt16LE = orig;
            PairOfInt16 copy2 = block.PairOfInt16LE;
            copy2.ShouldBe(orig);
            block.TryWrite(buffer).ShouldBeTrue();
            string.Join("-", buffer.ToArray().Select(b => b.ToString("X2"))).ShouldBe(expectedBytes);
            block.TryRead(buffer).ShouldBeTrue();
            PairOfInt16 copy3 = block.PairOfInt16LE;
            copy3.ShouldBe(orig);
        }
    }
}
