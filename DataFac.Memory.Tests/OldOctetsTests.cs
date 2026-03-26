using Shouldly;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace DataFac.Memory.Tests
{
    public class OldOctetsTests
    {
        private void AssertAreEquivalent(string chars, OctetsOld octets)
        {
            // assert
            int length = chars.Length;
            var memory = octets.AsMemory();
            memory.Length.ShouldBe(length);
            byte[] fromChars = Encoding.UTF8.GetBytes(chars);
            byte[] fromOctets = memory.ToArray();

            fromOctets.ShouldBeEquivalentTo(fromChars);
        }

        [Fact]
        public void EmptyA()
        {
            OctetsOld buffer = OctetsOld.Empty;
            buffer.AsMemory().Length.ShouldBe(0);
        }

        [Fact]
        public void EmptyB()
        {
            OctetsOld buffer = OctetsOld.UnsafeWrap(ReadOnlyMemory<byte>.Empty);
            buffer.AsMemory().Length.ShouldBe(0);
            buffer.ShouldBeSameAs(OctetsOld.Empty);
        }

        [Fact]
        public void EmptyC()
        {
            OctetsOld buffer = OctetsOld.UnsafeWrap(ReadOnlySequence<byte>.Empty);
            buffer.AsMemory().Length.ShouldBe(0);
            buffer.ShouldBeSameAs(OctetsOld.Empty);
        }

        [Fact]
        public void EmptyD()
        {
            OctetsOld buffer = OctetsOld.UnsafeWrap(Array.Empty<byte>());
            buffer.AsMemory().Length.ShouldBe(0);
            buffer.ShouldBeSameAs(OctetsOld.Empty);
        }

        [Fact]
        public void ConstructFromSequence0_Empty()
        {
            ReadOnlySequence<byte> sequence = ReadOnlySequence<byte>.Empty;
            OctetsOld buffer = OctetsOld.UnsafeWrap(sequence);
            buffer.AsMemory().Length.ShouldBe(0);
            buffer.Equals(OctetsOld.Empty).ShouldBeTrue();
        }

        [Fact]
        public void ConstructFromSequence1_SingleSegment()
        {
            ReadOnlySequence<byte> sequence = new ReadOnlySequence<byte>(new byte[] { 1, 2, 3, 4, 5 });
            sequence.IsSingleSegment.ShouldBeTrue();
            OctetsOld buffer1 = OctetsOld.UnsafeWrap(sequence);
            OctetsOld buffer2 = OctetsOld.UnsafeWrap(new ReadOnlyMemory<byte>(new byte[] { 1, 2, 3, 4, 5 }));
            buffer1.Equals(buffer2).ShouldBeTrue();

            int hash1 = buffer1.GetHashCode();
            int hash2 = buffer2.GetHashCode();
            hash2.ShouldBe(hash1);
        }

        [Fact]
        public void ConstructFromSequence2_MultiSegment()
        {
            OctetsOld octets1 = new OctetsOld([0, 1, 2]);
            OctetsOld octets2 = new OctetsOld([3]);
            OctetsOld octets3 = new OctetsOld([4, 5]);
            OctetsOld octets4 = new OctetsOld([6, 7, 8]);
            OctetsOld octets5 = new OctetsOld([9]);
            var octets6a = OctetsOld.Combine(octets1, octets2, octets3, octets4, octets5);

            octets6a.ToSequence().IsSingleSegment.ShouldBeFalse();
            octets6a.Length.ShouldBe(10);

            OctetsOld octets6b = new OctetsOld([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            octets6b.ToSequence().IsSingleSegment.ShouldBeTrue();
            // assert compacted sequence is same as uncompacted
            octets6b.Equals(octets6a).ShouldBeTrue();

            int hash1 = octets6a.GetHashCode();
            int hash2 = octets6b.GetHashCode();
            hash2.ShouldBe(hash1);
        }

        [Fact]
        public void EqualityEmpty()
        {
            OctetsOld buffer1 = OctetsOld.UnsafeWrap(new byte[0])!;
            OctetsOld buffer2 = OctetsOld.Empty;
            buffer1.ShouldBeSameAs(buffer2);
            buffer1.Equals(buffer2).ShouldBeTrue();

            int hash1 = buffer1.GetHashCode();
            int hash2 = buffer2.GetHashCode();
            hash2.ShouldBe(hash1);
        }

        [Fact]
        public void EqualityNonEmpty()
        {
            OctetsOld buffer1 = new OctetsOld(new byte[] { 1, 2, 3 });
            OctetsOld buffer2 = new OctetsOld(new byte[] { 1, 2, 3 });
            buffer1.Equals(buffer2).ShouldBeTrue();

            int hash1 = buffer1.GetHashCode();
            int hash2 = buffer2.GetHashCode();
            hash2.ShouldBe(hash1);
        }

        [Fact]
        public void Block_Create_Empty()
        {
            var s = string.Empty;
            var b = OctetsOld.Empty;

            AssertAreEquivalent(s, b);
        }

        [Fact]
        public void Block_Create_NonEmpty()
        {
            var s = "abc";
            var b = new OctetsOld(Encoding.UTF8.GetBytes(s));

            AssertAreEquivalent(s, b);
        }

        [Fact]
        public void Block_Equality_Null()
        {
            OctetsOld? a = null;
            OctetsOld b = OctetsOld.Empty;

            b.Equals(a).ShouldBeFalse();
            Equals(a, b).ShouldBeFalse();
        }

        [Fact]
        public void Block_Equality_Empty()
        {
            OctetsOld a = OctetsOld.Empty;
            OctetsOld b = new OctetsOld(new byte[0]);

            a.AsMemory().IsEmpty.ShouldBeTrue();
            b.AsMemory().IsEmpty.ShouldBeTrue();
            b.GetHashCode().ShouldBe(a.GetHashCode());
            b.ShouldBeEquivalentTo(a);
            b.Equals(a).ShouldBeTrue();
            Equals(a, b).ShouldBeTrue();
        }

        [Fact]
        public void Block_Equality_SameObject()
        {
            OctetsOld a = OctetsOld.Empty;
            OctetsOld b = OctetsOld.Empty;

            b.ShouldBeSameAs(a);
            b.GetHashCode().ShouldBe(a.GetHashCode());
            b.ShouldBeEquivalentTo(a);
            b.Equals(a).ShouldBeTrue();
            Equals(a, b).ShouldBeTrue();
        }

        [Fact]
        public void Block_Equality_DifferentLength()
        {
            OctetsOld a = new OctetsOld(Encoding.UTF8.GetBytes("abc"));
            OctetsOld b = new OctetsOld(Encoding.UTF8.GetBytes("abcd"));

            b.Equals(a).ShouldBeFalse();
            Equals(a, b).ShouldBeFalse();
            (a == b).ShouldBeFalse();
            (a != b).ShouldBeTrue();
        }

        [Fact]
        public void Block_Equality_SameLength()
        {
            OctetsOld a = new OctetsOld(Encoding.UTF8.GetBytes("abc"));
            OctetsOld b = new OctetsOld(Encoding.UTF8.GetBytes("def"));

            b.Equals(a).ShouldBeFalse();
            Equals(a, b).ShouldBeFalse();
            (a == b).ShouldBeFalse();
            (a != b).ShouldBeTrue();
        }

        [Fact]
        public void Block_Equality_NonEmpty()
        {
            var s = "abc";
            var a = new OctetsOld(Encoding.UTF8.GetBytes(s));
            var b = new OctetsOld(Encoding.UTF8.GetBytes(s));

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
            var a = OctetsOld.Empty;
            var b = OctetsOld.UnsafeWrap(a.AsMemory());

            a.AsMemory().IsEmpty.ShouldBeTrue();
            b.AsMemory().IsEmpty.ShouldBeTrue();
            b.GetHashCode().ShouldBe(a.GetHashCode());
            b.ShouldBeEquivalentTo(a);
            b.Equals(a).ShouldBeTrue();
            (a == b).ShouldBeTrue();
            (a != b).ShouldBeFalse();
        }

        [Fact]
        public void Block_Equality_Null1()
        {
            OctetsOld a = OctetsOld.Empty;
            OctetsOld? b = null;

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
            OctetsOld? a = null;
            OctetsOld? b = null;

            Equals(a, b).ShouldBeTrue();
            (a == b).ShouldBeTrue();
            (a != b).ShouldBeFalse();
        }

        [Fact]
        public void Block_Enumeration()
        {
            var bytes = Encoding.UTF8.GetBytes("abc");
            var block = new OctetsOld(bytes);

            block.ToSequence().ToArray().ShouldBeEquivalentTo(bytes);

            IEnumerator<byte> e = block.ToByteArray().ToList().GetEnumerator();
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
            var a = new OctetsOld(Encoding.UTF8.GetBytes("abc"));
            var b = OctetsOld.UnsafeWrap(a.AsMemory());

            a.AsMemory().IsEmpty.ShouldBeFalse();
            a.AsMemory().Length.ShouldBe(3);
            b.AsMemory().IsEmpty.ShouldBeFalse();
            a.AsMemory().Length.ShouldBe(3);
            b.GetHashCode().ShouldBe(a.GetHashCode());
            b.ShouldBeEquivalentTo(a);
            b.Equals(a).ShouldBeTrue();
        }

        [Fact]
        public void Block_IsImmutable()
        {
            ReadOnlySpan<byte> immutable = new byte[] { 1, 2, 3 };
            var mutable = new byte[] { 1, 2, 3 };

            OctetsOld block = new OctetsOld(mutable);
            block.AsMemory().Span.SequenceEqual(immutable).ShouldBeTrue();
            block.AsMemory().Span.SequenceEqual(mutable).ShouldBeTrue();
            int hash1 = block.GetHashCode();

            // act
            mutable[1] = 4;

            // assert
            block.AsMemory().Span.SequenceEqual(immutable).ShouldBeTrue();
            block.AsMemory().Span.SequenceEqual(mutable).ShouldBeFalse();
            int hash2 = block.GetHashCode();
            hash2.ShouldBe(hash1);
        }

        [Fact]
        public void Block_UnsafeWrap_IsNotImmutable()
        {
            ReadOnlySpan<byte> immutable = new byte[] { 1, 2, 3 };
            var mutable = new byte[] { 1, 2, 3 };
            var memory = new ReadOnlyMemory<byte>(mutable);

            OctetsOld block = OctetsOld.UnsafeWrap(memory);
            block.AsMemory().Span.SequenceEqual(immutable).ShouldBeTrue();
            block.AsMemory().Span.SequenceEqual(mutable).ShouldBeTrue();

            // act
            mutable[1] = 4;

            // assert
            block.AsMemory().Span.SequenceEqual(immutable).ShouldBeFalse();
            block.AsMemory().Span.SequenceEqual(mutable).ShouldBeTrue();
        }

        [Fact]
        public void Block_UnsafeWrap_CanBeMadeSafe()
        {
            var mutable = new byte[] { 1, 2, 3 };
            var memory = new ReadOnlyMemory<byte>(mutable);

            // arrange
            OctetsOld unsafeOrig = OctetsOld.UnsafeWrap(memory);
            OctetsOld unsafeCopy = OctetsOld.UnsafeWrap(unsafeOrig.AsMemory());
            OctetsOld safeCopy = new OctetsOld(unsafeOrig.AsMemory().Span);

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
            OctetsOld orig = OctetsOld.Empty;
            (var head, var body) = orig.GetHead(0);
            head.Equals(OctetsOld.Empty).ShouldBeTrue();
            body.Equals(OctetsOld.Empty).ShouldBeTrue();
            var copy = new OctetsOld(head.AsMemory().Span, body.AsMemory().Span);
            copy.Equals(orig).ShouldBeTrue();
        }

        [Fact]
        public void GetHeadNonEmptyA()
        {
            OctetsOld orig = new OctetsOld(new byte[] { 1, 2, 3 });
            (var head, var body) = orig.GetHead(1);
            head.AsMemory().Length.ShouldBe(1);
            var headBytes = head.ToByteArray();
            headBytes[0].ShouldBe((byte)1);
            body.AsMemory().Length.ShouldBe(2);
            var bodyBytes = body.ToByteArray();
            bodyBytes[0].ShouldBe((byte)2);
            bodyBytes[1].ShouldBe((byte)3);
            var copy = new OctetsOld(head.AsMemory().Span, body.AsMemory().Span);
            copy.Equals(orig).ShouldBeTrue();
        }

        [Fact]
        public void GetHeadNonEmptyB()
        {
            OctetsOld orig = new OctetsOld(new byte[] { 1, 2 }, new byte[] { 3, 4 });
            (var head, var body) = orig.GetHead(1);
            // head
            head.AsMemory().Length.ShouldBe(1);
            var headBytes = head.ToByteArray();
            headBytes[0].ShouldBe((byte)1);
            // body
            body.AsMemory().Length.ShouldBe(3);
            var bodyBytes = body.ToByteArray();
            bodyBytes[0].ShouldBe((byte)2);
            bodyBytes[1].ShouldBe((byte)3);
            bodyBytes[2].ShouldBe((byte)4);

            var copy = new OctetsOld(head.AsMemory().Span, body.AsMemory().Span);
            copy.Equals(orig).ShouldBeTrue();
        }

        [Fact]
        public void GetHeadTailEmpty()
        {
            OctetsOld orig = OctetsOld.Empty;
            (var head, var body, var tail) = orig.GetHeadAndBody(0, 0);
            head.Equals(OctetsOld.Empty).ShouldBeTrue();
            body.Equals(OctetsOld.Empty).ShouldBeTrue();
            tail.Equals(OctetsOld.Empty).ShouldBeTrue();
            var copy = new OctetsOld(head.AsMemory().Span, body.AsMemory().Span, tail.AsMemory().Span);
            copy.Equals(orig).ShouldBeTrue();
        }

        [Fact]
        public void GetHeadTailNonEmptyA()
        {
            OctetsOld orig = new OctetsOld(new byte[] { 1, 2, 3 });
            (var head, var body, var tail) = orig.GetHeadAndBody(1, 1);
            head.AsMemory().Length.ShouldBe(1);
            var headBytes = head.ToByteArray();
            headBytes[0].ShouldBe((byte)1);
            body.AsMemory().Length.ShouldBe(1);
            var bodyBytes = body.ToByteArray();
            bodyBytes[0].ShouldBe((byte)2);
            tail.AsMemory().Length.ShouldBe(1);
            var tailBytes = tail.ToByteArray();
            tailBytes[0].ShouldBe((byte)3);
            var copy = new OctetsOld(head.AsMemory().Span, body.AsMemory().Span, tail.AsMemory().Span);
            copy.Equals(orig).ShouldBeTrue();
        }

        [Fact]
        public void GetHeadTailNonEmptyB()
        {
            OctetsOld orig = new OctetsOld(new byte[] { 1, 2, 3, 4 }, new byte[] { 5, 6, 7, 8 });
            (var head, var body, var tail) = orig.GetHeadAndBody(2, 4);
            // head
            head.AsMemory().Length.ShouldBe(2);
            var headBytes = head.ToByteArray();
            headBytes[0].ShouldBe((byte)1);
            headBytes[1].ShouldBe((byte)2);
            // body
            body.AsMemory().Length.ShouldBe(4);
            var bodyBytes = body.ToByteArray();
            bodyBytes[0].ShouldBe((byte)3);
            bodyBytes[1].ShouldBe((byte)4);
            bodyBytes[2].ShouldBe((byte)5);
            bodyBytes[3].ShouldBe((byte)6);
            // tail
            tail.AsMemory().Length.ShouldBe(2);
            var tailBytes = tail.ToByteArray();
            tailBytes[0].ShouldBe((byte)7);
            tailBytes[1].ShouldBe((byte)8);

            var copy = new OctetsOld(head.AsMemory().Span, body.AsMemory().Span, tail.AsMemory().Span);
            copy.Equals(orig).ShouldBeTrue();
        }
    }
}