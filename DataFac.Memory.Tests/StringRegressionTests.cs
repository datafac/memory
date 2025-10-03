using Shouldly;
using System;
using System.Linq;
using Xunit;

namespace DataFac.Memory.Tests
{
    public class StringRegressionTests
    {
        private void Roundtrip_Block<T>(in string value, string expectedBytes, bool shouldFail = false) where T : struct, IMemBlock
        {
            T block = default;
            int size = UnsafeHelpers.BlockHelper.BlockSize<T>();

            if (shouldFail)
            {
                try
                {
                    block.UTF8String = value;
                }
                catch (ArgumentException e)
                {
                    e.Message.ShouldStartWith("The output byte buffer is too small");
                    return;
                }
            }
            else
            {
                block.UTF8String = value;
            }

            // emit
            Span<byte> buffer = stackalloc byte[size];
            bool emitOk = block.TryWrite(buffer);
            emitOk.ShouldBeTrue();

            // check bytes
            string expected = string.Join("-", buffer.ToArray().Select(b => b.ToString("X2")));
            expected.ShouldStartWith(expectedBytes);
            expected.Length.ShouldBe(size * 3 - 1);

            // load
            bool loadOk = block.TryRead(buffer);
            loadOk.ShouldBeTrue();

            // compare values
            string? copy = block.UTF8String;
            copy.ShouldBe(value);
        }

        [Theory]
        [InlineData("", "00")]
        [InlineData("a", "??", true)]
        public void Roundtrip_BlockB001(in string value, string expectedBytes, bool shouldFail = false) => Roundtrip_Block<BlockB001>(value, expectedBytes, shouldFail);

        [Theory]
        [InlineData("", "00-00")]
        [InlineData("a", "01-61")]
        [InlineData("ab", "??", true)]
        public void Roundtrip_BlockB002(in string value, string expectedBytes, bool shouldFail = false) => Roundtrip_Block<BlockB002>(value, expectedBytes, shouldFail);

        [Theory]
        [InlineData("", "00-00-00-00")]
        [InlineData("abc", "03-61-62-63")]
        [InlineData("abcd", "??", true)]
        public void Roundtrip_BlockB004(in string value, string expectedBytes, bool shouldFail = false) => Roundtrip_Block<BlockB004>(value, expectedBytes, shouldFail);

        [Theory]
        [InlineData("", "00-00-00-00-00-00-00-00")]
        [InlineData("abcdefg", "07-61-62-63-64-65-66-67")]
        [InlineData("abcdefgh", "??", true)]
        public void Roundtrip_BlockB008(in string value, string expectedBytes, bool shouldFail = false) => Roundtrip_Block<BlockB008>(value, expectedBytes, shouldFail);

        [Theory]
        [InlineData("", "00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00")]
        [InlineData("abcdefghijklmno", "0F-61-62-63-64-65-66-67-68-69-6A-6B-6C-6D-6E-6F")]
        [InlineData("abcdefghijklmnop", "??", true)]
        public void Roundtrip_BlockB016(in string value, string expectedBytes, bool shouldFail = false) => Roundtrip_Block<BlockB016>(value, expectedBytes, shouldFail);

        [Theory]
        [InlineData("", "00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00")]
        [InlineData("abcdefghijklmnopqrstuvwxyz01234", "1F-61-62-63-64-65-66-67-68-69-6A-6B-6C-6D-6E-6F-70-71-72-73-74-75-76-77-78-79-7A-30-31-32-33-34")]
        [InlineData("abcdefghijklmnopqrstuvwxyz012345", "??", true)]
        public void Roundtrip_BlockB032(in string value, string expectedBytes, bool shouldFail = false) => Roundtrip_Block<BlockB032>(value, expectedBytes, shouldFail);

        public enum StringKind
        {
            Empty,
            OneChar,
            Maximum,
            Oversize
        }

        private void Roundtrip_LargeBlock<T>(StringKind kind, string expectedStartsWith) where T : struct, IMemBlock
        {
            T block = default;
            int size = UnsafeHelpers.BlockHelper.BlockSize<T>();
            string value = kind switch
            {
                StringKind.Empty => string.Empty,
                StringKind.OneChar => "a",
                StringKind.Maximum => new string('z', size <= 256 ? (size - 1) : (size - 2)),
                _ => new string('?', size)
            };

            if (kind == StringKind.Oversize)
            {
                try
                {
                    block.UTF8String = value;
                }
                catch (ArgumentException e)
                {
                    e.Message.ShouldStartWith("The output byte buffer is too small");
                    return;
                }
            }
            else
            {
                block.UTF8String = value;
            }

            // emit
            Span<byte> buffer = stackalloc byte[size];
            bool emitOk = block.TryWrite(buffer);
            emitOk.ShouldBeTrue();

            // check bytes
            string expected = string.Join("-", buffer.ToArray().Select(b => b.ToString("X2")));
            expected.ShouldStartWith(expectedStartsWith);
            expected.Length.ShouldBe(size * 3 - 1);

            // load
            bool loadOk = block.TryRead(buffer);
            loadOk.ShouldBeTrue();

            // compare values
            string? copy = block.UTF8String;
            copy.ShouldBe(value);
        }

        [Theory]
        [InlineData(StringKind.Empty, "00-00-00-")]
        [InlineData(StringKind.OneChar, "01-61-00-")]
        [InlineData(StringKind.Maximum, "3F-7A-7A-")]
        [InlineData(StringKind.Oversize, "??")]
        public void Roundtrip_BlockB064(StringKind kind, string expectedStartsWith) => Roundtrip_LargeBlock<BlockB064>(kind, expectedStartsWith);

        [Theory]
        [InlineData(StringKind.Empty, "00-00-00-")]
        [InlineData(StringKind.OneChar, "01-61-00-")]
        [InlineData(StringKind.Maximum, "7F-7A-7A-")]
        [InlineData(StringKind.Oversize, "??")]
        public void Roundtrip_BlockB128(StringKind kind, string expectedStartsWith) => Roundtrip_LargeBlock<BlockB128>(kind, expectedStartsWith);

        [Theory]
        [InlineData(StringKind.Empty, "00-00-00-")]
        [InlineData(StringKind.OneChar, "01-61-00-")]
        [InlineData(StringKind.Maximum, "FF-7A-7A-")]
        [InlineData(StringKind.Oversize, "??")]
        public void Roundtrip_BlockB256(StringKind kind, string expectedStartsWith) => Roundtrip_LargeBlock<BlockB256>(kind, expectedStartsWith);

        [Theory]
        [InlineData(StringKind.Empty, "00-00-00-00-")]
        [InlineData(StringKind.OneChar, "01-00-61-00-")]
        [InlineData(StringKind.Maximum, "FE-01-7A-7A-")]
        [InlineData(StringKind.Oversize, "??")]
        public void Roundtrip_BlockB512(StringKind kind, string expectedStartsWith) => Roundtrip_LargeBlock<BlockB512>(kind, expectedStartsWith);

        [Theory]
        [InlineData(StringKind.Empty, "00-00-00-00-")]
        [InlineData(StringKind.OneChar, "01-00-61-00-")]
        [InlineData(StringKind.Maximum, "FE-03-7A-7A-")]
        [InlineData(StringKind.Oversize, "??")]
        public void Roundtrip_BlockK001(StringKind kind, string expectedStartsWith) => Roundtrip_LargeBlock<BlockK001>(kind, expectedStartsWith);

        [Theory]
        [InlineData(StringKind.Empty, "00-00-00-00-")]
        [InlineData(StringKind.OneChar, "01-00-61-00-")]
        [InlineData(StringKind.Maximum, "FE-07-7A-7A-")]
        [InlineData(StringKind.Oversize, "??")]
        public void Roundtrip_BlockK002(StringKind kind, string expectedStartsWith) => Roundtrip_LargeBlock<BlockK002>(kind, expectedStartsWith);

        [Theory]
        [InlineData(StringKind.Empty, "00-00-00-00-")]
        [InlineData(StringKind.OneChar, "01-00-61-00-")]
        [InlineData(StringKind.Maximum, "FE-0F-7A-7A-")]
        [InlineData(StringKind.Oversize, "??")]
        public void Roundtrip_BlockK004(StringKind kind, string expectedStartsWith) => Roundtrip_LargeBlock<BlockK004>(kind, expectedStartsWith);

        [Theory]
        [InlineData(StringKind.Empty, "00-00-00-00-")]
        [InlineData(StringKind.OneChar, "01-00-61-00-")]
        [InlineData(StringKind.Maximum, "FE-1F-7A-7A-")]
        [InlineData(StringKind.Oversize, "??")]
        public void Roundtrip_BlockK008(StringKind kind, string expectedStartsWith) => Roundtrip_LargeBlock<BlockK008>(kind, expectedStartsWith);

    }
}