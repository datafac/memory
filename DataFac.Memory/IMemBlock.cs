using System;

namespace DataFac.Memory
{
    public interface IMemBlock
    {
        int BlockSize { get; }
        bool IsEmpty { get; }
        bool TryRead(ReadOnlySpan<byte> source);
        bool TryWrite(Span<byte> target);
        void WriteTo(Span<byte> target);
        void WriteTo(int start, int length, Span<byte> target);
        string ToBase64String();
        string ToBase64String(int start, int length);
        string UTF8String { get; set; }

        // todo ByteString
    }
}