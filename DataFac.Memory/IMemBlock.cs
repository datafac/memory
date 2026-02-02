using System;
using System.Buffers;
using System.Collections.Generic;

namespace DataFac.Memory;

public interface IMemBlock
{
    int BlockSize { get; }
    bool IsEmpty { get; }
    bool TryRead(ReadOnlySpan<byte> source);
    bool TryRead(ReadOnlySequence<byte> source);
    bool TryWrite(Span<byte> target);
    void WriteTo(Span<byte> target);
    void WriteTo(int start, int length, Span<byte> target);
    string ToBase64String(Base64FormattingOptions options = Base64FormattingOptions.None);
    string ToBase64String(int start, int length, Base64FormattingOptions options = Base64FormattingOptions.None);
    byte[] ToByteArray();
    byte[] ToByteArray(int start, int length);
    string UTF8String { get; set; }

    // todo ByteString
}
