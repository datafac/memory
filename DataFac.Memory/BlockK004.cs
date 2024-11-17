using System;
using System.Runtime.InteropServices;

namespace DataFac.Memory
{
    [StructLayout(LayoutKind.Explicit, Size = 4 * 1024)]
    public struct BlockK004 : IMemBlock
    {
        [FieldOffset(0)]
        public BlockK002 A;
        [FieldOffset(2 * 1024)]
        public BlockK002 B;

        public bool TryRead(ReadOnlySpan<byte> source) => MemoryMarshal.TryRead(source.Slice(0, 4 * 1024), out this);
        public bool TryWrite(Span<byte> target) => MemoryMarshal.TryWrite(target.Slice(0, 4 * 1024),
#if NET8_0_OR_GREATER
            in this);
#else
            ref this);
#endif
    }

}