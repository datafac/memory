using System;
using System.Runtime.InteropServices;

namespace DataFac.Memory
{
    [StructLayout(LayoutKind.Explicit, Size = 64)]
    public struct BlockB064 : IMemBlock
    {
        [FieldOffset(0)] public BlockB032 A;
        [FieldOffset(32)] public BlockB032 B;

        public bool TryRead(ReadOnlySpan<byte> source) => MemoryMarshal.TryRead(source.Slice(0, 64), out this);
        public bool TryWrite(Span<byte> target) => MemoryMarshal.TryWrite(target.Slice(0, 64),
#if NET8_0_OR_GREATER
            in this);
#else
            ref this);
#endif
    }

}