using System;

namespace DataFac.Memory
{
    public interface IMemBlock
    {
        bool TryRead(ReadOnlySpan<byte> source);
        bool TryWrite(Span<byte> target);
#if NET6_0_OR_GREATER
        string? UTF8String { get; set; }
#endif
    }

}