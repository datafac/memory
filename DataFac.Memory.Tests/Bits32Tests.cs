using Shouldly;
using Xunit;

namespace DataFac.Memory.Tests
{
    public class Bits32Tests
    {
        [Theory]
        [InlineData(default(uint), "0x00000000")]
        [InlineData(1, "0x00000001")]
        [InlineData(uint.MaxValue, "0xFFFFFFFF")]
        public void InitBits(uint data, string expected)
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
        public void SetBits(uint init, int pos, bool value, uint expected)
        {
            Bits32 bits = (new Bits32(init)).SetBit(pos, value);
            bits.Data.ShouldBe(expected);
        }
    }
}