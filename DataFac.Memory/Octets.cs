using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;

namespace DataFac.Memory
{
    /// <summary>
    /// An immutable reference type that wraps a ReadOnlyMemory<byte> buffer.
    /// </summary>
    public sealed class Octets : IOctets, IEquatable<Octets>, IEnumerable<byte>
    {
        private static readonly Octets _empty = new Octets(ReadOnlySequence<byte>.Empty);
        public static Octets Empty => _empty;

        /// <summary>
        /// Wraps the source bytes *without* copying. This assumes the source will never change.
        /// </summary>
        /// <param name="source"></param>
        /// <returns>The wrapped buffer.</returns>
        public static Octets Wrap(ReadOnlyMemory<byte> source)
        {
            return source.IsEmpty ? _empty : new Octets(source);
        }
        public static Octets Wrap(ReadOnlySequence<byte> source)
        {
            return source.IsEmpty ? _empty : new Octets(source);
        }
        public static Octets Wrap(ImmutableArray<ReadOnlyMemory<byte>> source)
        {
            return source.IsEmpty ? _empty : new Octets(source);
        }

        /// <summary>
        /// Combines multiple Octets.
        /// </summary>
        public static Octets Combine(Octets source1, Octets source2)
        {
            return Octets.Wrap([.. source1.Buffers, .. source2.Buffers]);
        }

        /// <summary>
        /// Combines multiple Octets.
        /// </summary>
        public static Octets Combine(Octets source1, Octets source2, Octets source3)
        {
            return Octets.Wrap([.. source1.Buffers, .. source2.Buffers, .. source3.Buffers]);
        }

        /// <summary>
        /// Combines multiple Octets.
        /// </summary>
        public static Octets Combine(params Octets[] sources)
        {
            var builder = ImmutableArray.CreateBuilder<ReadOnlyMemory<byte>>();
            foreach (var source in sources)
            {
                foreach (var buffer in source.Buffers)
                {
                    builder.Add(buffer);
                }
            }
            return Octets.Wrap(builder.ToImmutable());
        }

        private readonly ImmutableArray<ReadOnlyMemory<byte>> _buffers;
        public ImmutableArray<ReadOnlyMemory<byte>> Buffers => _buffers;

        /// <summary>
        /// Returns a new instance with all internal buffers combined into
        /// a single buffer.
        /// </summary>
        /// <returns></returns>
        public Octets Compact()
        {
            if (_buffers.Length == 0 || _buffers.Length == 1) return this;
            Memory<byte> memory = new byte[_length];
            var span = memory.Span;
            int position = 0;
            foreach (var buffer in _buffers)
            {
                buffer.Span.CopyTo(span.Slice(position));
                position += buffer.Length;
            }
            return Octets.Wrap(memory);
        }

        /// <summary>
        /// Gets the internal sequence of bytes represented as a <see cref="System.Buffers.ReadOnlySequence{T}"/>.
        /// </summary>
        public ReadOnlySequence<byte> ToSequence()
        {
            ReadOnlySequenceBuilder<byte> builder = default;
            foreach (var buffer in _buffers)
            {
                if (buffer.Length > 0) builder = builder.Append(buffer);
            }
            return builder.Build();
        }

        private readonly int _length;
        public int Length => _length;

        public bool IsEmpty => _length == 0;
        public bool IsCompact => _buffers.Length <= 1;

        /// <summary>
        /// Converts the sequence into a single contiguous <see cref="ReadOnlyMemory{T}"/> of bytes.
        /// </summary>
        /// <remarks>This method consolidates the contents of the sequence into a single memory block.  If
        /// the sequence is empty, the method returns <see cref="ReadOnlyMemory{T}.Empty"/>. If the sequence consists of
        /// a single segment, the method returns that segment directly. For sequences with multiple segments, the method
        /// allocates a new memory block and copies  the contents of each segment into it.</remarks>
        /// <returns>A <see cref="ReadOnlyMemory{T}"/> containing the bytes from the sequence. Returns <see
        /// cref="ReadOnlyMemory{T}.Empty"/> if the sequence is empty.</returns>
        public ReadOnlyMemory<byte> AsMemory()
        {
            if (_buffers.Length == 0) return ReadOnlyMemory<byte>.Empty;
            if (_buffers.Length == 1) return _buffers[0];
            Memory<byte> result = new byte[_length];
            int position = 0;
            foreach (var buffer in _buffers)
            {
                buffer.CopyTo(result.Slice(position));
                position += buffer.Length;
            }
            return result;
        }

        /// <summary>
        /// Converts the sequence into a single byte array.
        /// </summary>
        /// <remarks>This method consolidates all segments of the sequence into a single contiguous byte
        /// array. If the sequence is empty, an empty array is returned. If the sequence contains only one segment, the
        /// segment is directly converted to an array for efficiency.</remarks>
        /// <returns>A byte array containing all the data from the sequence. Returns an empty array if the sequence is empty.</returns>
        public byte[] ToByteArray()
        {
            if (_buffers.Length == 0) return Array.Empty<byte>();
            if (_buffers.Length == 1) return _buffers[0].ToArray();
            byte[] result = new byte[_length];
            var span = result.AsSpan();
            int position = 0;
            foreach (var buffer in _buffers)
            {
                buffer.Span.CopyTo(span.Slice(position));
                position += buffer.Length;
            }
            return result;
        }

        private Octets(ReadOnlyMemory<byte> memory)
        {
            _buffers = [memory];
            _length = memory.Length;
            _hashCodeFunc = new Lazy<int>(CalcHashCode);
        }

        private Octets(ReadOnlySequence<byte> sequence)
        {
            var builder = ImmutableArray.CreateBuilder<ReadOnlyMemory<byte>>();
            int totalLength = 0;
            foreach (var buffer in sequence)
            {
                if (buffer.Length > 0)
                {
                    builder.Add(buffer);
                    totalLength += buffer.Length;
                }
            }
            _buffers = builder.ToImmutable();
            _length = totalLength;
            _hashCodeFunc = new Lazy<int>(CalcHashCode);
        }

        private Octets(ImmutableArray<ReadOnlyMemory<byte>> buffers)
        {
            _buffers = buffers;
            _length = buffers.Sum(b => b.Length);
            _hashCodeFunc = new Lazy<int>(CalcHashCode);
        }

        /// <summary>
        /// Creates an immutable block, copying the source to a new internal buffer.
        /// </summary>
        /// <param name="source"></param>
        public Octets(ReadOnlySpan<byte> source)
        {
            _buffers = ImmutableArray<ReadOnlyMemory<byte>>.Empty.Add(source.ToArray());
            _length = source.Length;
            _hashCodeFunc = new Lazy<int>(CalcHashCode);
        }

        /// <summary>
        /// Creates an immutable block, copying the sources to a new internal buffer.
        /// </summary>
        public Octets(ReadOnlySpan<byte> source1, ReadOnlySpan<byte> source2)
        {
            Memory<byte> buffer = new byte[source1.Length + source2.Length];
            var target = buffer.Span;
            int start = 0;
            source1.CopyTo(target.Slice(start));
            start += source1.Length;
            source2.CopyTo(target.Slice(start));
            start += source2.Length;
            _buffers = ImmutableArray<ReadOnlyMemory<byte>>.Empty.Add(buffer);
            _length = source1.Length + source2.Length;
            _hashCodeFunc = new Lazy<int>(CalcHashCode);
        }

        /// <summary>
        /// Creates an immutable block, copying the sources to a new internal buffer.
        /// </summary>
        /// <param name="source"></param>
        public Octets(ReadOnlySpan<byte> source1, ReadOnlySpan<byte> source2, ReadOnlySpan<byte> source3)
        {
            Memory<byte> buffer = new byte[source1.Length + source2.Length + source3.Length];
            var target = buffer.Span;
            int start = 0;
            source1.CopyTo(target.Slice(start));
            start += source1.Length;
            source2.CopyTo(target.Slice(start));
            start += source2.Length;
            source3.CopyTo(target.Slice(start));
            start += source3.Length;
            _buffers = ImmutableArray<ReadOnlyMemory<byte>>.Empty.Add(buffer);
            _length = source1.Length + source2.Length + source3.Length;
            _hashCodeFunc = new Lazy<int>(CalcHashCode);
        }

        public (Octets head, Octets rest) GetHead(int headLength)
        {
            if (headLength <= 0) return (Octets.Empty, this);
            if (_length < headLength) return (this, Octets.Empty);
            var headBuilder = ImmutableArray.CreateBuilder<ReadOnlyMemory<byte>>(_buffers.Length);
            var restBuilder = ImmutableArray.CreateBuilder<ReadOnlyMemory<byte>>(_buffers.Length);
            int thisPosition = 0;
            foreach (var buffer in _buffers)
            {
                // headLen thisPos bufSize  -> action
                //      10       0       4     head
                //      10       4      10     split 6/4
                //      10      14       6     tail
                if (thisPosition + buffer.Length <= headLength)
                {
                    // add to head
                    headBuilder.Add(buffer);
                }
                else if (thisPosition >= headLength)
                {
                    // add to tail
                    restBuilder.Add(buffer);
                }
                else
                {
                    // split
                    int splitLen = headLength - thisPosition;
                    headBuilder.Add(buffer.Slice(0, splitLen));
                    restBuilder.Add(buffer.Slice(splitLen));
                }
                thisPosition += buffer.Length;
            }
            return (Wrap(headBuilder.ToImmutable()), Wrap(restBuilder.ToImmutable()));
        }

        public (Octets rest, Octets tail) GetTail(int tailLength)
        {
            int headLength = _length - tailLength;
            return GetHead(headLength);
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

        #region IEquatable implementation
        private readonly Lazy<int> _hashCodeFunc;

        private int CalcHashCode()
        {
            var result = new HashCode();
            result.Add(_length);
            foreach (var buffer in _buffers)
            {
                var span = buffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    result.Add(span[i]);
                }
            }
            return result.ToHashCode();
        }

        public override int GetHashCode() => _hashCodeFunc.Value;
        public override bool Equals(object? obj) => obj is Octets other && Equals(other);

        /// <summary>
        /// Returns true if the content of this instance is equal to the other instance.
        /// todo do span-wise equality for better perf
        /// </summary>
        public bool Equals(Octets? other)
        {
            if (ReferenceEquals(other, this)) return true;
            if (other is null) return false;

            if (other.Length != _length) return false;

            var thisEnumerator = _buffers.GetEnumerator();
            var thisBuffer = ReadOnlySpan<byte>.Empty;
            int thisOffset = 0;

            var thatEnumerator = other._buffers.GetEnumerator();
            var thatBuffer = ReadOnlySpan<byte>.Empty;
            int thatOffset = 0;

            int position = 0;

            while (position < _length)
            {
                while (thisOffset >= thisBuffer.Length)
                {
                    if (!thisEnumerator.MoveNext()) return false;
                    thisBuffer = thisEnumerator.Current.Span;
                    thisOffset = 0;
                }

                while (thatOffset >= thatBuffer.Length)
                {
                    if (!thatEnumerator.MoveNext()) return false;
                    thatBuffer = thatEnumerator.Current.Span;
                    thatOffset = 0;
                }

                if (thatBuffer[thatOffset] != thisBuffer[thisOffset]) return false;

                // next
                position++;
                thisOffset++;
                thatOffset++;
            }

            return true;
        }

        private sealed class OctetsEnumerator : IEnumerator<byte>
        {
            private readonly ImmutableArray<ReadOnlyMemory<byte>> _buffers;
            private int _number;
            private int _offset;

            public OctetsEnumerator(ImmutableArray<ReadOnlyMemory<byte>> buffers)
            {
                _buffers = buffers;
                _number = 0;
                _offset = -1;
            }

            public void Dispose() { }

            public void Reset()
            {
                _number = 0;
                _offset = -1;
            }

            public byte Current => _buffers[_number].Span[_offset];
            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                _offset++;
                while (true)
                {
                    if (_number >= _buffers.Length) return false;
                    if (_offset < _buffers[_number].Length) return true;
                    // try next buffer
                    _number++;
                    _offset = 0;
                }
            }

        }

        public IEnumerator<byte> GetEnumerator() => new OctetsEnumerator(_buffers);

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static bool operator ==(Octets? left, Octets? right) => left is null ? right is null : left.Equals(right);
        public static bool operator !=(Octets? left, Octets? right) => left is null ? right is not null : !left.Equals(right);

#endregion

    }
}
