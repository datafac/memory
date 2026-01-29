using Shouldly;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DataFac.Memory.Tests
{
    public class ReadOnlySequenceBuilderTests
    {
        [Fact]
        public void ConstructEmpty()
        {
            var builder = new ReadOnlySequenceBuilder<byte>();
            var sequence = builder.Build();
            sequence.IsEmpty.ShouldBeTrue();
        }

        [Fact]
        public void ConstructOneSegmentA()
        {
            var expectedData = new byte[] { 1, 2, 3, 4, 5, 6 };
            var builder = new ReadOnlySequenceBuilder<byte>();
            builder = builder.Append(new byte[] { 1, 2, 3, 4, 5, 6 });
            var sequence = builder.Build();
            sequence.IsSingleSegment.ShouldBeTrue();
            sequence.Length.ShouldBe(expectedData.LongLength);
            sequence.ToArray().ShouldBeEquivalentTo(expectedData);
        }

        [Fact]
        public void ConstructOneSegmentB()
        {
            var expectedData = new byte[] { 1, 2, 3, 4, 5, 6 };
            var builder = new ReadOnlySequenceBuilder<byte>(new byte[] { 1, 2, 3, 4, 5, 6 });
            var sequence = builder.Build();
            sequence.IsSingleSegment.ShouldBeTrue();
            sequence.Length.ShouldBe(expectedData.LongLength);
            sequence.ToArray().ShouldBeEquivalentTo(expectedData);
        }

        [Fact]
        public void ConstructTwoSegmentsA()
        {
            var expectedData = new byte[] { 1, 2, 3, 4, 5, 6 };
            var builder = new ReadOnlySequenceBuilder<byte>();
            builder = builder.Append(new byte[] { 1, 2, 3 });
            builder = builder.Append(new byte[] { 4, 5, 6 });
            var sequence = builder.Build();
            sequence.IsSingleSegment.ShouldBeFalse();
            sequence.Length.ShouldBe(expectedData.LongLength);
            sequence.ToArray().ShouldBeEquivalentTo(expectedData);
        }

        [Fact]
        public void ConstructTwoSegmentsB()
        {
            var expectedData = new byte[] { 1, 2, 3, 4, 5, 6 };
            var builder = new ReadOnlySequenceBuilder<byte>(new byte[] { 1, 2, 3 }, new byte[] { 4, 5, 6 });
            var sequence = builder.Build();
            sequence.IsSingleSegment.ShouldBeFalse();
            sequence.Length.ShouldBe(expectedData.LongLength);
            sequence.ToArray().ShouldBeEquivalentTo(expectedData);
        }

        [Fact]
        public void ConstructMultiSegmentA()
        {
            var expectedData = new byte[] { 1, 2, 3, 4, 5, 6 };
            var builder = new ReadOnlySequenceBuilder<byte>(new byte[] { 1, 2 }, new byte[] { 3, 4 }, new byte[] { 5, 6 });
            var sequence = builder.Build();
            sequence.IsSingleSegment.ShouldBeFalse();
            sequence.Length.ShouldBe(expectedData.LongLength);
            sequence.ToArray().ShouldBeEquivalentTo(expectedData);
        }
        [Fact]
        public void ConstructMultiSegmentB()
        {
            var expectedData = new byte[] { 1, 2, 3, 4, 5, 6 };
            var builder = new ReadOnlySequenceBuilder<byte>(new List<ReadOnlyMemory<byte>>() { new byte[] { 1, 2 }, new byte[] { 3, 4 }, new byte[] { 5, 6 } });
            var sequence = builder.Build();
            sequence.IsSingleSegment.ShouldBeFalse();
            sequence.Length.ShouldBe(expectedData.LongLength);
            sequence.ToArray().ShouldBeEquivalentTo(expectedData);
        }
    }
}