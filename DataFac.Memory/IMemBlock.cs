using System;

namespace DataFac.Memory
{
    public interface IMemBlock
    {
        bool TryRead(ReadOnlySpan<byte> source);
        bool TryWrite(Span<byte> target);
        string UTF8String { get; set; }
    }

}