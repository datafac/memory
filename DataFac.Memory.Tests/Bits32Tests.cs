using Shouldly;
using System;
using Xunit;

namespace DataFac.Memory.Tests;

public class Bits32Tests
{
    [Theory]
    [InlineData(default(UInt32), "0x00000000")]
    [InlineData(1, "0x00000001")]
    [InlineData(UInt32.MaxValue, "0xFFFFFFFF")]
    public void InitBits(UInt32 data, string expected)
    {
        Bits32 bits = new Bits32(data);
        bits.ToString().ShouldBe(expected);
    }

    [Theory]
    [InlineData(0x00000000, 0,  true, 0x00000001)]
    [InlineData(0x00000001, 1,  true, 0x00000003)]
    [InlineData(0x00000003, 2,  true, 0x00000007)]
    [InlineData(0x00000007, 3,  true, 0x0000000F)]
    [InlineData(0x0000000F, 31, true, 0x8000000F)]
    [InlineData(0x8000000F, 30, true, 0xC000000F)]
    [InlineData(0xC000000F, 29, true, 0xE000000F)]
    [InlineData(0xE000000F, 28, true, 0xF000000F)]
    [InlineData(0xF000000F, 0, false, 0xF000000E)]
    [InlineData(0xF000000E, 1, false, 0xF000000C)]
    [InlineData(0xF000000C, 2, false, 0xF0000008)]
    [InlineData(0xF0000008, 3, false, 0xF0000000)]
    public void SetBits(UInt32 init, int pos, bool value, UInt32 expected)
    {
        Bits32 bits = (new Bits32(init)).SetBit(pos, value);
        bits.Data.ShouldBe(expected);
    }
}

public class Bits64Tests
{
    [Theory]
    [InlineData(default(UInt64), "0x00000000")]
    [InlineData(1, "0x00000001")]
    [InlineData(UInt64.MaxValue, "0xFFFFFFFFFFFFFFFF")]
    public void InitBits(UInt64 data, string expected)
    {
        Bits64 bits = new Bits64(data);
        bits.ToString().ShouldBe(expected);
    }

    [Theory]
    [InlineData(0x0000000000000000, 0, true, 0x0000000000000001)]
    [InlineData(0x0000000000000001, 1, true, 0x0000000000000003)]
    [InlineData(0x0000000000000003, 2, true, 0x0000000000000007)]
    [InlineData(0x0000000000000007, 3, true, 0x000000000000000F)]
    [InlineData(0x000000000000000F, 63, true, 0x800000000000000F)]
    [InlineData(0x800000000000000F, 62, true, 0xC00000000000000F)]
    [InlineData(0xC00000000000000F, 61, true, 0xE00000000000000F)]
    [InlineData(0xE00000000000000F, 60, true, 0xF00000000000000F)]
    [InlineData(0xF00000000000000F, 0, false, 0xF00000000000000E)]
    [InlineData(0xF00000000000000E, 1, false, 0xF00000000000000C)]
    [InlineData(0xF00000000000000C, 2, false, 0xF000000000000008)]
    [InlineData(0xF000000000000008, 3, false, 0xF000000000000000)]
    public void SetBits(UInt64 init, int pos, bool value, UInt64 expected)
    {
        Bits64 bits = (new Bits64(init)).SetBit(pos, value);
        bits.Data.ShouldBe(expected);
    }
}