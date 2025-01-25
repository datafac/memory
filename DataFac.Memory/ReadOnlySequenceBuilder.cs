using System;
using System.Buffers;

namespace DataFac.Memory
{
    /// <summary>
    /// A helper struct that supports efficient building of ReadOnlySequence\<T\>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct ReadOnlySequenceBuilder<T>
    {
        public ReadOnlyMemorySegment<T>? First { get; private set; } = null;
        public ReadOnlyMemorySegment<T>? Last { get; private set; } = null;

        public ReadOnlySequenceBuilder() { }

        public ReadOnlySequenceBuilder(ReadOnlyMemory<T> memory) : this()
        {
            First = new ReadOnlyMemorySegment<T>(memory);
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

        public void Add(ReadOnlyMemory<T> memory)
        {
            if (First is null)
            {
                First = new ReadOnlyMemorySegment<T>(memory);
            }
            else if (Last is null)
            {
                Last = First.Append(memory);
            }
            else
            {
                Last = Last.Append(memory);
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
