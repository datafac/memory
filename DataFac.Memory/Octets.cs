using System;
using System.Buffers;

namespace DataFac.Memory
{
    /// <summary>
    /// An immutable reference type that wraps a ReadOnlyMemory<byte> buffer.
    /// </summary>
    public sealed class Octets : IEquatable<Octets?>
    {
        private static readonly Octets _empty = new Octets(ReadOnlySequence<byte>.Empty);
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
        public static Octets UnsafeWrap(ReadOnlySequence<byte> source)
        {
            return source.Length == 0 ? _empty : new Octets(source);
        }

        /// <summary>
        /// Combines multiple Octets.
        /// </summary>
        public static Octets Combine(Octets source1, Octets source2)
        {
            ReadOnlySequenceBuilder<byte> builder = default;
            foreach (var buffer in source1.Sequence)
            {
                if (buffer.Length > 0) builder = builder.Append(buffer);
            }
            foreach (var buffer in source2.Sequence)
            {
                if (buffer.Length > 0) builder = builder.Append(buffer);
            }
            return new Octets(builder.Build());
        }

        /// <summary>
        /// Combines multiple Octets.
        /// </summary>
        public static Octets Combine(Octets source1, Octets source2, Octets source3)
        {
            ReadOnlySequenceBuilder<byte> builder = default;
            foreach (var buffer in source1.Sequence)
            {
                if (buffer.Length > 0) builder = builder.Append(buffer);
            }
            foreach (var buffer in source2.Sequence)
            {
                if (buffer.Length > 0) builder = builder.Append(buffer);
            }
            foreach (var buffer in source3.Sequence)
            {
                if (buffer.Length > 0) builder = builder.Append(buffer);
            }
            return new Octets(builder.Build());
        }

        /// <summary>
        /// Combines multiple Octets.
        /// </summary>
        public static Octets Combine(params Octets[] sources)
        {
            ReadOnlySequenceBuilder<byte> builder = default;
            foreach (var source in sources)
            {
                foreach (var buffer in source.Sequence)
                {
                    if (buffer.Length > 0) builder = builder.Append(buffer);
                }
            }
            return new Octets(builder.Build());
        }

        private readonly ReadOnlySequence<byte> _sequence;

        /// <summary>
        /// Gets the internal sequence of bytes represented as a <see cref="System.Buffers.ReadOnlySequence{T}"/>.
        /// </summary>
        public ReadOnlySequence<byte> Sequence => _sequence;
        public long Length => _sequence.Length;

        [Obsolete("This property has been deprecated. Instead use the AsMemory() method.", true)]
        public ReadOnlyMemory<byte> Memory => AsMemory();

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
            if (_sequence.IsEmpty) return ReadOnlyMemory<byte>.Empty;
            if (_sequence.IsSingleSegment) return _sequence.First;
            Memory<byte> result = new byte[_sequence.Length];
            int position = 0;
            foreach (var buffer in _sequence)
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
            if (_sequence.IsEmpty) return Array.Empty<byte>();
            if (_sequence.IsSingleSegment) return _sequence.First.ToArray();
            byte[] result = new byte[_sequence.Length];
            int position = 0;
            foreach (var buffer in _sequence)
            {
                buffer.Span.CopyTo(result.AsSpan(position));
                position += buffer.Length;
            }
            return result;
        }

        private Octets(ReadOnlyMemory<byte> memory)
        {
            _sequence = new ReadOnlySequence<byte>(memory);
            _hashCodeFunc = new Lazy<int>(CalcHashCode);
        }

        private Octets(ReadOnlySequence<byte> sequence)
        {
            _sequence = sequence;
            _hashCodeFunc = new Lazy<int>(CalcHashCode);
        }

        /// <summary>
        /// Creates an immutable block, copying the source to a new internal buffer.
        /// </summary>
        /// <param name="source"></param>
        public Octets(ReadOnlySpan<byte> source)
        {
            _sequence = new ReadOnlySequence<byte>(source.ToArray());
            _hashCodeFunc = new Lazy<int>(CalcHashCode);
        }

        /// <summary>
        /// Creates an immutable block, copying the sources to a new internal buffer.
        /// </summary>
        public Octets(ReadOnlySpan<byte> source1, ReadOnlySpan<byte> source2)
        {
            byte[] buffer = new byte[source1.Length + source2.Length];
            Span<byte> span = buffer.AsSpan();
            Span<byte> span1 = span.Slice(0, source1.Length);
            Span<byte> span2 = span.Slice(source1.Length, source2.Length);
            source1.CopyTo(span1);
            source2.CopyTo(span2);
            _sequence = new ReadOnlySequence<byte>(new ReadOnlyMemory<byte>(buffer));
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
            _sequence = new ReadOnlySequence<byte>(new ReadOnlyMemory<byte>(buffer));
            _hashCodeFunc = new Lazy<int>(CalcHashCode);
        }

        public (Octets head, Octets rest) GetHead(int headLength)
        {
            return (UnsafeWrap(_sequence.Slice(0, headLength)), UnsafeWrap(_sequence.Slice(headLength)));
        }

        public (Octets rest, Octets tail) GetTail(int tailLength)
        {
            long restLength = (_sequence.Length >= tailLength) ? (_sequence.Length - tailLength) : 0;
            return (UnsafeWrap(_sequence.Slice(0, restLength)), UnsafeWrap(_sequence.Slice(restLength)));
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
                UnsafeWrap(_sequence.Slice(0, headLength)),
                UnsafeWrap(_sequence.Slice(headLength, bodyLength)),
                UnsafeWrap(_sequence.Slice(headLength + bodyLength)));
        }

        #region IEquatable implementation
        private readonly Lazy<int> _hashCodeFunc;

        private int CalcHashCode()
        {
            var result = new HashCode();
            result.Add(_sequence.Length);
            foreach (var buffer in _sequence)
            {
                var span = buffer.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    result.Add(span[i]);
                }
            }
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

            if (other.Sequence.Length != _sequence.Length) return false;

            var thisEnumerator = _sequence.GetEnumerator();
            var thisBuffer = ReadOnlySpan<byte>.Empty;
            int thisOffset = 0;

            var thatEnumerator = other.Sequence.GetEnumerator();
            var thatBuffer = ReadOnlySpan<byte>.Empty;
            int thatOffset = 0;

            int position = 0;

            while (position < _sequence.Length)
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

                //next
                position++;
                thisOffset++;
                thatOffset++;
            }

            return true;
        }

        public static bool operator ==(Octets? left, Octets? right) => left is null ? right is null : left.Equals(right);
        public static bool operator !=(Octets? left, Octets? right) => left is null ? right is not null : !left.Equals(right);

        #endregion

    }
}
