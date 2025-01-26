using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace DataFac.Memory.Tests
{
    public class VectorRegressionTests_Int32
    {
        [Fact]
        public void Block064_Int32_BE()
        {
            // set
            BlockB064 block = default;
            ReadOnlySpan<Int32> origValues = stackalloc Int32[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            block.SetInt32ArrayBE(origValues);

            var buffer = DataFac.UnsafeHelpers.BlockHelper.AsReadOnlySpan(ref block);
            string.Join("-", buffer.ToArray().Select(b => b.ToString("X2"))).Should().Be(
                "00-00-00-00-00-00-00-01-00-00-00-02-00-00-00-03-00-00-00-04-00-00-00-05-00-00-00-06-00-00-00-07-" +
                "00-00-00-08-00-00-00-09-00-00-00-0A-00-00-00-0B-00-00-00-0C-00-00-00-0D-00-00-00-0E-00-00-00-0F");

            //get
            Span<Int32> copyValues = stackalloc Int32[16];
            block.GetInt32ArrayBE(copyValues);
            copyValues.SequenceEqual(origValues).Should().BeTrue();
        }

        [Fact]
        public void Block064_Int32_LE()
        {
            // set
            BlockB064 block = default;
            ReadOnlySpan<Int32> origValues = stackalloc Int32[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            block.SetInt32ArrayLE(origValues);

            var buffer = DataFac.UnsafeHelpers.BlockHelper.AsReadOnlySpan(ref block);
            string.Join("-", buffer.ToArray().Select(b => b.ToString("X2"))).Should().Be(
                "00-00-00-00-01-00-00-00-02-00-00-00-03-00-00-00-04-00-00-00-05-00-00-00-06-00-00-00-07-00-00-00-" +
                "08-00-00-00-09-00-00-00-0A-00-00-00-0B-00-00-00-0C-00-00-00-0D-00-00-00-0E-00-00-00-0F-00-00-00");

            // get
            Span<Int32> copyValues = stackalloc Int32[16];
            block.GetInt32ArrayLE(copyValues);
            copyValues.SequenceEqual(origValues).Should().BeTrue();
        }

        [Fact]
        public void Block064_UInt32_BE()
        {
            // set
            BlockB064 block = default;
            ReadOnlySpan<UInt32> origValues = stackalloc UInt32[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            block.SetUInt32ArrayBE(origValues);

            var buffer = DataFac.UnsafeHelpers.BlockHelper.AsReadOnlySpan(ref block);
            string.Join("-", buffer.ToArray().Select(b => b.ToString("X2"))).Should().Be(
                "00-00-00-00-00-00-00-01-00-00-00-02-00-00-00-03-00-00-00-04-00-00-00-05-00-00-00-06-00-00-00-07-" +
                "00-00-00-08-00-00-00-09-00-00-00-0A-00-00-00-0B-00-00-00-0C-00-00-00-0D-00-00-00-0E-00-00-00-0F");

            //get
            Span<UInt32> copyValues = stackalloc UInt32[16];
            block.GetUInt32ArrayBE(copyValues);
            copyValues.SequenceEqual(origValues).Should().BeTrue();
        }

        [Fact]
        public void Block064_UInt32_LE()
        {
            // set
            BlockB064 block = default;
            ReadOnlySpan<UInt32> origValues = stackalloc UInt32[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            block.SetUInt32ArrayLE(origValues);

            var buffer = DataFac.UnsafeHelpers.BlockHelper.AsReadOnlySpan(ref block);
            string.Join("-", buffer.ToArray().Select(b => b.ToString("X2"))).Should().Be(
                "00-00-00-00-01-00-00-00-02-00-00-00-03-00-00-00-04-00-00-00-05-00-00-00-06-00-00-00-07-00-00-00-" +
                "08-00-00-00-09-00-00-00-0A-00-00-00-0B-00-00-00-0C-00-00-00-0D-00-00-00-0E-00-00-00-0F-00-00-00");

            // get
            Span<UInt32> copyValues = stackalloc UInt32[16];
            block.GetUInt32ArrayLE(copyValues);
            copyValues.SequenceEqual(origValues).Should().BeTrue();
        }

        [Fact]
        public void Block064_Int32_OverWrite()
        {
            // set
            BlockB064 block = default;
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                ReadOnlySpan<Int32> origValues = stackalloc Int32[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 };
                block.SetInt32ArrayLE(origValues);
            });
            ex.Message.Should().StartWith("Destination is too short.");

        }

        [Fact]
        public void Block064_Int32_PartialRead()
        {
            // set
            BlockB064 block = default;
            ReadOnlySpan<Int32> origValues = stackalloc Int32[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            block.SetInt32ArrayLE(origValues);

            // get
            ReadOnlySpan<Int32> expected = stackalloc Int32[] { 0, 1, 2, 3, 4, 5, 6, 7 };
            Span<Int32> copyValues = stackalloc Int32[8];
            block.GetInt32ArrayLE(copyValues);
            copyValues.SequenceEqual(expected).Should().BeTrue();
        }

        [Fact]
        public void Block064_Int32_PartialWrite()
        {
            // set
            BlockB064 block = default;
            ReadOnlySpan<Int32> origValues = stackalloc Int32[] { 0, 1, 2, 3, 4, 5, 6, 7 };
            block.SetInt32ArrayLE(origValues);

            // get
            ReadOnlySpan<Int32> expected = stackalloc Int32[] { 0, 1, 2, 3, 4, 5, 6, 7, 0, 0, 0, 0, 0, 0, 0, 0 };
            Span<Int32> copyValues = stackalloc Int32[16];
            block.GetInt32ArrayLE(copyValues);
            copyValues.SequenceEqual(expected).Should().BeTrue();
        }
    }
}
