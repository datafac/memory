using System;
using System.Buffers;

namespace DataFac.Memory;

/// <summary>
/// An implementation of ReadOnlySequenceSegment of type T.
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class ReadOnlyMemorySegment<T> : ReadOnlySequenceSegment<T>
{
    /// <summary>
    /// Creates a new segment with the provided memory. 
    /// </summary>
    /// <param name="memory"></param>
    public ReadOnlyMemorySegment(ReadOnlyMemory<T> memory) => Memory = memory;

    /// <summary>
    /// Appends a memory segment to the current segment and returns the new segment.
    /// </summary>
    /// <param name="memory"></param>
    /// <returns></returns>
    public ReadOnlyMemorySegment<T> Append(ReadOnlyMemory<T> memory)
    {
        var segment = new ReadOnlyMemorySegment<T>(memory) { RunningIndex = RunningIndex + Memory.Length };
        Next = segment;
        return segment;
    }
}
