using System;
using System.Buffers;

namespace DataFac.Memory
{
    /// <summary>
    /// A helper struct that supports efficient building of ReadOnlySequence\<T\>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public readonly struct ReadOnlySequenceBuilder<T>
    {
        public readonly ReadOnlyMemorySegment<T>? First;
        public readonly ReadOnlyMemorySegment<T>? Last;

        public ReadOnlySequenceBuilder() { }

        private ReadOnlySequenceBuilder(ReadOnlyMemorySegment<T>? first, ReadOnlyMemorySegment<T>? last) : this()
        {
            First = first;
            Last = last;
        }

        public ReadOnlySequenceBuilder(ReadOnlyMemory<T> memory) : this()
        {
            First = new ReadOnlyMemorySegment<T>(memory);
            Last = null;
        }

        public ReadOnlySequenceBuilder(ReadOnlyMemory<T> memory1, ReadOnlyMemory<T> memory2) : this()
        {
            First = new ReadOnlyMemorySegment<T>(memory1);
            Last = First.Append(memory2);
        }

        public ReadOnlySequenceBuilder(ReadOnlyMemory<T> memory1, ReadOnlyMemory<T> memory2, ReadOnlyMemory<T> memory3) : this()
        {
            First = new ReadOnlyMemorySegment<T>(memory1);
            Last = First.Append(memory2).Append(memory3);
        }

        public ReadOnlySequenceBuilder(params ReadOnlyMemory<T>[] buffers) : this()
        {
            if (buffers.Length >= 1)
            {
                First = new ReadOnlyMemorySegment<T>(buffers[0]);
            }
            if (buffers.Length >= 2)
            {
                Last = First!.Append(buffers[1]);
            }
            for (int i = 2; i < buffers.Length; i++)
            {
                Last = Last!.Append(buffers[i]);
            }
        }

        public ReadOnlySequenceBuilder<T> Append(ReadOnlyMemory<T> memory)
        {
            if (First is null)
            {
                return new ReadOnlySequenceBuilder<T>(memory);
            }
            else if (Last is null)
            {
                var last = First.Append(memory);
                return new ReadOnlySequenceBuilder<T>(First, last);
            }
            else
            {
                var last = Last.Append(memory);
                return new ReadOnlySequenceBuilder<T>(First, last);
            }
        }

        public ReadOnlySequence<T> Build()
        {
            if (First is null) return ReadOnlySequence<T>.Empty;
            if (Last is null) return new ReadOnlySequence<T>(First.Memory);
            return new ReadOnlySequence<T>(First, 0, Last, Last.Memory.Length);
        }
    }
}
