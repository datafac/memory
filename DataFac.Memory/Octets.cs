﻿using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DataFac.Memory
{
    /// <summary>
    /// An immutable reference type that wraps a ReadOnlyMemory<byte> buffer.
    /// </summary>
    public sealed class Octets : IEnumerable<byte>, IReadOnlyList<byte>, IEquatable<Octets?>
    {
        private static readonly Octets _empty = new Octets(ReadOnlyMemory<byte>.Empty);
        public static Octets Empty => _empty;

        /// <summary>
        /// Wraps the source bytes *without* copying. This assumes the source will never change.
        /// </summary>
        /// <param name="source"></param>
        /// <returns>The wrapped buffer.</returns>
        public static Octets UnsafeWrap(ReadOnlyMemory<byte> source)
        {
            return source.Length == 0 ? _empty : new Octets(source);
        }

        /// <summary>
        /// Creates a ReadOnlySequence\<byte\> from given Octets.
        /// </summary>
        public static ReadOnlySequence<byte> CreateReadOnlySequence(Octets buffer1)
        {
            ReadOnlySequenceBuilder<byte> builder = new(buffer1.Memory);
            return builder.Build();
        }

        /// <summary>
        /// Creates a ReadOnlySequence\<byte\> from given Octets.
        /// </summary>
        public static ReadOnlySequence<byte> CreateReadOnlySequence(Octets buffer1, Octets buffer2)
        {
            ReadOnlySequenceBuilder<byte> builder = new(buffer1.Memory, buffer2.Memory);
            return builder.Build();
        }

        /// <summary>
        /// Creates a ReadOnlySequence\<byte\> from given Octets.
        /// </summary>
        public static ReadOnlySequence<byte> CreateReadOnlySequence(Octets buffer1, Octets buffer2, Octets buffer3)
        {
            ReadOnlySequenceBuilder<byte> builder = new(buffer1.Memory, buffer2.Memory, buffer3.Memory);
            return builder.Build();
        }

        /// <summary>
        /// Creates a ReadOnlySequence\<byte\> from given Octets.
        /// </summary>
        public static ReadOnlySequence<byte> CreateReadOnlySequence(Octets buffer1, Octets buffer2, Octets buffer3, Octets buffer4)
        {
            ReadOnlySequenceBuilder<byte> builder = new(buffer1.Memory, buffer2.Memory, buffer3.Memory);
            builder.Add(buffer4.Memory);
            return builder.Build();
        }

        /// <summary>
        /// Creates a ReadOnlySequence\<byte\> from given Octets.
        /// </summary>
        public static ReadOnlySequence<byte> CreateReadOnlySequence(Octets buffer1, Octets buffer2, Octets buffer3, Octets buffer4, Octets buffer5)
        {
            ReadOnlySequenceBuilder<byte> builder = new(buffer1.Memory, buffer2.Memory, buffer3.Memory);
            builder.Add(buffer4.Memory);
            builder.Add(buffer5.Memory);
            return builder.Build();
        }

        /// <summary>
        /// Creates a ReadOnlySequence\<byte\> from given Octets.
        /// </summary>
        public static ReadOnlySequence<byte> CreateReadOnlySequence(params Octets[] buffers)
        {
            ReadOnlySequenceBuilder<byte> builder = default;
            foreach (var buffer in buffers)
            {
                builder.Add(buffer.Memory);
            }
            return builder.Build();
        }

        private readonly ReadOnlyMemory<byte> _memory;
        public ReadOnlyMemory<byte> Memory => _memory;

        private Octets(ReadOnlyMemory<byte> memory)
        {
            _memory = memory;
            _hashCodeFunc = new Lazy<int>(CalcHashCode);
        }

        /// <summary>
        /// Creates an immutable block, copying the source to a new internal buffer.
        /// </summary>
        /// <param name="source"></param>
        public Octets(ReadOnlySpan<byte> source)
        {
            _memory = source.ToArray();
            _hashCodeFunc = new Lazy<int>(CalcHashCode);
        }

        /// <summary>
        /// Creates an immutable block, copying the sources to a new internal buffer.
        /// </summary>
        /// <param name="source"></param>
        public Octets(ReadOnlySpan<byte> source1, ReadOnlySpan<byte> source2)
        {
            byte[] buffer = new byte[source1.Length + source2.Length];
            Span<byte> span = buffer.AsSpan();
            Span<byte> span1 = span.Slice(0, source1.Length);
            Span<byte> span2 = span.Slice(source1.Length, source2.Length);
            source1.CopyTo(span1);
            source2.CopyTo(span2);
            _memory = buffer;
            _hashCodeFunc = new Lazy<int>(CalcHashCode);
        }

        /// <summary>
        /// Creates an immutable block, copying the sources to a new internal buffer.
        /// </summary>
        /// <param name="source"></param>
        public Octets(ReadOnlySpan<byte> source1, ReadOnlySpan<byte> source2, ReadOnlySpan<byte> source3)
        {
            byte[] buffer = new byte[source1.Length + source2.Length + source3.Length];
            Span<byte> span = buffer.AsSpan();
            Span<byte> span1 = span.Slice(0, source1.Length);
            Span<byte> span2 = span.Slice(source1.Length, source2.Length);
            Span<byte> span3 = span.Slice(source1.Length + source2.Length, source3.Length);
            source1.CopyTo(span1);
            source2.CopyTo(span2);
            source3.CopyTo(span3);
            _memory = buffer;
            _hashCodeFunc = new Lazy<int>(CalcHashCode);
        }

        public Octets(ReadOnlySequence<byte> sequence)
        {
            if (sequence.IsEmpty)
            {
                _memory = ReadOnlyMemory<byte>.Empty;
            }
            else if (sequence.IsSingleSegment)
            {
                _memory = sequence.First;
            }
            else
            {
                Memory<byte> buffer = new byte[sequence.Length];
                int offset = 0;
                foreach (var segment in sequence)
                {
                    segment.CopyTo(buffer.Slice(offset));
                    offset += segment.Length;
                }
                _memory = buffer;
            }
            _hashCodeFunc = new Lazy<int>(CalcHashCode);
        }

        public (Octets head, Octets rest) GetHead(int headLength)
        {
            return (
                UnsafeWrap(_memory.Slice(0, headLength)),
                UnsafeWrap(_memory.Slice(headLength)));
        }

        public (Octets rest, Octets tail) GetTail(int tailLength)
        {
            int restLength = (Memory.Length >= tailLength) ? Memory.Length - tailLength : 0;
            return (UnsafeWrap(_memory.Slice(0, restLength)), UnsafeWrap(_memory.Slice(restLength)));
        }

        public (Octets head, Octets body, Octets tail) GetHeadAndBody(int headLength, int bodyLength)
        {
            var (head, rest) = this.GetHead(headLength);
            var (body, tail) = rest.GetHead(bodyLength);
            return (head, body, tail);
        }

        public (Octets head, Octets body, Octets tail) GetHeadAndTail(int headLength, int tailLength)
        {
            var (head, rest) = this.GetHead(headLength);
            var (body, tail) = rest.GetTail(tailLength);
            return (head, body, tail);
        }

        [Obsolete("Deprecated, Use GetHeadAndBody or GetHeadAndTail")]
        public (Octets head, Octets body, Octets tail) GetHeadTail(int headLength, int bodyLength)
        {
            return (
                UnsafeWrap(_memory.Slice(0, headLength)),
                UnsafeWrap(_memory.Slice(headLength, bodyLength)),
                UnsafeWrap(_memory.Slice(headLength + bodyLength)));
        }

        #region IEnumerable implementation
        private sealed class Enumerator : IEnumerator<byte>
        {
            private readonly ReadOnlyMemory<byte> _memory;
            private int _index = -1;

            public Enumerator(ReadOnlyMemory<byte> memory)
            {
                _memory = memory;
            }

            public byte Current => _memory.Span[_index];

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                return ++_index < _memory.Length;
            }

            public void Reset()
            {
                _index = -1;
            }
        }
        public IEnumerator<byte> GetEnumerator()
        {
            return new Enumerator(_memory);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region IReadOnlyList implementation
        public int Count => _memory.Length;
        public byte this[int index] => _memory.Span[index];
        #endregion

        #region IEquatable implementation
        private readonly Lazy<int> _hashCodeFunc;

        private int CalcHashCode()
        {
            var result = new HashCode();
            var span = _memory.Span;
            result.Add(span.Length);
#if NET6_0_OR_GREATER
            result.AddBytes(span);
#else
            for (int i = 0; i < span.Length; i++)
            {
                result.Add(span[i]);
            }
#endif
            return result.ToHashCode();
        }

        public override int GetHashCode()
        {
            return _hashCodeFunc.Value;
        }

        public override bool Equals(object? obj)
        {
            return obj is Octets other && Equals(other);
        }
        public bool Equals(Octets? other)
        {
            if (ReferenceEquals(other, this)) return true;
            if (other is null) return false;

            var thisSpan = _memory.Span;
            var thatSpan = other._memory.Span;
            if (thatSpan.Length != thisSpan.Length) return false;
            if (other.GetHashCode() != GetHashCode()) return false;
            return thatSpan.SequenceEqual(thisSpan);
        }

        public static bool operator ==(Octets? left, Octets? right) => left is null ? right is null : left.Equals(right);
        public static bool operator !=(Octets? left, Octets? right) => left is null ? right is not null : !left.Equals(right);

        #endregion

    }
}
