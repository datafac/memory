using System;
using System.Buffers;

namespace DataFac.Memory;

/// <summary>
/// ReadOnlySequence extension methods.
/// </summary>
public static class ReadOnlySequenceExtensions
{
    /// <summary>
    /// Converts a ReadOnlySequence to a single ReadOnlyMemory.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static ReadOnlyMemory<T> Compact<T>(this ReadOnlySequence<T> sequence)
    {
        if (sequence.IsEmpty) return ReadOnlyMemory<T>.Empty;
        if (sequence.IsSingleSegment) return sequence.First;
        // combine all segments
        Memory<T> result = new T[sequence.Length];
        int pos = 0;
        foreach (var buffer in sequence)
        {
            buffer.CopyTo(result.Slice(pos));
            pos += buffer.Length;
        }
        return result;
    }
}
