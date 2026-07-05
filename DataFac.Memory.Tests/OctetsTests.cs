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
            var memory = octets.AsMemory();
            memory.Length.ShouldBe(length);
            byte[] fromChars = Encoding.UTF8.GetBytes(chars);
            byte[] fromOctets = memory.ToArray();

            fromOctets.ShouldBeEquivalentTo(fromChars);
        }

        [Fact]
        public void EmptyA()
        {
            Octets buffer = Octets.Empty;
            buffer.Length.ShouldBe(0);
        }

        [Fact]
        public void EmptyB()
        {
            Octets buffer = Octets.Wrap(ReadOnlyMemory<byte>.Empty);
            buffer.Length.ShouldBe(0);
            buffer.ShouldBeSameAs(Octets.Empty);
        }

        [Fact]
        public void EmptyC()
        {
            Octets buffer = Octets.Wrap(ReadOnlySequence<byte>.Empty);
            buffer.Length.ShouldBe(0);
            buffer.ShouldBeSameAs(Octets.Empty);
        }

        [Fact]
        public void EmptyD()
        {
            Octets buffer = Octets.Wrap(Array.Empty<byte>());
            buffer.Length.ShouldBe(0);
            buffer.ShouldBeSameAs(Octets.Empty);
        }

        [Fact]
        public void ConstructFromSequence0_Empty()
        {
            ReadOnlySequence<byte> sequence = ReadOnlySequence<byte>.Empty;
            Octets buffer = Octets.Wrap(sequence);
            buffer.Length.ShouldBe(0);
            buffer.Equals(Octets.Empty).ShouldBeTrue();
        }

        [Fact]
        public void ConstructFromSequence1_SingleSegment()
        {
            ReadOnlySequence<byte> sequence = new ReadOnlySequence<byte>(new byte[] { 1, 2, 3, 4, 5 });
            sequence.IsSingleSegment.ShouldBeTrue();
            Octets buffer1 = Octets.Wrap(sequence);
            Octets buffer2 = Octets.Wrap(new ReadOnlyMemory<byte>(new byte[] { 1, 2, 3, 4, 5 }));
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
            var octets6a = Octets.Combine(octets1, octets2, octets3, octets4, octets5);

            octets6a.ToSequence().IsSingleSegment.ShouldBeFalse();
            octets6a.Length.ShouldBe(10);

            Octets octets6b = new Octets([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
            octets6b.ToSequence().IsSingleSegment.ShouldBeTrue();
            // assert compacted sequence is same as uncompacted
            octets6b.Equals(octets6a).ShouldBeTrue();

            int hash1 = octets6a.GetHashCode();
            int hash2 = octets6b.GetHashCode();
            hash2.ShouldBe(hash1);
        }

        [Fact]
        public void ConstructFromBinaryData()
        {
            var orig1 = new BinaryData(new byte[] { 1, 2, 3 });
            var orig2 = new BinaryData(new byte[] { 1, 2, 3 });

            (orig1 == orig2).ShouldBeFalse();

            var octets1 = Octets.Wrap(orig1);
            var octets2 = Octets.Wrap(orig2);

            (octets2 == octets1).ShouldBeTrue();
            octets2.Equals(octets1).ShouldBeTrue();
            octets2.ShouldBe(octets1);
        }

        [Fact]
        public void ConstructFromImmutableArray()
        {
            var orig1 = ImmutableArray.Create(new byte[] { 1, 2, 3 });
            var orig2 = ImmutableArray.Create(new byte[] { 1, 2, 3 });

            (orig1 == orig2).ShouldBeFalse();

            var octets1 = Octets.Wrap(orig1.AsMemory());
            var octets2 = Octets.Wrap(orig2.AsMemory());

            (octets2 == octets1).ShouldBeTrue();
            octets2.Equals(octets1).ShouldBeTrue();
            octets2.ShouldBe(octets1);
        }

        [Fact]
        public void ConstructFromArraySegment()
        {
            var orig1 = new ArraySegment<byte>(new byte[] { 1, 2, 3 });
            var orig2 = new ArraySegment<byte>(new byte[] { 1, 2, 3 });

            (orig1 == orig2).ShouldBeFalse();

            var octets1 = Octets.Wrap(orig1);
            var octets2 = Octets.Wrap(orig2);

            (octets2 == octets1).ShouldBeTrue();
            octets2.Equals(octets1).ShouldBeTrue();
            octets2.ShouldBe(octets1);
        }

        [Fact]
        public void EqualityEmpty()
        {
            Octets buffer1 = Octets.Wrap(new byte[0])!;
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

            a.AsMemory().IsEmpty.ShouldBeTrue();
            b.AsMemory().IsEmpty.ShouldBeTrue();
            b.GetHashCode().ShouldBe(a.GetHashCode());
            b.Equals(a).ShouldBeTrue();
            Equals(a, b).ShouldBeTrue();
            IEnumerable<byte> aEnum = a;
            IEnumerable<byte> bEnum = b;
            bEnum.ShouldBeEquivalentTo(aEnum);
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
            var b = Octets.Wrap(a.AsMemory());

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

            block.ToSequence().ToArray().ShouldBeEquivalentTo(bytes);

            int i = 0;
            foreach (byte b in block)
            {
                b.ShouldBe(bytes[i]);
                i++;
            }

            // again
            IEnumerator<byte> e = block.GetEnumerator(); 
            i = 0;
            while (e.MoveNext())
            {
                var b = e.Current;
                b.ShouldBe(bytes[i]);
                i++;
            }

            // and again
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
            var b = Octets.Wrap(a.AsMemory());

            a.AsMemory().IsEmpty.ShouldBeFalse();
            a.Length.ShouldBe(3);
            b.AsMemory().IsEmpty.ShouldBeFalse();
            a.Length.ShouldBe(3);
            b.GetHashCode().ShouldBe(a.GetHashCode());
            b.ShouldBeEquivalentTo(a);
            b.Equals(a).ShouldBeTrue();
        }

        [Fact]
        public void Block_IsImmutable()
        {
            ReadOnlySpan<byte> immutable = new byte[] { 1, 2, 3 };
            var mutable = new byte[] { 1, 2, 3 };

            Octets block = new Octets(mutable);
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

            Octets block = Octets.Wrap(memory);
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
            Octets unsafeOrig = Octets.Wrap(memory);
            Octets unsafeCopy = Octets.Wrap(unsafeOrig.AsMemory());
            Octets safeCopy = new Octets(unsafeOrig.AsMemory().Span);

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
            var copy = new Octets(head.AsMemory().Span, body.AsMemory().Span);
            copy.Equals(orig).ShouldBeTrue();
        }

        [Fact]
        public void GetHeadNonEmptyA()
        {
            Octets orig = new Octets(new byte[] { 1, 2, 3 });
            (var head, var body) = orig.GetHead(1);
            head.Length.ShouldBe(1);
            var headBytes = head.ToByteArray();
            headBytes[0].ShouldBe((byte)1);
            body.Length.ShouldBe(2);
            var bodyBytes = body.ToByteArray();
            bodyBytes[0].ShouldBe((byte)2);
            bodyBytes[1].ShouldBe((byte)3);
            var copy = new Octets(head.AsMemory().Span, body.AsMemory().Span);
            copy.Equals(orig).ShouldBeTrue();
        }

        [Fact]
        public void GetHeadNonEmptyB()
        {
            Octets orig = new Octets(new byte[] { 1, 2 }, new byte[] { 3, 4 });
            (var head, var body) = orig.GetHead(1);
            // head
            head.Length.ShouldBe(1);
            var headBytes = head.ToByteArray();
            headBytes[0].ShouldBe((byte)1);
            // body
            body.Length.ShouldBe(3);
            var bodyBytes = body.ToByteArray();
            bodyBytes[0].ShouldBe((byte)2);
            bodyBytes[1].ShouldBe((byte)3);
            bodyBytes[2].ShouldBe((byte)4);

            var copy = new Octets(head.AsMemory().Span, body.AsMemory().Span);
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
            var copy = new Octets(head.AsMemory().Span, body.AsMemory().Span, tail.AsMemory().Span);
            copy.Equals(orig).ShouldBeTrue();
        }

        [Fact]
        public void GetHeadTailNonEmptyA()
        {
            Octets orig = new Octets(new byte[] { 1, 2, 3 });
            (var head, var body, var tail) = orig.GetHeadAndBody(1, 1);
            head.Length.ShouldBe(1);
            var headBytes = head.ToByteArray();
            headBytes[0].ShouldBe((byte)1);
            body.Length.ShouldBe(1);
            var bodyBytes = body.ToByteArray();
            bodyBytes[0].ShouldBe((byte)2);
            tail.Length.ShouldBe(1);
            var tailBytes = tail.ToByteArray();
            tailBytes[0].ShouldBe((byte)3);
            var copy = new Octets(head.AsMemory().Span, body.AsMemory().Span, tail.AsMemory().Span);
            copy.Equals(orig).ShouldBeTrue();
        }

        [Fact]
        public void GetHeadTailNonEmptyB()
        {
            Octets orig = new Octets(new byte[] { 1, 2, 3, 4 }, new byte[] { 5, 6, 7, 8 });
            (var head, var body, var tail) = orig.GetHeadAndBody(2, 4);
            // head
            head.Length.ShouldBe(2);
            var headBytes = head.ToByteArray();
            headBytes[0].ShouldBe((byte)1);
            headBytes[1].ShouldBe((byte)2);
            // body
            body.Length.ShouldBe(4);
            var bodyBytes = body.ToByteArray();
            bodyBytes[0].ShouldBe((byte)3);
            bodyBytes[1].ShouldBe((byte)4);
            bodyBytes[2].ShouldBe((byte)5);
            bodyBytes[3].ShouldBe((byte)6);
            // tail
            tail.Length.ShouldBe(2);
            var tailBytes = tail.ToByteArray();
            tailBytes[0].ShouldBe((byte)7);
            tailBytes[1].ShouldBe((byte)8);

            var copy = new Octets(head.AsMemory().Span, body.AsMemory().Span, tail.AsMemory().Span);
            copy.Equals(orig).ShouldBeTrue();
        }
    }
}