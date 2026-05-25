using System;
using System.Buffers;
using System.Collections.Generic;

namespace DataFac.Memory;

/// <summary>
/// A helper struct that supports efficient building of ReadOnlySequence of type T.
/// </summary>
/// <typeparam name="T"></typeparam>
#pragma warning disable CA1815 // Override equals and operator equals on value types
public readonly struct ReadOnlySequenceBuilder<T>
#pragma warning restore CA1815 // Override equals and operator equals on value types
{
#pragma warning disable CA1051 // Do not declare visible instance fields
    /// <summary>
    /// The first segment in the sequence.
    /// </summary>
    public readonly ReadOnlyMemorySegment<T>? First;
    /// <summary>
    /// The last segment in the sequence.
    /// </summary>
    public readonly ReadOnlyMemorySegment<T>? Last;
#pragma warning restore CA1051 // Do not declare visible instance fields

    /// <summary>
    /// Create a new instance of ReadOnlySequenceBuilder with no segments.
    /// </summary>
    public ReadOnlySequenceBuilder() { }

    private ReadOnlySequenceBuilder(ReadOnlyMemorySegment<T>? first, ReadOnlyMemorySegment<T>? last) : this()
    {
        First = first;
        Last = last;
    }

    /// <summary>
    /// Creates a new instance of ReadOnlySequenceBuilder with a single segment containing the provided memory.
    /// </summary>
    /// <param name="memory"></param>
    public ReadOnlySequenceBuilder(ReadOnlyMemory<T> memory) : this()
    {
        First = new ReadOnlyMemorySegment<T>(memory);
        Last = null;
    }

    /// <summary>
    /// Creates a new instance of ReadOnlySequenceBuilder with two segments containing the provided memories.
    /// </summary>
    /// <param name="memory1"></param>
    /// <param name="memory2"></param>
    public ReadOnlySequenceBuilder(ReadOnlyMemory<T> memory1, ReadOnlyMemory<T> memory2) : this()
    {
        First = new ReadOnlyMemorySegment<T>(memory1);
        Last = First.Append(memory2);
    }

    /// <summary>
    /// Creates a new instance of ReadOnlySequenceBuilder with three segments containing the provided memories.
    /// </summary>
    /// <param name="memory1"></param>
    /// <param name="memory2"></param>
    /// <param name="memory3"></param>
    public ReadOnlySequenceBuilder(ReadOnlyMemory<T> memory1, ReadOnlyMemory<T> memory2, ReadOnlyMemory<T> memory3) : this()
    {
        First = new ReadOnlyMemorySegment<T>(memory1);
        Last = First.Append(memory2).Append(memory3);
    }

    /// <summary>
    /// Creates a new instance of ReadOnlySequenceBuilder containing the provided buffers.
    /// </summary>
    /// <param name="buffers"></param>
    public ReadOnlySequenceBuilder(params ReadOnlyMemory<T>[] buffers) : this()
    {
#if NET8_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(buffers);
#else
        if (buffers is null) throw new ArgumentNullException(nameof(buffers));
#endif
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

    /// <summary>
    /// Creates a new instance of ReadOnlySequenceBuilder containing the provided buffers.
    /// </summary>
    /// <param name="buffers"></param>
    public ReadOnlySequenceBuilder(IEnumerable<ReadOnlyMemory<T>> buffers) : this()
    {
#if NET8_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(buffers);
#else
        if (buffers is null) throw new ArgumentNullException(nameof(buffers));
#endif
        int count = 0;
        foreach (var buffer in buffers)
        {
            if (count == 0)
            {
                First = new ReadOnlyMemorySegment<T>(buffer);
            }
            else if (count == 1)
            {
                Last = First!.Append(buffer);
            }
            else
            {
                Last = Last!.Append(buffer);
            }
            count++;
        }
    }

    /// <summary>
    /// Appends a buffer to the sequence builder.
    /// </summary>
    /// <param name="memory"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Builds and returns the ReadOnlySequence of T.
    /// </summary>
    /// <returns></returns>
    public ReadOnlySequence<T> Build()
    {
        if (First is null) return ReadOnlySequence<T>.Empty;
        if (Last is null) return new ReadOnlySequence<T>(First.Memory);
        return new ReadOnlySequence<T>(First, 0, Last, Last.Memory.Length);
    }
}
