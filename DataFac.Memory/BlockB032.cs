using System;
using System.Runtime.InteropServices;

namespace DataFac.Memory
{
    [StructLayout(LayoutKind.Explicit, Size = 32)]
    public struct BlockB032 : IMemBlock
    {
        [FieldOffset(0)] public BlockB016 A;
        [FieldOffset(16)] public BlockB016 B;

        public bool TryRead(ReadOnlySpan<byte> source) => MemoryMarshal.TryRead(source.Slice(0, 32), out this);
        public bool TryWrite(Span<byte> target) => MemoryMarshal.TryWrite(target.Slice(0, 32),
#if NET8_0_OR_GREATER
            in this);
#else
            ref this);
#endif
    }

}