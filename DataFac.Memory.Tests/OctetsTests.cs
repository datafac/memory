using Shouldly;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Xunit;

namespace DataFac.Memory.Tests
{
    public class OctetsTests
    {
        private void AssertAreEquivalent(string chars, Octets octets)
        {
            // assert
            int length = chars.Length;
            octets.Memory.Length.ShouldBe(length);
            byte[] fromChars = Encoding.UTF8.GetBytes(chars);
            byte[] fromOctets = octets.ToArray();

            fromOctets.ShouldBeEquivalentTo(fromChars);
        }

        [Fact]
        public void EmptyA()
        {
            Octets buffer = Octets.Empty;
            buffer.Memory.Length.ShouldBe(0);
        }

        [Fact]
        public void EmptyB()
        {
            Octets? buffer = Octets.UnsafeWrap(ReadOnlyMemory<byte>.Empty);
            buffer.Memory.Length.ShouldBe(0);
            buffer.ShouldBeSameAs(Octets.Empty);
        }

        [Fact]
        public void EmptyC()
        {
            Octets? buffer = Octets.UnsafeWrap(default);
            buffer.Memory.Length.ShouldBe(0);
            buffer.ShouldBeSameAs(Octets.Empty);
        }

        [Fact]
        public void EmptyD()
        {
            Octets buffer = Octets.UnsafeWrap(Array.Empty<byte>());
            buffer.Memory.Length.ShouldBe(0);
            buffer.ShouldBeSameAs(Octets.Empty);
        }

        [Fact]
        public void ConstructFromSequence0_Empty()
        {
            ReadOnlySequence<byte> sequence = ReadOnlySequence<byte>.Empty;
            Octets buffer = new Octets(sequence);
            buffer.Memory.Length.ShouldBe(0);
            buffer.Equals(Octets.Empty).ShouldBeTrue();
        }

        [Fact]
        public void ConstructFromSequence1_SingleSegment()
        {
            ReadOnlySequence<byte> sequence = new ReadOnlySequence<byte>(new byte[] { 1, 2, 3, 4, 5 });
            sequence.IsSingleSegment.ShouldBeTrue();
            Octets buffer1 = new Octets(sequence);
            Octets buffer2 = new Octets(new byte[] { 1, 2, 3, 4, 5 });
            buffer1.Equals(buffer2).ShouldBeTrue();

            int hash1 = buffer1.GetHashCode();
            int hash2 = buffer2.GetHashCode();
            hash2.ShouldBe(hash1);
        }

        [Fact]
        public void ConstructFromSequence2_MultiSegment()
        {
            Octets octets1 = new Octets([0, 1, 2]);
            Octets octets2 = new Octets([3]);
            Octets octets3 = new Octets([4, 5]);
            Octets octets4 = new Octets([6, 7, 8]);
            Octets octets5 = new Octets([9]);
            var sequence = Octets.CreateReadOnlySequence(octets1, octets2, octets3, octets4, octets5);

            sequence.IsSingleSegment.ShouldBeFalse();
            sequence.Length.ShouldBe(10);

            Octets buffer1 = new Octets(sequence);
            Octets buffer2 = new Octets([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            buffer1.Equals(buffer2).ShouldBeTrue();

            int hash1 = buffer1.GetHashCode();
            int hash2 = buffer2.GetHashCode();
            hash2.ShouldBe(hash1);

            // assert compacted sequence is same
            var sequence2 = sequence.Compact();
            Octets buffer3 = Octets.UnsafeWrap(sequence2);
            buffer3.Equals(buffer2).ShouldBeTrue();
        }

        [Fact]
        public void EqualityEmpty()
        {
            Octets buffer1 = Octets.UnsafeWrap(new byte[0])!;
            Octets buffer2 = Octets.Empty;
            buffer1.ShouldBeSameAs(buffer2);
            buffer1.Equals(buffer2).ShouldBeTrue();

            int hash1 = buffer1.GetHashCode();
            int hash2 = buffer2.GetHashCode();
            hash2.ShouldBe(hash1);
        }

        [Fact]
        public void EqualityNonEmpty()
        {
            Octets buffer1 = new Octets(new byte[] { 1, 2, 3 });
            Octets buffer2 = new Octets(new byte[] { 1, 2, 3 });
            buffer1.Equals(buffer2).ShouldBeTrue();

            int hash1 = buffer1.GetHashCode();
            int hash2 = buffer2.GetHashCode();
            hash2.ShouldBe(hash1);
        }

        [Fact]
        public void Block_Create_Empty()
        {
            var s = string.Empty;
            var b = Octets.Empty;

            AssertAreEquivalent(s, b);
        }

        [Fact]
        public void Block_Create_NonEmpty()
        {
            var s = "abc";
            var b = new Octets(Encoding.UTF8.GetBytes(s));

            AssertAreEquivalent(s, b);
        }

        [Fact]
        public void Block_Equality_Null()
        {
            Octets? a = null;
            Octets b = Octets.Empty;

            b.Equals(a).ShouldBeFalse();
            Equals(a, b).ShouldBeFalse();
        }

        [Fact]
        public void Block_Equality_Empty()
        {
            Octets a = Octets.Empty;
            Octets b = new Octets(new byte[0]);

            a.Memory.IsEmpty.ShouldBeTrue();
            b.Memory.IsEmpty.ShouldBeTrue();
            b.GetHashCode().ShouldBe(a.GetHashCode());
            b.ShouldBeEquivalentTo(a);
            b.Equals(a).ShouldBeTrue();
            Equals(a, b).ShouldBeTrue();
        }

        [Fact]
        public void Block_Equality_SameObject()
        {
            Octets a = Octets.Empty;
            Octets b = Octets.Empty;

            b.ShouldBeSameAs(a);
            b.GetHashCode().ShouldBe(a.GetHashCode());
            b.ShouldBeEquivalentTo(a);
            b.Equals(a).ShouldBeTrue();
            Equals(a, b).ShouldBeTrue();
        }

        [Fact]
        public void Block_Equality_DifferentLength()
        {
            Octets a = new Octets(Encoding.UTF8.GetBytes("abc"));
            Octets b = new Octets(Encoding.UTF8.GetBytes("abcd"));

            b.Equals(a).ShouldBeFalse();
            Equals(a, b).ShouldBeFalse();
            (a == b).ShouldBeFalse();
            (a != b).ShouldBeTrue();
        }

        [Fact]
        public void Block_Equality_SameLength()
        {
            Octets a = new Octets(Encoding.UTF8.GetBytes("abc"));
            Octets b = new Octets(Encoding.UTF8.GetBytes("def"));

            b.Equals(a).ShouldBeFalse();
            Equals(a, b).ShouldBeFalse();
            (a == b).ShouldBeFalse();
            (a != b).ShouldBeTrue();
        }

        [Fact]
        public void Block_Equality_NonEmpty()
        {
            var s = "abc";
            var a = new Octets(Encoding.UTF8.GetBytes(s));
            var b = new Octets(Encoding.UTF8.GetBytes(s));

            b.ShouldBeEquivalentTo(a);
            b.GetHashCode().ShouldBe(a.GetHashCode());
            b.Equals(a).ShouldBeTrue();
            b.ShouldNotBeSameAs(a);
            Equals(a, b).ShouldBeTrue();
            (a == b).ShouldBeTrue();
            (a != b).ShouldBeFalse();
        }

        [Fact]
        public void Block_Clone_Empty()
        {
            var a = Octets.Empty;
            var b = Octets.UnsafeWrap(a.Memory);

            a.Memory.IsEmpty.ShouldBeTrue();
            b.Memory.IsEmpty.ShouldBeTrue();
            b.GetHashCode().ShouldBe(a.GetHashCode());
            b.ShouldBeEquivalentTo(a);
            b.Equals(a).ShouldBeTrue();
            (a == b).ShouldBeTrue();
            (a != b).ShouldBeFalse();
        }

        [Fact]
        public void Block_Equality_Null1()
        {
            Octets a = Octets.Empty;
            Octets? b = null;

            Equals(a, b).ShouldBeFalse();
            (a == b).ShouldBeFalse();
            (a != b).ShouldBeTrue();

            Equals(b, a).ShouldBeFalse();
            (b == a).ShouldBeFalse();
            (b != a).ShouldBeTrue();
        }

        [Fact]
        public void Block_Equality_Null2()
        {
            Octets? a = null;
            Octets? b = null;

            Equals(a, b).ShouldBeTrue();
            (a == b).ShouldBeTrue();
            (a != b).ShouldBeFalse();
        }

        [Fact]
        public void Block_Enumeration()
        {
            var bytes = Encoding.UTF8.GetBytes("abc");
            var block = new Octets(bytes);

            block.ToArray().ShouldBeEquivalentTo(bytes);

            IEnumerator<byte> e = block.GetEnumerator();
            e.Reset();
            int i = 0;
            while (e.MoveNext())
            {
                var b = e.Current;
                b.ShouldBe(bytes[i]);
                i++;
            }

            // again
            e.Reset();
            e.MoveNext().ShouldBeTrue();
            e.Current.ShouldBe(bytes[0]);
            e.MoveNext().ShouldBeTrue();
            e.Current.ShouldBe(bytes[1]);
            e.MoveNext().ShouldBeTrue();
            e.Current.ShouldBe(bytes[2]);
            e.MoveNext().ShouldBeFalse();
        }

        [Fact]
        public void Block_Clone_NonEmpty()
        {
            var a = new Octets(Encoding.UTF8.GetBytes("abc"));
            var b = Octets.UnsafeWrap(a.Memory);

            a.Memory.IsEmpty.ShouldBeFalse();
            a.Memory.Length.ShouldBe(3);
            b.Memory.IsEmpty.ShouldBeFalse();
            a.Memory.Length.ShouldBe(3);
            b.GetHashCode().ShouldBe(a.GetHashCode());
            b.ShouldBeEquivalentTo(a);
            b.Equals(a).ShouldBeTrue();
        }

        [Fact]
        public void Block_IsImmutable_FromByteArray()
        {
            var immutable = ImmutableArray<byte>.Empty.AddRange(new byte[] { 1, 2, 3 });
            var mutable = new byte[] { 1, 2, 3 };

            Octets block = new Octets(mutable);
            var output1 = block.Memory.ToArray();
            output1.ShouldBeEquivalentTo(immutable.ToArray());
            output1.ShouldBeEquivalentTo(mutable);
            int hash1 = block.GetHashCode();

            // act
            mutable[1] = 4;

            // assert
            var output2 = block.Memory.ToArray();
            output2.ShouldBeEquivalentTo(immutable.ToArray());
            output2.SequenceEqual(mutable).ShouldBeFalse();
            int hash2 = block.GetHashCode();
            hash2.ShouldBe(hash1);
        }

        [Fact]
        public void Block_IsImmutable_FromArraySegment()
        {
            var immutable = ImmutableArray<byte>.Empty.AddRange(new byte[] { 1, 2, 3 });
            var mutable = new byte[] { 1, 2, 3 };
            var segment = new ArraySegment<byte>(mutable);

            Octets block = new Octets(segment);
            byte[] output1 = block.Memory.ToArray();
            output1.ShouldBeEquivalentTo(immutable.ToArray());
            output1.ShouldBeEquivalentTo(mutable);
            int hash1 = block.GetHashCode();

            // act
            mutable[1] = 4;

            // assert
            var output2 = block.Memory.ToArray();
            output2.ShouldBeEquivalentTo(immutable.ToArray());
            //output2.ShouldNotBeEquivalentTo(mutable);
            output2.SequenceEqual(mutable).ShouldBeFalse();
            int hash2 = block.GetHashCode();
            hash2.ShouldBe(hash1);
        }

        [Fact]
        public void Block_IsImmutable_FromSpan()
        {
            var immutable = ImmutableArray<byte>.Empty.AddRange(new byte[] { 1, 2, 3 });
            var mutable = new byte[] { 1, 2, 3 };
            var span = new ReadOnlySpan<byte>(mutable);

            Octets block = new Octets(span);
            var output1 = block.Memory.ToArray();
            output1.ShouldBeEquivalentTo(immutable.ToArray());
            output1.ShouldBeEquivalentTo(mutable);
            int hash1 = block.GetHashCode();

            // act
            mutable[1] = 4;

            // assert
            var output2 = block.Memory.ToArray();
            output2.ShouldBeEquivalentTo(immutable.ToArray());
            //output2.ShouldNotBeEquivalentTo(mutable);
            output2.SequenceEqual(mutable).ShouldBeFalse();
            int hash2 = block.GetHashCode();
            hash2.ShouldBe(hash1);
        }

        [Fact]
        public void Block_UnsafeWrap_IsNotImmutable()
        {
            var immutable = ImmutableArray<byte>.Empty.AddRange(new byte[] { 1, 2, 3 });
            var mutable = new byte[] { 1, 2, 3 };
            var memory = new ReadOnlyMemory<byte>(mutable);

            Octets block = Octets.UnsafeWrap(memory);
            var output1 = block.Memory.ToArray();
            output1.ShouldBeEquivalentTo(immutable.ToArray());
            output1.ShouldBeEquivalentTo(mutable);

            // act
            mutable[1] = 4;

            // assert
            var output2 = block.Memory.ToArray();
            //output2.ShouldNotBeEquivalentTo(immutable);
            output2.SequenceEqual(immutable).ShouldBeFalse();
            output2.ShouldBeEquivalentTo(mutable);
        }

        [Fact]
        public void Block_UnsafeWrap_CanBeMadeSafe()
        {
            var mutable = new byte[] { 1, 2, 3 };
            var memory = new ReadOnlyMemory<byte>(mutable);

            // arrange
            Octets unsafeOrig = Octets.UnsafeWrap(memory);
            Octets unsafeCopy = Octets.UnsafeWrap(unsafeOrig.Memory);
            Octets safeCopy = new Octets(unsafeOrig.Memory.Span);

            // act
            mutable[1] = 4;

            // assert
            unsafeCopy.Equals(unsafeOrig).ShouldBeTrue();
            unsafeCopy.GetHashCode().ShouldBe(unsafeOrig.GetHashCode());

            safeCopy.Equals(unsafeOrig).ShouldBeFalse();
            safeCopy.GetHashCode().ShouldNotBe(unsafeOrig.GetHashCode());
        }

        [Fact]
        public void GetHeadEmpty()
        {
            Octets orig = Octets.Empty;
            (var head, var body) = orig.GetHead(0);
            head.Equals(Octets.Empty).ShouldBeTrue();
            body.Equals(Octets.Empty).ShouldBeTrue();
            var copy = new Octets(head.Memory.Span, body.Memory.Span);
            copy.Equals(orig).ShouldBeTrue();
        }

        [Fact]
        public void GetHeadNonEmptyA()
        {
            Octets orig = new Octets(new byte[] { 1, 2, 3 });
            (var head, var body) = orig.GetHead(1);
            head.Memory.Length.ShouldBe(1);
            var headBytes = head.Memory.ToArray();
            headBytes[0].ShouldBe((byte)1);
            body.Memory.Length.ShouldBe(2);
            var bodyBytes = body.Memory.ToArray();
            bodyBytes[0].ShouldBe((byte)2);
            bodyBytes[1].ShouldBe((byte)3);
            var copy = new Octets(head.Memory.Span, body.Memory.Span);
            copy.Equals(orig).ShouldBeTrue();
        }

        [Fact]
        public void GetHeadNonEmptyB()
        {
            Octets orig = new Octets(new byte[] { 1, 2 }, new byte[] { 3, 4 });
            (var head, var body) = orig.GetHead(1);
            // head
            head.Memory.Length.ShouldBe(1);
            var headBytes = head.Memory.ToArray();
            headBytes[0].ShouldBe((byte)1);
            // body
            body.Memory.Length.ShouldBe(3);
            var bodyBytes = body.Memory.ToArray();
            bodyBytes[0].ShouldBe((byte)2);
            bodyBytes[1].ShouldBe((byte)3);
            bodyBytes[2].ShouldBe((byte)4);

            var copy = new Octets(head.Memory.Span, body.Memory.Span);
            copy.Equals(orig).ShouldBeTrue();
        }

        [Fact]
        public void GetHeadTailEmpty()
        {
            Octets orig = Octets.Empty;
            (var head, var body, var tail) = orig.GetHeadAndBody(0, 0);
            head.Equals(Octets.Empty).ShouldBeTrue();
            body.Equals(Octets.Empty).ShouldBeTrue();
            tail.Equals(Octets.Empty).ShouldBeTrue();
            var copy = new Octets(head.Memory.Span, body.Memory.Span, tail.Memory.Span);
            copy.Equals(orig).ShouldBeTrue();
        }

        [Fact]
        public void GetHeadTailNonEmptyA()
        {
            Octets orig = new Octets(new byte[] { 1, 2, 3 });
            (var head, var body, var tail) = orig.GetHeadAndBody(1, 1);
            head.Memory.Length.ShouldBe(1);
            var headBytes = head.Memory.ToArray();
            headBytes[0].ShouldBe((byte)1);
            body.Memory.Length.ShouldBe(1);
            var bodyBytes = body.Memory.ToArray();
            bodyBytes[0].ShouldBe((byte)2);
            tail.Memory.Length.ShouldBe(1);
            var tailBytes = tail.Memory.ToArray();
            tailBytes[0].ShouldBe((byte)3);
            var copy = new Octets(head.Memory.Span, body.Memory.Span, tail.Memory.Span);
            copy.Equals(orig).ShouldBeTrue();
        }

        [Fact]
        public void GetHeadTailNonEmptyB()
        {
            Octets orig = new Octets(new byte[] { 1, 2, 3, 4 }, new byte[] { 5, 6, 7, 8 });
            (var head, var body, var tail) = orig.GetHeadAndBody(2, 4);
            // head
            head.Memory.Length.ShouldBe(2);
            var headBytes = head.Memory.ToArray();
            headBytes[0].ShouldBe((byte)1);
            headBytes[1].ShouldBe((byte)2);
            // body
            body.Memory.Length.ShouldBe(4);
            var bodyBytes = body.Memory.ToArray();
            bodyBytes[0].ShouldBe((byte)3);
            bodyBytes[1].ShouldBe((byte)4);
            bodyBytes[2].ShouldBe((byte)5);
            bodyBytes[3].ShouldBe((byte)6);
            // tail
            tail.Memory.Length.ShouldBe(2);
            var tailBytes = tail.Memory.ToArray();
            tailBytes[0].ShouldBe((byte)7);
            tailBytes[1].ShouldBe((byte)8);

            var copy = new Octets(head.Memory.Span, body.Memory.Span, tail.Memory.Span);
            copy.Equals(orig).ShouldBeTrue();
        }
    }
}