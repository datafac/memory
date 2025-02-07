using System;

namespace DataFac.Memory
{
    public interface IMemBlock
    {
        int BlockSize { get; }
        bool IsEmpty { get; }
        bool TryRead(ReadOnlySpan<byte> source);
        bool TryWrite(Span<byte> target);
        string UTF8String { get; set; }

        // todo ByteString
    }
}