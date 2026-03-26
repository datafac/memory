using System;
using System.Buffers;
using System.Collections.Immutable;

namespace DataFac.Memory
{
    public interface IOctets
    {
        bool IsEmpty { get; }
        bool IsCompact { get; }
        int Length { get; }
        ImmutableArray <ReadOnlyMemory<byte>> Buffers { get; }
        ReadOnlySequence<byte> ToSequence();
        ReadOnlyMemory<byte> AsMemory();
    }
}
