using System;
using System.Buffers;

namespace DataFac.Memory
{
    /// <summary>
    /// An implementation of ReadOnlySequenceSegment\<T\>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class ReadOnlyMemorySegment<T> : ReadOnlySequenceSegment<T>
    {
        public ReadOnlyMemorySegment(ReadOnlyMemory<T> memory) => Memory = memory;
        public ReadOnlyMemorySegment<T> Append(ReadOnlyMemory<T> memory)
        {
            var segment = new ReadOnlyMemorySegment<T>(memory) { RunningIndex = RunningIndex + Memory.Length };
            Next = segment;
            return segment;
        }
    }
}
