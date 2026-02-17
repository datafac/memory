using Shouldly;
using System;
using System.Linq;
using Xunit;

namespace DataFac.Memory.Tests;

public class CodecRegressionTests_QuadOfInt32
{
    [Theory]
    [InlineData(0, 1, 2, 3,    "00-00-00-00-00-00-00-01-00-00-00-02-00-00-00-03")]
    [InlineData(-3, -2, -1, 0, "FF-FF-FF-FD-FF-FF-FF-FE-FF-FF-FF-FF-00-00-00-00")]
    [InlineData(Int32.MinValue, Int32.MinValue + 1, Int32.MaxValue - 1, Int32.MaxValue, "80-00-00-00-80-00-00-01-7F-FF-FF-FE-7F-FF-FF-FF")]
    public void Roundtrip_QuadOfInt32_BE(in Int32 a, in Int32 b, in Int32 c, in Int32 d, string expectedBytes)
    {
        QuadOfInt32 orig = new QuadOfInt32(a, b, c, d);
        Span<byte> buffer = stackalloc byte[16];
        Codec_QuadOfInt32_BE.WriteToSpan(buffer, orig);
        string.Join("-", buffer.ToArray().Select(b => b.ToString("X2"))).ShouldBe(expectedBytes);
        QuadOfInt32 copy1 = Codec_QuadOfInt32_BE.ReadFromSpan(buffer);
        copy1.ShouldBe(orig);
        BlockB016 block = new BlockB016();
        block.QuadOfInt32BE = orig;
        QuadOfInt32 copy2 = block.QuadOfInt32BE;
        copy2.ShouldBe(orig);
        block.TryWrite(buffer).ShouldBeTrue();
        string.Join("-", buffer.ToArray().Select(b => b.ToString("X2"))).ShouldBe(expectedBytes);
        block.TryRead(buffer).ShouldBeTrue();
        QuadOfInt32 copy3 = block.QuadOfInt32BE;
        copy3.ShouldBe(orig);
    }

    [Theory]
    [InlineData(0, 1, 2, 3, "00-00-00-00-01-00-00-00-02-00-00-00-03-00-00-00")]
    [InlineData(-3, -2, -1, 0, "FD-FF-FF-FF-FE-FF-FF-FF-FF-FF-FF-FF-00-00-00-00")]
    [InlineData(Int32.MinValue, Int32.MinValue + 1, Int32.MaxValue - 1, Int32.MaxValue, "00-00-00-80-01-00-00-80-FE-FF-FF-7F-FF-FF-FF-7F")]
    public void Roundtrip_QuadOfInt32_LE(in Int32 a, in Int32 b, in Int32 c, in Int32 d, string expectedBytes)
    {
        QuadOfInt32 orig = new QuadOfInt32(a, b, c, d);
        Span<byte> buffer = stackalloc byte[16];
        Codec_QuadOfInt32_LE.WriteToSpan(buffer, orig);
        string.Join("-", buffer.ToArray().Select(b => b.ToString("X2"))).ShouldBe(expectedBytes);
        QuadOfInt32 copy1 = Codec_QuadOfInt32_LE.ReadFromSpan(buffer);
        copy1.ShouldBe(orig);
        BlockB016 block = new BlockB016();
        block.QuadOfInt32LE = orig;
        QuadOfInt32 copy2 = block.QuadOfInt32LE;
        copy2.ShouldBe(orig);
        block.TryWrite(buffer).ShouldBeTrue();
        string.Join("-", buffer.ToArray().Select(b => b.ToString("X2"))).ShouldBe(expectedBytes);
        block.TryRead(buffer).ShouldBeTrue();
        QuadOfInt32 copy3 = block.QuadOfInt32LE;
        copy3.ShouldBe(orig);
    }
}
